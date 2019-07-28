
# this is where the cert file will be saved
$Path = "pdm.pawau.pfx"

# you'll need this password to load the PFX file later
$Password = Read-Host -Prompt 'Enter new password to protect certificate' -AsSecureString

# create cert, export to file, then delete again
$cert = New-SelfSignedCertificate -KeyUsage DigitalSignature -KeySpec Signature -FriendlyName 'PowerAppsWebApiUtils' -Subject CN=PowerAppsWebApiUtils -KeyExportPolicy ExportableEncrypted -CertStoreLocation Cert:\CurrentUser\My
$cert | Export-PfxCertificate -Password $Password -FilePath $Path
#$cert | Remove-Item