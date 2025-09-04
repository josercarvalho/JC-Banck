$baseUrl = "http://localhost:5000"


Write-Host "Criando conta corrente..."
$createAccountBody = @{
    nome = "Teste Usuario"
    senha = "senha123"
} | ConvertTo-Json

$createAccountResponse = Invoke-RestMethod -Uri "$baseUrl/contacorrente" -Method Post -Body $createAccountBody -ContentType "application/json"
Write-Host "Conta criada com número: $($createAccountResponse.numeroConta)"


Write-Host "\nFazendo login..."
$loginBody = @{
    numeroConta = $createAccountResponse.numeroConta
    senha = "senha123"
} | ConvertTo-Json

$loginResponse = Invoke-RestMethod -Uri "$baseUrl/contacorrente/login" -Method Post -Body $loginBody -ContentType "application/json"
Write-Host "Token JWT obtido: $($loginResponse.token)"


$headers = @{
    Authorization = "Bearer $($loginResponse.token)"
}


Write-Host "\nRealizando depósito..."
$depositoBody = @{
    idRequisicao = [guid]::NewGuid().ToString()
    idContaCorrente = $null  # Será preenchido pelo servidor com base no token
    valor = 1000
    tipoMovimento = "C"
} | ConvertTo-Json

Invoke-RestMethod -Uri "$baseUrl/contacorrente/movimentacao" -Method Post -Body $depositoBody -ContentType "application/json" -Headers $headers
Write-Host "Depósito realizado com sucesso!"


Write-Host "\nCriando segunda conta corrente..."
$createAccount2Body = @{
    nome = "Destinatario Teste"
    senha = "senha456"
} | ConvertTo-Json

$createAccount2Response = Invoke-RestMethod -Uri "$baseUrl/contacorrente" -Method Post -Body $createAccount2Body -ContentType "application/json"
Write-Host "Segunda conta criada com número: $($createAccount2Response.numeroConta)"


Write-Host "\nRealizando transferência..."
$transferBody = @{
    idRequisicao = [guid]::NewGuid().ToString()
    idContaCorrenteOrigem = $null  # Será preenchido pelo servidor com base no token
    idContaCorrenteDestino = $null  # Será preenchido pelo servidor com base no número da conta
    valor = 500
} | ConvertTo-Json

Invoke-RestMethod -Uri "$baseUrl/transferencia" -Method Post -Body $transferBody -ContentType "application/json" -Headers $headers
Write-Host "Transferência realizada com sucesso!"

Write-Host "\nTestes concluídos com sucesso!"