@echo off
@echo Deleting all BIN and OBJ foldes
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S bin') DO RMDIR /S /Q "%%G"
FOR /F "tokens=*" %%G IN ('DIR /B /AD /S obj') DO RMDIR /S /Q "%%G"
@echo BIN and OBJ folders successfully deleted :) Close the window.
pause > nul