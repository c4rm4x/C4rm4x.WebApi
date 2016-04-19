Param (
    [Parameter(Mandatory = $true)]
    [string]$subscriber,
    [string]$appConfig = "app.config"
)


function Get-AppConfig {

    Param(
        [string]$file = 'app.config'
    )

    $appConfig = [xml](cat $file)
    $appConfig.configuration.connectionStrings.add | foreach{
        return $_.connectionString
    }
}

function Get-Password {

    Param(
        [int]$length=10,
        [string[]]$sourceData
    )

    for ($loop = 1; $loop –le $length; $loop++) {
        $password += ($sourceData | GET-RANDOM)
    }

    return $password
}

function Get-Base64 {

    Param(
        [string]$text
    )
    
    $bytes = [System.Text.Encoding]::Unicode.GetBytes($text)
    $encodedText = [Convert]::ToBase64String($bytes)
    
    return $encodedText
}

function Get-Md5Hash {

    Param(
        [string]$text
    )
    
    $md5 = new-object -TypeName System.Security.Cryptography.MD5CryptoServiceProvider
    $utf8 = new-object -TypeName System.Text.UTF8Encoding
    $hash = $md5.ComputeHash($utf8.GetBytes($text)) | foreach {
        $result += $_.ToString("x2")
    }

    return $result
}

function Get-Subscribers {
    
    Param(
        [string]$subscriber,
        [string]$connectionString
    )

    $sqlQuery = "SELECT count(1) FROM dbo.Subscribers WHERE Name = '$subscriber'"
    $sqlConnection = New-Object System.Data.SqlClient.SqlConnection 
    $sqlConnection.ConnectionString = $connectionString
    $sqlCmd = New-Object System.Data.SqlClient.SqlCommand 
    $sqlCmd.CommandText = $sqlQuery 
    $sqlCmd.Connection = $sqlConnection
    $sqlConnection.Open() 
    $rows = [Int32]$sqlCmd.ExecuteScalar()
    $sqlConnection.Close()
    
    return $rows    
}

function Insert-Subscriber {
    
    Param(
        [string]$subscriber,
        [string]$connectionString,
        [string]$secret
    )

    $sqlQuery = "INSERT INTO dbo.Subscribers VALUES ('$subscriber', '$secret')"
    $sqlConnection = New-Object System.Data.SqlClient.SqlConnection 
    $sqlConnection.ConnectionString = $connectionString
    $sqlCmd = New-Object System.Data.SqlClient.SqlCommand 
    $sqlCmd.CommandText = $sqlQuery 
    $sqlCmd.Connection = $sqlConnection
    $sqlConnection.Open() 
    $rows = [Int32]$sqlCmd.ExecuteNonQuery()
    $sqlConnection.Close()
    
    return $rows    
}




$alphabet=$NULL;for ($a = 65; $a –le 90; $a++) { $alphabet +=,[char][byte]$a }

$connectionString = Get-AppConfig -file $appConfig
$password = Get-Password -length 40 -sourceData $alphabet
$base64 = Get-Base64 -text $password
$hash = Get-Md5Hash -text $password
$numberOfSubscribers = Get-Subscribers -subscriber $subscriber -connectionString $connectionString


if ($numberOfSubscribers -eq 0) {
    $success = Insert-Subscriber -subscriber $subscriber -connectionString $connectionString -secret $hash

    if ($success -eq 1) {
        Write-Host "Include this as the subscriber's password in your config file: $base64"
    }
    else {
        Write-Host "Some weird error..."
    }        
}
else {
    Write-Host "The subscriber with name $subscriber already exists"
}
