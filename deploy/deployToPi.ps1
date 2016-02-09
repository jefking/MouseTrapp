$secpasswd = ConvertTo-SecureString "C@nITPr01" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("Administrator", $secpasswd)

# net use w: \\mousetrapp\c$\MouseTrapp\ /user:Administrator C@nITPr01
write-host "W drive Mount" 
Remove-Item W:\deploy\* -Recurse

xcopy C:\Data\Hackathon\agent\_work\1\b\AppxPackages\MouseTrapp.IOT_2.0.0.0_ARM_Test W:\deploy /i /s

Invoke-Command -ComputerName mousetrapp -ScriptBlock { shutdown /r /t 0 } -credential $mycreds

# net use w: /Delete