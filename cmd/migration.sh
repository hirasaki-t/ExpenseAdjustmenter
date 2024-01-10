cd $(cd $(dirname $0); pwd)
pwsh -ExecutionPolicy RemoteSigned -File "./ps1/migration.ps1"

read -s -k