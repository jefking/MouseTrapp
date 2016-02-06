$secpasswd = ConvertTo-SecureString "C@nITPr01" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("Administrator", $secpasswd)

net use w: \\mousetrapp\c$\MouseTrapp\ /user:Administrator C@nITPr01

Remove-Item W:\deploy\* -Recurse

xcopy C:\Data\Hackathon\MouseTrapp\MouseTrapp.IOT\MouseTrapp.IOT\AppPackages\MouseTrapp.IOT_1.0.2.0_ARM_Test W:\deploy /i /s

Invoke-Command -ComputerName mousetrapp -ScriptBlock { ping google.com } -credential $mycreds