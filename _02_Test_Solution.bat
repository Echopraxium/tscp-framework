@echo off
:: TSCP Solution Orchestrator
:: Orchestrates validation in PowerShell and launches services in dedicated windows.

echo [1/3] LAUNCHING TESTS IN POWERSHELL...
:: Launches PowerShell, runs tests, and stays open so you can inspect results.
start powershell -NoExit -Command "& { Write-Host '--- TSCP INTEGRITY SUITE ---' -ForegroundColor Cyan; dotnet test; Write-Host '--- Inspect results above before interacting with CLI ---' -ForegroundColor Yellow }"

echo [2/3] STARTING TSCP SERVER (PORT 5000)...
:: Starts the Giraffe server in a separate CMD window.
start "TSCP-Server" cmd /k "dotnet run --project src/TSCP.Server/TSCP.Server.fsproj"

echo [3/3] STARTING TSCP CLI...
:: Starts the interactive CLI in a separate CMD window.
start "TSCP-CLI" cmd /k "dotnet run --project src/TSCP.CLI/TSCP.CLI.fsproj"

echo.
echo Launching sequence complete. 
echo - Check the PowerShell window for test traces.
echo - Use the TSCP-CLI window for commands.
echo - The TSCP-Server window displays HTTP logs.
echo.
pause