$eps = '/api/portfolio/resumo','/api/ativos','/api/ordens','/api/watchlist'
foreach ($e in $eps) {
  try {
    $r = Invoke-WebRequest "http://localhost:5000$e" -UseBasicParsing -TimeoutSec 15
    Write-Host "$e => $($r.StatusCode) OK"
  } catch {
    $resp = $_.Exception.Response
    $code = if ($resp) { [int]$resp.StatusCode } else { -1 }
    $body = ''
    if ($resp) { $body = (New-Object IO.StreamReader($resp.GetResponseStream())).ReadToEnd() }
    Write-Host "$e => $code ERRO"
    if ($body) { Write-Host "   corpo: $body" }
  }
}
