echo off
cd %~dp0
pwsh -ExecutionPolicy RemoteSigned -File "./ps1/migration.ps1"

pause
