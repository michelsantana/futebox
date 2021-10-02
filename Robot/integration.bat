REM DIRETORIO DO ROBO
set arg1=%1
REM TIPO DE COMANDO
set arg2=%2
REM PARAMETROS
set arg3=%3
set arg4=%4
cd /d %arg1%
REM pause
start integration.step2.bat %arg2% %arg3% %arg4%