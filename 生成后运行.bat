taskkill /F /IM DSPGAME.EXE
set ModPath=C:\Users\zheng\AppData\Roaming\r2modmanPlus-local\DysonSphereProgram\profiles\Default\BepInEx\plugins\Test\
set ZIPPath=$(SolutionDir)\Upload\$(ProjectName)
MKDIR %ZIPPath%
MKDIR %MODPath%
copy /y $(TargetPath) %MODPath%\$(TargetFileName)
copy /y $(TargetPath) %ZIPPath%\$(TargetFileName)
