Write-Host '--- portas ---'
Get-NetTCPConnection -State Listen -LocalPort 5000,5173 -ErrorAction SilentlyContinue | Select-Object LocalPort,OwningProcess | Format-Table | Out-String | Write-Host
Write-Host '--- connection strings ---'
Get-ChildItem backend/src/PortfolioApi/appsettings*.json | ForEach-Object {
  Write-Host "# $($_.Name)"
  Get-Content $_.FullName | Write-Host
}
Write-Host '--- arquivos de banco ---'
Get-ChildItem -Recurse -Include *.db,*.sqlite,*.db3 backend -ErrorAction SilentlyContinue | Select-Object -ExpandProperty FullName | Write-Host
