@echo off
:: uuid: f8a1e2b3-c4d5-4e5f-a6b7-8c9d0e1f2a3b
:: This script launches a PowerShell window to perform a deep rebuild.
@echo off
:: uuid: e8f4a2b1-c3d4-4b1e-9a2f-7c8b9d0e1f2a

:: Define the PowerShell logic in a variable to avoid CMD line-continuation bugs
set "PS_LOGIC=$p='TSCP.CLI'; Write-Host '--- TSCP REBUILD SHIELD ---' -Fore Cyan; if (Get-Process $p -EA 0) { Write-Host '' ; Write-Host 'ERROR: ' $p ' is currently running.' -Fore Red; Write-Host 'This process is locking bin/obj folders. Please close it.' -Fore Yellow; return }; Write-Host 'No lock detected. Starting deep clean...' -Fore Green; Get-ChildItem -Path . -Include bin,obj -Recurse | Remove-Item -Recurse -Force; dotnet clean; dotnet restore; dotnet build; Write-Host '' ; Write-Host 'Rebuild completed successfully.' -Fore Green"

:: Launch PowerShell in a new window (Blue background) with the logic
start powershell -NoExit -Command "& { %PS_LOGIC% }"