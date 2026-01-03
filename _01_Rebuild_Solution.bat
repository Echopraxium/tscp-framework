@echo off
:: uuid: e8f4a2b1-c3d4-4b1e-9a2f-7c8b9d0e1f2a
:: This script launches a PowerShell window to perform a deep rebuild safely.
:: It checks for running TSCP processes (CLI, Server, GUI) to prevent file locking.
:: It cleans bin, obj, and the hidden .vs folder (Visual Studio cache).

:: Define the PowerShell logic in a single variable.
:: Logic: Check list of processes -> If any running, list them and exit -> Clean bin/obj/.vs -> dotnet rebuild.
set "PS_LOGIC=$targets = 'TSCP.CLI','TSCP.Server','TSCP.GUI'; Write-Host '--- TSCP REBUILD SHIELD ---' -Fore Cyan; $running = Get-Process -Name $targets -ErrorAction SilentlyContinue; if ($running) { Write-Host '' ; Write-Host 'ERROR: The following processes are running:' -Fore Red; $running | ForEach-Object { Write-Host (' - ' + $_.ProcessName) -Fore Yellow }; Write-Host 'Please close them to release file locks.' -Fore Red; return }; Write-Host 'No locks detected. Starting deep clean...' -Fore Green; Get-ChildItem -Path . -Include bin,obj -Recurse | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue; if (Test-Path .\.vs) { Remove-Item -Path .\.vs -Recurse -Force -ErrorAction SilentlyContinue; Write-Host 'Visual Studio cache (.vs) cleaned.' -Fore Gray }; dotnet clean; dotnet restore; dotnet build; Write-Host '' ; Write-Host 'Rebuild completed successfully.' -Fore Green"

:: Launch PowerShell in a new window with the logic
start powershell -NoExit -Command "& { %PS_LOGIC% }"