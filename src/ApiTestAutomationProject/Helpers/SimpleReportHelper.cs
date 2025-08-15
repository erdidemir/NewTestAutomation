using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq; // Added for .Count() and .Count(predicate)

namespace ApiTestAutomationProject.Helpers
{
    public static class SimpleReportHelper
    {
        private static readonly string _reportPath = Path.Combine(Directory.GetCurrentDirectory(), "simple-reports");
        private static readonly List<TestResult> _testResults = new();
        private static readonly object _lockObject = new();

        public class TestResult
        {
            public string TestName { get; set; } = "";
            public string Description { get; set; } = "";
            public string Status { get; set; } = "";
            public string Duration { get; set; } = "";
            public List<string> Logs { get; set; } = new();
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        private static TestResult? _currentTest;

        public static void StartTest(string testName, string description = "")
        {
            lock (_lockObject)
            {
                _currentTest = new TestResult
                {
                    TestName = testName,
                    Description = description,
                    StartTime = DateTime.Now,
                    Status = "Running"
                };
            }
        }

        public static void LogInfo(string message)
        {
            lock (_lockObject)
            {
                _currentTest?.Logs.Add($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        public static void LogPass(string message)
        {
            lock (_lockObject)
            {
                _currentTest?.Logs.Add($"[PASS] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        public static void LogFail(string message)
        {
            lock (_lockObject)
            {
                _currentTest?.Logs.Add($"[FAIL] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        public static void LogWarning(string message)
        {
            lock (_lockObject)
            {
                _currentTest?.Logs.Add($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        public static void LogError(string message)
        {
            lock (_lockObject)
            {
                _currentTest?.Logs.Add($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        public static void EndTest(string status = "Completed")
        {
            lock (_lockObject)
            {
                if (_currentTest != null)
                {
                    _currentTest.EndTime = DateTime.Now;
                    _currentTest.Status = status;
                    _currentTest.Duration = (_currentTest.EndTime - _currentTest.StartTime).ToString(@"mm\:ss\.fff");
                    _testResults.Add(_currentTest);
                    _currentTest = null;
                }
            }
        }

        public static void GenerateReport()
        {
            lock (_lockObject)
            {
                if (!Directory.Exists(_reportPath))
                {
                    Directory.CreateDirectory(_reportPath);
                }

                var html = GenerateHtmlReport();
                var reportFile = Path.Combine(_reportPath, $"test-report-{DateTime.Now:yyyyMMdd-HHmmss}.html");
                File.WriteAllText(reportFile, html, Encoding.UTF8);

                // Ana index.html dosyasını da güncelle
                var indexFile = Path.Combine(_reportPath, "index.html");
                File.WriteAllText(indexFile, html, Encoding.UTF8);
            }
        }

        private static string GenerateHtmlReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='tr'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset='UTF-8'>");
            sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            sb.AppendLine("    <title>API Test Automation Report</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine("        body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; }");
            sb.AppendLine("        .container { max-width: 1200px; margin: 0 auto; background-color: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }");
            sb.AppendLine("        h1 { color: #333; text-align: center; border-bottom: 2px solid #007acc; padding-bottom: 10px; }");
            sb.AppendLine("        .summary { background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin-bottom: 20px; }");
            sb.AppendLine("        .test-result { border: 1px solid #ddd; margin: 10px 0; border-radius: 5px; overflow: hidden; }");
            sb.AppendLine("        .test-header { padding: 10px; background-color: #f8f9fa; border-bottom: 1px solid #ddd; cursor: pointer; }");
            sb.AppendLine("        .test-header:hover { background-color: #e9ecef; }");
            sb.AppendLine("        .test-content { padding: 10px; display: none; }");
            sb.AppendLine("        .test-content.show { display: block; }");
            sb.AppendLine("        .log-entry { margin: 2px 0; padding: 2px 5px; border-radius: 3px; }");
            sb.AppendLine("        .log-info { background-color: #d1ecf1; }");
            sb.AppendLine("        .log-pass { background-color: #d4edda; color: #155724; }");
            sb.AppendLine("        .log-fail { background-color: #f8d7da; color: #721c24; }");
            sb.AppendLine("        .log-warn { background-color: #fff3cd; color: #856404; }");
            sb.AppendLine("        .log-error { background-color: #f5c6cb; color: #721c24; }");
            sb.AppendLine("        .status-passed { color: #28a745; font-weight: bold; }");
            sb.AppendLine("        .status-failed { color: #dc3545; font-weight: bold; }");
            sb.AppendLine("        .status-running { color: #ffc107; font-weight: bold; }");
            sb.AppendLine("        .timestamp { color: #6c757d; font-size: 0.9em; }");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("    <div class='container'>");
            sb.AppendLine("        <h1>API Test Automation Report</h1>");
            
            // Summary
            var totalTests = _testResults.Count;
            var passedTests = _testResults.Count(t => t.Status == "Passed");
            var failedTests = _testResults.Count(t => t.Status == "Failed");
            var runningTests = _testResults.Count(t => t.Status == "Running");

            sb.AppendLine("        <div class='summary'>");
            sb.AppendLine($"            <h3>Test Summary</h3>");
            sb.AppendLine($"            <p><strong>Total Tests:</strong> {totalTests}</p>");
            sb.AppendLine($"            <p><strong>Passed:</strong> <span class='status-passed'>{passedTests}</span></p>");
            sb.AppendLine($"            <p><strong>Failed:</strong> <span class='status-failed'>{failedTests}</span></p>");
            sb.AppendLine($"            <p><strong>Running:</strong> <span class='status-running'>{runningTests}</span></p>");
            sb.AppendLine($"            <p><strong>Generated:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
            sb.AppendLine("        </div>");

            // Test Results
            foreach (var test in _testResults)
            {
                var statusClass = test.Status.ToLower() switch
                {
                    "passed" => "status-passed",
                    "failed" => "status-failed",
                    "running" => "status-running",
                    _ => ""
                };

                sb.AppendLine("        <div class='test-result'>");
                sb.AppendLine($"            <div class='test-header' onclick='toggleTest(this)'>");
                sb.AppendLine($"                <strong>{test.TestName}</strong> - <span class='{statusClass}'>{test.Status}</span>");
                sb.AppendLine($"                <span class='timestamp'>({test.Duration})</span>");
                sb.AppendLine("            </div>");
                sb.AppendLine("            <div class='test-content'>");
                
                if (!string.IsNullOrEmpty(test.Description))
                {
                    sb.AppendLine($"                <p><strong>Description:</strong> {test.Description}</p>");
                }
                
                sb.AppendLine($"                <p><strong>Start Time:</strong> {test.StartTime:yyyy-MM-dd HH:mm:ss}</p>");
                sb.AppendLine($"                <p><strong>End Time:</strong> {test.EndTime:yyyy-MM-dd HH:mm:ss}</p>");
                
                if (test.Logs.Count > 0)
                {
                    sb.AppendLine("                <h4>Test Logs:</h4>");
                    foreach (var log in test.Logs)
                    {
                        var logClass = log.Contains("[PASS]") ? "log-pass" :
                                      log.Contains("[FAIL]") ? "log-fail" :
                                      log.Contains("[WARN]") ? "log-warn" :
                                      log.Contains("[ERROR]") ? "log-error" : "log-info";
                        
                        sb.AppendLine($"                <div class='log-entry {logClass}'>{log}</div>");
                    }
                }
                
                sb.AppendLine("            </div>");
                sb.AppendLine("        </div>");
            }

            sb.AppendLine("    </div>");
            sb.AppendLine("    <script>");
            sb.AppendLine("        function toggleTest(element) {");
            sb.AppendLine("            var content = element.nextElementSibling;");
            sb.AppendLine("            content.classList.toggle('show');");
            sb.AppendLine("        }");
            sb.AppendLine("    </script>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
