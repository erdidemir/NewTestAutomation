using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Serilog;

namespace TestAutomationProject.Core
{
    public class PerformanceMonitor
    {
        private static readonly Dictionary<string, Stopwatch> _timers = new();
        private static readonly Dictionary<string, List<long>> _metrics = new();
        private static readonly object _lock = new();
        private static Process _currentProcess;

        static PerformanceMonitor()
        {
            _currentProcess = Process.GetCurrentProcess();
        }

        public static void StartTimer(string operationName)
        {
            lock (_lock)
            {
                if (!_timers.ContainsKey(operationName))
                {
                    _timers[operationName] = Stopwatch.StartNew();
                }
                else
                {
                    _timers[operationName].Restart();
                }
            }
        }

        public static long StopTimer(string operationName)
        {
            lock (_lock)
            {
                if (_timers.ContainsKey(operationName))
                {
                    _timers[operationName].Stop();
                    var elapsed = _timers[operationName].ElapsedMilliseconds;
                    
                    if (!_metrics.ContainsKey(operationName))
                    {
                        _metrics[operationName] = new List<long>();
                    }
                    _metrics[operationName].Add(elapsed);
                    
                    Log.Information($"Performance: {operationName} completed in {elapsed}ms");
                    return elapsed;
                }
                return 0;
            }
        }

        public static PerformanceMetrics GetCurrentMetrics()
        {
            _currentProcess.Refresh();
            
            return new PerformanceMetrics
            {
                CpuUsage = _currentProcess.TotalProcessorTime.TotalMilliseconds,
                MemoryUsageMB = _currentProcess.WorkingSet64 / 1024 / 1024,
                ThreadCount = _currentProcess.Threads.Count,
                ProcessTime = _currentProcess.UserProcessorTime.TotalMilliseconds,
                Timestamp = DateTime.Now
            };
        }

        public static Dictionary<string, PerformanceStatistics> GetPerformanceStatistics()
        {
            var statistics = new Dictionary<string, PerformanceStatistics>();
            
            lock (_lock)
            {
                foreach (var metric in _metrics)
                {
                    var values = metric.Value;
                    if (values.Count > 0)
                    {
                        statistics[metric.Key] = new PerformanceStatistics
                        {
                            OperationName = metric.Key,
                            AverageTime = values.Average(),
                            MinTime = values.Min(),
                            MaxTime = values.Max(),
                            TotalExecutions = values.Count,
                            TotalTime = values.Sum()
                        };
                    }
                }
            }
            
            return statistics;
        }

        public static void LogPerformanceMetrics()
        {
            var metrics = GetCurrentMetrics();
            var statistics = GetPerformanceStatistics();
            
            Log.Information("=== Performance Metrics ===");
            Log.Information($"CPU Usage: {metrics.CpuUsage:F2}ms");
            Log.Information($"Memory Usage: {metrics.MemoryUsageMB:F2}MB");
            Log.Information($"Thread Count: {metrics.ThreadCount}");
            Log.Information($"Process Time: {metrics.ProcessTime:F2}ms");
            
            Log.Information("=== Performance Statistics ===");
            foreach (var stat in statistics.Values)
            {
                Log.Information($"{stat.OperationName}: Avg={stat.AverageTime:F2}ms, Min={stat.MinTime}ms, Max={stat.MaxTime}ms, Count={stat.TotalExecutions}");
            }
        }

        public static void ClearMetrics()
        {
            lock (_lock)
            {
                _timers.Clear();
                _metrics.Clear();
            }
        }
    }

    public class PerformanceMetrics
    {
        public double CpuUsage { get; set; }
        public double MemoryUsageMB { get; set; }
        public int ThreadCount { get; set; }
        public double ProcessTime { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class PerformanceStatistics
    {
        public string OperationName { get; set; }
        public double AverageTime { get; set; }
        public long MinTime { get; set; }
        public long MaxTime { get; set; }
        public int TotalExecutions { get; set; }
        public long TotalTime { get; set; }
    }
} 