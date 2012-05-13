@echo off
copy README.md readme.txt
NuGet.exe pack HttpHelper.nuspec -verbose
pause
