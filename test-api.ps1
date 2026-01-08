$loginBody = Get-Content 'C:\Users\AfzalAhmed\source\repos\dotnetdeveloper20xx\LeagueMembershipManagementPortal\test-login.json' -Raw
$response = Invoke-RestMethod -Uri 'http://localhost:7000/api/auth/login' -Method Post -ContentType 'application/json' -Body $loginBody
$token = $response.data.token
$headers = @{'Authorization' = "Bearer $token"}
$members = Invoke-RestMethod -Uri 'http://localhost:7000/api/members?pageSize=5' -Headers $headers
Write-Output "First 5 members:"
$members.items | ForEach-Object { Write-Output "$($_.email) - $($_.fullName)" }
