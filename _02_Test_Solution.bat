@echo off
:: uuid: b2c3d4e5-f6a7-4890-b1c2-d3e4f5a6b7c8
:: This script launches a PowerShell window to run the Integrity Suite first.
:: If TSCP.Tests pass (Green), it automatically launches the M3 Engine (Server) and the Interface (CLI).

:: Define paths relative to the root folder
set TEST_PROJECT=src/TSCP.Tests/TSCP.Tests.fsproj
set SERVER_PROJECT=src/TSCP.Server/TSCP.Server.fsproj
set CLI_PROJECT=src/TSCP.CLI/TSCP.CLI.csproj

:: Define the PowerShell logic
:: 1. Run Tests on the specific Test Project (Cleaner output)
:: 2. If Success ($LASTEXITCODE=0), Launch Server and CLI
:: 3. If Fail, Pause to let the user see the red lines.

set "PS_LOGIC=Write-Host '--- TSCP CORE 4 INTEGRITY SUITE ---' -Fore Cyan; Write-Host '[1/3] Verifying Systemic Axioms (M3/M2)...' -Fore Gray; dotnet test %TEST_PROJECT% --nologo; if ($LASTEXITCODE -eq 0) { Write-Host ''; Write-Host 'Axioms VALIDATED. Initializing Continuum...' -Fore Green; Start-Sleep -Seconds 1; Write-Host '[2/3] Starting TSCP.Server (M3 Engine)...' -Fore Cyan; Start-Process cmd -ArgumentList '/k title TSCP-Server & dotnet run --project %SERVER_PROJECT%'; Write-Host '[3/3] Starting TSCP.CLI (Observer Interface)...' -Fore Cyan; Start-Process cmd -ArgumentList '/k title TSCP-CLI & dotnet run --project %CLI_PROJECT%'; Write-Host ''; Write-Host 'System Online.' -Fore Green } else { Write-Host ''; Write-Host 'CRITICAL FAILURE: Axioms broken. Aborting launch.' -Fore Red; Write-Host 'Review the errors above.' -Fore Yellow; Read-Host 'Press Enter to exit...' }"

:: Launch PowerShell
start powershell -NoExit -Command "& { %PS_LOGIC% }"