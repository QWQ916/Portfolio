$base = "http://localhost:2121/api/v1/notes"

function Get-Notes {
    Invoke-RestMethod -Uri $base
}

function New-Note($title, $text) {
    $body = @{ title = $title; text = $text } | ConvertTo-Json
    Invoke-RestMethod -Uri $base -Method Post -Body $body `
                      -ContentType "application/json; charset=utf-8"
}

function Remove-Note($id) {
    try {
        Invoke-WebRequest -Uri "$base/$id" -Method Delete | Out-Null
        "Удалена заметка $id"
    }
    catch {
        "Не удалось: " + $_.Exception.Response.StatusCode.value__
    }
}