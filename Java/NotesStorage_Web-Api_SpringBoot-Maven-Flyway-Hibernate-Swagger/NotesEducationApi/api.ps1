# ---------------------------------------------------------------
#  Notes API - helpers for PowerShell (5.1 and 7+)
#
#  Load once per session (dot, space, path):
#      . .\api.ps1
#
#  Then:
#      New-Note "Test" "Some text"
#      Get-Notes
#      Find-Notes "test"
#      Remove-Note 6
# ---------------------------------------------------------------

$base = "http://localhost:2121/api/v1/notes"

# Make the console able to print non-ASCII at all.
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding           = [System.Text.Encoding]::UTF8

# ---------------------------------------------------------------
#  Core: always send and read bytes as UTF-8 explicitly.
#  Windows PowerShell 5.1 otherwise decodes the response as
#  Latin-1 and turns Cyrillic into garbage.
# ---------------------------------------------------------------
function Invoke-Api {
    param(
        [string] $Method,
        [string] $Url,
        [string] $Json
    )

    $params = @{
        Uri             = $Url
        Method          = $Method
        UseBasicParsing = $true
    }

    if ($Json) {
        $params.Body        = [System.Text.Encoding]::UTF8.GetBytes($Json)
        $params.ContentType = "application/json; charset=utf-8"
    }

    try {
        $response = Invoke-WebRequest @params
    }
    catch {
        $code = $_.Exception.Response.StatusCode.value__
        Write-Host ("HTTP " + $code) -ForegroundColor Red
        if ($_.ErrorDetails.Message) { Write-Host $_.ErrorDetails.Message }
        return
    }

    $bytes = $response.RawContentStream.ToArray()
    if ($bytes.Length -eq 0) {
        Write-Host ("HTTP " + $response.StatusCode + " (no content)") -ForegroundColor Green
        return
    }

    [System.Text.Encoding]::UTF8.GetString($bytes) | ConvertFrom-Json
}

# ---------------------------------------------------------------
#  Commands
# ---------------------------------------------------------------

function Get-Notes {
    Invoke-Api -Method Get -Url $base
}

function Get-Note($id) {
    Invoke-Api -Method Get -Url "$base/$id"
}

function Find-Notes($query, $sort = "date") {
    Invoke-Api -Method Get -Url "$base`?str=$query&sort=$sort"
}

function New-Note($title, $text) {
    $json = @{ title = $title; text = $text } | ConvertTo-Json
    Invoke-Api -Method Post -Url $base -Json $json
}

function Remove-Note($id) {
    Invoke-Api -Method Delete -Url "$base/$id"
}

Write-Host "Notes API helpers loaded: Get-Notes, Get-Note, Find-Notes, New-Note, Remove-Note" -ForegroundColor Cyan
Write-Host ("Base: " + $base) -ForegroundColor DarkGray
