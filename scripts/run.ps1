# Executa backend (ASP.NET Core) e frontend (Vite) da carteira B3.
# Uso:  ./scripts/run.ps1
#
# Abre duas janelas do PowerShell: uma para a API e outra para o frontend.
# Backend:  http://localhost:5000  (Swagger em /swagger)
# Frontend: http://localhost:5173

$ErrorActionPreference = 'Stop'

# Raiz do repositório (pasta pai de /scripts).
$root = Split-Path -Parent $PSScriptRoot
$backend = Join-Path $root 'backend\src\PortfolioApi'
$frontend = Join-Path $root 'frontend'

# Pré-requisitos.
foreach ($cmd in 'dotnet', 'npm') {
    if (-not (Get-Command $cmd -ErrorAction SilentlyContinue)) {
        throw "'$cmd' não encontrado no PATH. Instale-o antes de continuar."
    }
}

# Instala dependências do frontend na primeira execução.
if (-not (Test-Path (Join-Path $frontend 'node_modules'))) {
    Write-Host 'Instalando dependências do frontend...' -ForegroundColor Cyan
    Push-Location $frontend
    npm install
    Pop-Location
}

$shell = Get-Command pwsh -ErrorAction SilentlyContinue
if (-not $shell) {
    $shell = Get-Command powershell -ErrorAction SilentlyContinue
}

if (-not $shell) {
    throw "PowerShell não encontrado no PATH. Instale o PowerShell 7 (pwsh) ou use o Windows PowerShell (powershell.exe)."
}

Write-Host 'Iniciando backend (http://localhost:5000)...' -ForegroundColor Green
Start-Process -FilePath $shell.Source -WorkingDirectory $backend -ArgumentList '-NoExit', '-Command', 'dotnet run'

Write-Host 'Iniciando frontend (http://localhost:5173)...' -ForegroundColor Green
Start-Process -FilePath $shell.Source -WorkingDirectory $frontend -ArgumentList '-NoExit', '-Command', 'npm run dev'

Write-Host ''
Write-Host 'Aplicação iniciada em duas janelas separadas.' -ForegroundColor Yellow
Write-Host '  Backend : http://localhost:5000  (Swagger em /swagger)'
Write-Host '  Frontend: http://localhost:5173'
Write-Host 'Feche as janelas do PowerShell para encerrar os serviços.'
