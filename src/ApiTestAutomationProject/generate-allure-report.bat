@echo off
echo Generating Allure Report...

REM Testleri çalıştır
dotnet test ApiTestAutomationProject.csproj --logger "trx;LogFileName=test-results.trx"

REM Allure report oluştur
if exist "allure-results" (
    echo Allure results found, generating report...
    allure generate allure-results --clean -o allure-report
    echo Allure report generated successfully!
    echo Opening report...
    allure open allure-report
) else (
    echo No allure results found. Please run tests first.
)

pause 