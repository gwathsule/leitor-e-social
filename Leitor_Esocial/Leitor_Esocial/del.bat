@echo off

forfiles /P C:\Users\%username%\AppData\Roaming\AssinadorESocial /c "cmd /c del @path /q & rd @path /s /q"

exit

