
$jaVer = "8.0_73"
$dir = Split-Path -parent $MyInvocation.MyCommand.Definition

$env:CLASSPATH= ".;$env:CLASSPATH"
$env:Path= "C:\Program Files\Java\jdk1.$jaVer\bin;$env:Path"

exit
