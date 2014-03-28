@echo off
 
:: check for illegal charecters
if "%1"=="" goto USAGE
echo %1 | find /v ":" | find /v "\" | find /v "*" | find /v "?" | find /v "," | find /v ";" | find /v "/"  | find "%1" > nul
if errorlevel 1 goto USAGE
 
:: actual 'which' logic
for %%a in (.;%pathext%) do for %%b in (%1%%a) do ( echo %%~f$PATH:b | find /i "%1" )
goto END
 
:USAGE
:: Help screen:
 
echo.
echo UNIX-like which utility for Windows
echo Written by Ibrahim - www.digitalinternals.com
echo.
echo Usage:  which  executable_name
echo.
echo you may specify executable_name with or without
echo extension, but without a drive, path,
echo spaces or wildcards character(s).
echo.
 
:END
