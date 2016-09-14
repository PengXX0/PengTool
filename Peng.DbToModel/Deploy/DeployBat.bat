@echo off
set destination="..\Deploy\Apps.v%Date:~0,4%%Date:~5,2%%Date:~8,2%%Time:~0,2%%Time:~3,2%%Time:~6,2%"
xcopy "..\bin\Release" %destination% /exclude:exclude.txt /E/I/Y
pause