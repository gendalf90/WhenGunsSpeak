[cmdletbinding(SupportsShouldProcess=$true)]
param($publishProperties=@{}, $packOutput, $pubProfilePath)

# to learn more about this file visit https://go.microsoft.com/fwlink/?LinkId=524327

try{
    if ($publishProperties['ProjectGuid'] -eq $null){
        $publishProperties['ProjectGuid'] = '02a17ecd-61da-4ed2-a2ed-03c581f8c291'
    }

    $publishModulePath = Join-Path (Split-Path $MyInvocation.MyCommand.Path) 'publish-module.psm1'
    Import-Module $publishModulePath -DisableNameChecking -Force

    # call Publish-AspNet to perform the publish operation
    Publish-AspNet -publishProperties $publishProperties -packOutput $packOutput -pubProfilePath $pubProfilePath

	$login = ""
	$passwd = ""
	$addr = ""

	$secPasswd = ConvertTo-SecureString $passwd -AsPlainText -Force
	$cred = New-Object System.Management.Automation.PSCredential($login, $secPasswd)

	$session = New-SSHSession -ComputerName $addr -Credential $cred -Verbose

	$from = $packOutput
	$to = "/home/gendalf/node_test"

    Invoke-SSHCommand -SSHSession $session -Command "rm -r $($to)" -Verbose
	Set-SCPFolder -LocalFolder $from -RemoteFolder $to -ComputerName $addr -Credential $cred -Verbose
	Invoke-SSHCommand -SSHSession $session -Command "echo $($passwd) | sudo -S systemctl restart node-test" -Verbose
	Remove-SSHSession -SSHSession $session -Verbose
}
catch{
    "An error occurred during publish.`n{0}" -f $_.Exception.Message | Write-Error
}