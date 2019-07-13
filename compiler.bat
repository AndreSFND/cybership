echo OFF
cls
SET PATH=%PATH%;c:\windows\Microsoft.NET\Framework\v3.5

csc /o /out:Cybership.exe /target:exe /win32icon:res\icon.ico files\*.cs /reference:lib\sfmlnet-graphics-2.dll,lib\sfmlnet-system-2.dll,lib\sfmlnet-window-2.dll,lib\System.Data.SQLite.dll,lib\MySql.Data.dll,lib\System.Net.Http.Formatting.dll,lib\Gma.QrCodeNet.Encoding.Net35.dll
pause


REM /target:winexe	Sem Console
REM /target:exe		Com Console