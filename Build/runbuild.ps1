cls
$parms = @{
	'buildFile'='.\build.ps1';
	'taskList'=$args[0];
}
Import-Module '..\Tools\PSake\psake.psm1'
Invoke-psake @parms
Remove-Module psake