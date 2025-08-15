@echo off
echo Generating Extent Report...

REM Testleri çalıştır
dotnet test ApiTestAutomationProject.csproj

REM Extent report klasörünü kontrol et
if exist "extent-reports" (
    echo Extent report generated successfully!
    echo Opening report...
    start "" "extent-reports\index.html"
) else (
    echo No extent report found. Please run tests first.
)

pause
