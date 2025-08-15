@echo off
echo Generating Simple HTML Report...

REM Testleri çalıştır
dotnet test ApiTestAutomationProject.csproj

REM Simple report klasörünü kontrol et
if exist "simple-reports" (
    echo Simple HTML report generated successfully!
    echo Opening report...
    start "" "simple-reports\index.html"
) else (
    echo No simple report found. Please run tests first.
)

pause
