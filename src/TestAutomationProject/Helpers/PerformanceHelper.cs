using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Serilog;
using TestAutomationProject.Core;

namespace TestAutomationProject.Helpers
{
    public static class PerformanceHelper
    {
        private static readonly string _performanceReportPath = Path.Combine(Directory.GetCurrentDirectory(), "Results", "performance-report.json");
        
        public static void GeneratePerformanceReport()
        {
            try
            {
                var metrics = PerformanceMonitor.GetCurrentMetrics();
                var statistics = PerformanceMonitor.GetPerformanceStatistics();
                
                var report = new PerformanceReport
                {
                    Timestamp = DateTime.Now,
                    Metrics = metrics,
                    Statistics = statistics,
                    Summary = GenerateSummary(statistics)
                };
                
                // JSON raporu oluştur
                var jsonString = JsonSerializer.Serialize(report, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
                
                // Results klasörünü oluştur
                var resultsDir = Path.GetDirectoryName(_performanceReportPath);
                if (!Directory.Exists(resultsDir))
                {
                    Directory.CreateDirectory(resultsDir);
                }
                
                File.WriteAllText(_performanceReportPath, jsonString);
                
                Log.Information($"Performans raporu oluşturuldu: {_performanceReportPath}");
                
                // Console'a özet bilgileri yazdır
                LogPerformanceSummary(report);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Performans raporu oluşturulurken hata oluştu");
            }
        }
        
        private static PerformanceSummary GenerateSummary(Dictionary<string, PerformanceStatistics> statistics)
        {
            var summary = new PerformanceSummary();
            
            if (statistics.Count > 0)
            {
                var allTimes = new List<double>();
                var browserTimes = new List<double>();
                var elementTimes = new List<double>();
                
                foreach (var stat in statistics.Values)
                {
                    allTimes.Add(stat.AverageTime);
                    
                    if (stat.OperationName.Contains("Browser") || stat.OperationName.Contains("Driver"))
                    {
                        browserTimes.Add(stat.AverageTime);
                    }
                    
                    if (stat.OperationName.Contains("Element") || stat.OperationName.Contains("Wait"))
                    {
                        elementTimes.Add(stat.AverageTime);
                    }
                }
                
                summary.TotalOperations = statistics.Count;
                summary.AverageOperationTime = allTimes.Count > 0 ? allTimes.Average() : 0;
                summary.SlowestOperation = allTimes.Count > 0 ? allTimes.Max() : 0;
                summary.FastestOperation = allTimes.Count > 0 ? allTimes.Min() : 0;
                summary.AverageBrowserTime = browserTimes.Count > 0 ? browserTimes.Average() : 0;
                summary.AverageElementTime = elementTimes.Count > 0 ? elementTimes.Average() : 0;
            }
            
            return summary;
        }
        
        private static void LogPerformanceSummary(PerformanceReport report)
        {
            Log.Information("=== PERFORMANS RAPORU ÖZETİ ===");
            Log.Information($"Rapor Tarihi: {report.Timestamp:yyyy-MM-dd HH:mm:ss}");
            Log.Information($"Memory Kullanımı: {report.Metrics.MemoryUsageMB:F2}MB");
            Log.Information($"Thread Sayısı: {report.Metrics.ThreadCount}");
            Log.Information($"Toplam İşlem: {report.Summary.TotalOperations}");
            Log.Information($"Ortalama İşlem Süresi: {report.Summary.AverageOperationTime:F2}ms");
            Log.Information($"En Hızlı İşlem: {report.Summary.FastestOperation:F2}ms");
            Log.Information($"En Yavaş İşlem: {report.Summary.SlowestOperation:F2}ms");
            Log.Information($"Ortalama Browser Süresi: {report.Summary.AverageBrowserTime:F2}ms");
            Log.Information($"Ortalama Element Süresi: {report.Summary.AverageElementTime:F2}ms");
            Log.Information("=================================");
        }
        
        public static void StartElementTimer(string elementName)
        {
            PerformanceMonitor.StartTimer($"Element_{elementName}");
        }
        
        public static long StopElementTimer(string elementName)
        {
            return PerformanceMonitor.StopTimer($"Element_{elementName}");
        }
        
        public static void StartPageLoadTimer(string pageName)
        {
            PerformanceMonitor.StartTimer($"PageLoad_{pageName}");
        }
        
        public static long StopPageLoadTimer(string pageName)
        {
            return PerformanceMonitor.StopTimer($"PageLoad_{pageName}");
        }
        
        public static void StartActionTimer(string actionName)
        {
            PerformanceMonitor.StartTimer($"Action_{actionName}");
        }
        
        public static long StopActionTimer(string actionName)
        {
            return PerformanceMonitor.StopTimer($"Action_{actionName}");
        }
        
        public static void LogSlowOperations(double thresholdMs = 5000)
        {
            var statistics = PerformanceMonitor.GetPerformanceStatistics();
            var slowOperations = new List<PerformanceStatistics>();
            
            foreach (var stat in statistics.Values)
            {
                if (stat.AverageTime > thresholdMs)
                {
                    slowOperations.Add(stat);
                }
            }
            
            if (slowOperations.Count > 0)
            {
                Log.Warning($"=== YAVAŞ İŞLEMLER ({thresholdMs}ms üzeri) ===");
                foreach (var operation in slowOperations)
                {
                    Log.Warning($"{operation.OperationName}: Ortalama={operation.AverageTime:F2}ms, En Yavaş={operation.MaxTime}ms");
                }
            }
        }
        
        public static void ClearPerformanceData()
        {
            PerformanceMonitor.ClearMetrics();
            Log.Information("Performans verileri temizlendi");
        }
    }
    
    public class PerformanceReport
    {
        public DateTime Timestamp { get; set; }
        public PerformanceMetrics Metrics { get; set; }
        public Dictionary<string, PerformanceStatistics> Statistics { get; set; }
        public PerformanceSummary Summary { get; set; }
    }
    
    public class PerformanceSummary
    {
        public int TotalOperations { get; set; }
        public double AverageOperationTime { get; set; }
        public double SlowestOperation { get; set; }
        public double FastestOperation { get; set; }
        public double AverageBrowserTime { get; set; }
        public double AverageElementTime { get; set; }
    }
} 