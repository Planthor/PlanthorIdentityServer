New-SelfSignedCertificate -Subject "CN=localhost" -FriendlyName "PlanthorDevelopmentCertificate" -KeyUsage DigitalSignature,KeyEncipherment -NotBefore (Get-Date) -NotAfter (Get-Date).AddYears(4) -KeyLength 2048 -KeyAlgorithm RSA

$mypwd = ConvertTo-SecureString -String '<Password>' -Force -AsPlainText

Get-ChildItem -path 'Cert:\LocalMachine\My\<Cert ThumbprintId>' | Export-PfxCertificate -FilePath aspnetapp.pfx -Password $mypwd

