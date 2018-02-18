@ECHO OFF
ECHO DotRas Release Builder
ECHO Copyright (c) Jeff Winn. All rights reserved.
ECHO.

REM *****************************************************************
REM * CONFIGURATION
REM *****************************************************************

REM # Specifies the relative or absolute path to a strong name key file which will be used to sign the assemblies.
REM # NOTE: This value cannot be omitted.
SET KEYFILE=E:\Private Keys\JeffWinn.snk

REM # Specifies the build configuration which will be used. (Debug, Release)
SET CONFIG=Release

REM # Indicates whether unit tests should be executed. (True, False)
REM # This option requires Microsoft Visual Studio 9.0 (2008) Professional or Team Edition installed
REM # on the build machine. Also may require additional software used in the unit tests.
SET RUNTESTS=True

REM # Indicates whether help documentation will be generated. (True, False)
REM # This option requires Sandcastle and Sandcastle Help File Builder to both be installed 
REM # on the build machine.
SET MAKEDOCS=True

REM # Specifies the starting build version to compile.
REM # Possible values (in order): WIN2K, WINXP, WINXPSP2, WIN2K8
SET BUILDVER=WIN2K

REM # Indicates whether the installer will be generated upon completion. (True, False)
REM # NOTE: This option requires the WiX toolset (http://wix.sourceforge.net) to be installed.
SET MAKEINSTALLER=True

REM # Specifies the relative or absolute path where all binaries and source code will be dropped.
SET OUTFOLDER=.\Output

REM *****************************************************************
REM DO NOT MODIFY THE CONTENTS OF THIS FILE BELOW THIS POINT!!
REM *****************************************************************

SET SHFBFILE=.\Help\DotRas.shfb
SET CHMFILE=.\Help\Output\DotRas.chm
SET SHFBPATH=%PROGRAMFILES%\Sandcastle Help File Builder
SET FXFOLDER=%SYSTEMROOT%\Microsoft.NET\Framework\v3.5
SET VSFOLDER=%PROGRAMFILES%\Microsoft Visual Studio 9.0\Common7\IDE
SET ZIPPATH=.\Components\7Zip

REM *****************************************************************
REM * PREPARE FOR BUILD
REM *****************************************************************

RD /S /Q "%OUTFOLDER%"

IF NOT EXIST "%KEYFILE%" GOTO MissingSNK

:Build

ECHO.
ECHO *******************************************
ECHO * BUILDING: %BUILDVER%
ECHO *******************************************
ECHO.

IF /I NOT '%RUNTESTS%'=='true' GOTO SkipTests
IF NOT EXIST "%VSFOLDER%\MSTest.exe" GOTO SkipTests

REM *** Build the unit testing staging configuration. ***
"%FXFOLDER%\MSBUILD.exe" /p:Configuration=Staging /p:DefineConstants="%BUILDVER%;CODE_ANALYSIS" /t:REBUILD ".\Source\DotRas\DotRas.sln"

IF '%ERRORLEVEL%'=='1' GOTO BuildFailed

CALL "C:\Program Files\Typemock\Isolator\5.3\mocking_on.bat"
"%VSFOLDER%\MSTest.exe" /testcontainer:".\Source\DotRas\DotRas.Tests\bin\Staging\DotRas.Tests.dll"

IF '%ERRORLEVEL%'=='1' GOTO TestsFailed

:SkipTests

REM *** Build the final version that will be published. ***
"%FXFOLDER%\MSBUILD.exe" /p:SignAssembly=true /p:AssemblyOriginatorKeyFile="%KEYFILE%" /p:Configuration=%CONFIG% /p:DefineConstants="%BUILDVER%;CODE_ANALYSIS;NO_UNIT_TESTING" /t:REBUILD ".\Source\DotRas\DotRas.sln"

IF '%ERRORLEVEL%'=='1' GOTO BuildFailed

MD "%OUTFOLDER%\Binaries\%BUILDVER%"

COPY ".\Source\DotRas\DotRas\bin\%CONFIG%\DotRas.dll" "%OUTFOLDER%\Binaries\%BUILDVER%\"
COPY ".\Source\DotRas\DotRas\bin\%CONFIG%\DotRas.xml" "%OUTFOLDER%\Binaries\%BUILDVER%\"

IF '%BUILDVER%'=='WIN2K' (SET BUILDVER=WINXP) & GOTO Build
IF '%BUILDVER%'=='WINXP' (SET BUILDVER=WINXPSP2) & GOTO Build
IF '%BUILDVER%'=='WINXPSP2' (SET BUILDVER=WIN2K8) & GOTO Build
REM IF '%BUILDVER%'=='WIN2K8' (SET BUILDVER=WIN7) & GOTO Build

GOTO MakeDocs



:MakeDocs

IF /I NOT '%MAKEDOCS%'=='true' GOTO SkipDocs
IF NOT EXIST "%SHFBPATH%"  GOTO MissingSHFB

ECHO.
ECHO *** BUILDING DOCUMENTATION ***
ECHO.

DEL /F /Q "%CHMFILE%"

"%SHFBPATH%\SandcastleBuilderConsole.exe" "%SHFBFILE%"

IF '%ERRORLEVEL%'=='1' GOTO BuildFailed

COPY "%CHMFILE%" "%OUTFOLDER%\Binaries\%BUILDVER%\"



:SkipDocs
:TransferLicense

ECHO.
ECHO *******************************************
ECHO * TRANSFERRING LICENSES
ECHO *******************************************
ECHO.

COPY "..\License.rtf" "%OUTFOLDER%\Binaries\"
COPY "..\ReadMe.txt" "%OUTFOLDER%\Binaries\"



:TransferSource

ECHO.
ECHO *******************************************
ECHO * TRANSFERRING SOURCE CODE
ECHO *******************************************
ECHO.

SET OUTSOURCE=%OUTFOLDER%\Temp\Source

ECHO *** REMOVING BIN FOLDERS ***
ECHO.

RD /S /Q ".\TestResults"
RD /S /Q ".\Source\DotRas\TestResults"
RD /S /Q ".\Source\DotRas\DotRas\bin"
RD /S /Q ".\Source\DotRas\DotRas\obj"
RD /S /Q ".\Source\DotRas\DotRas.Setup\DotRas.Setup\bin"
RD /S /Q ".\Source\DotRas\DotRas.Setup\DotRas.Setup\obj"
RD /S /Q ".\Source\DotRas\DotRas.Tests\bin"
RD /S /Q ".\Source\DotRas\DotRas.Tests\obj"

RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\CS\CreateAndDialVpnEntry\bin"
RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\CS\CreateAndDialVpnEntry\obj"
RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\VB\CreateAndDialVpnEntry\bin"
RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\VB\CreateAndDialVpnEntry\obj"
RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\CS\GetActiveConnectionIPAddress\bin"
RD /S /Q ".\Examples\Examples\CreateAndDialVpnEntry\CS\GetActiveConnectionIPAddress\obj"

ECHO.
ECHO *** COPYING SOURCE FILES ***
ECHO.

XCOPY /E ".\Source\DotRas\*.*" "%OUTSOURCE%\"
XCOPY /E ".\Examples\Examples\*.*" "%OUTFOLDER%\Examples\"

COPY ".\Source\DotRas\DotRas.sln" "%OUTSOURCE%\"
COPY ".\Source\DotRas\DotRas.DevKey.snk" "%OUTSOURCE%\"
COPY ".\Source\DotRas\DotRas.vsmdi" "%OUTSOURCE%\"
COPY ".\Source\DotRas\LocalTestRun.testrunconfig" "%OUTSOURCE%\"
COPY "..\License.rtf" "%OUTSOURCE%\"
COPY "..\ReadMe.txt" "%OUTSOURCE%\"

ATTRIB -R "%OUTSOURCE%\*.*" /S

ECHO.
ECHO *** CLEANING FOLDERS ***
ECHO.

DEL /S /Q "%OUTFOLDER%\*.suo"
DEL /S /Q "%OUTFOLDER%\*.cache"
DEL /S /Q "%OUTFOLDER%\*.*proj.user"

ECHO.
ECHO *** STRIPPING SOURCE CONTROL BINDINGS ***
ECHO.

".\Components\StripSccBindings\StripSccBindings.exe" vstfs -recursive "%OUTFOLDER%\Examples"
".\Components\StripSccBindings\StripSccBindings.exe" vstfs -recursive "%OUTSOURCE%"

ECHO.
ECHO *** COMPRESSING SOURCE CODE ***
ECHO.

"%ZIPPATH%\7za.exe" a -tzip -r "%OUTFOLDER%\Source\DotRas.zip" "%OUTSOURCE%\*" -mx9

RD /S /Q "%OUTFOLDER%\Temp"

:MakeInstaller

IF /I NOT '%MAKEINSTALLER%'=='true' GOTO ExitBatch

ECHO.
ECHO *******************************************
ECHO * BUILDING: INSTALLER
ECHO *******************************************
ECHO.

"%FXFOLDER%\MSBUILD.exe" /p:Configuration=%CONFIG% /t:REBUILD ".\Source\DotRas\DotRas.Setup\DotRas.Setup.sln"

IF '%ERRORLEVEL%'=='1' GOTO BuildFailed

COPY ".\Source\DotRas\DotRas.Setup\DotRas.Setup\bin\%CONFIG%\en-us\DotRas SDK.msi" "%OUTFOLDER%"

GOTO ExitBatch



:MissingSHFB

ECHO *******************************************
ECHO * ABORTING! MISSING SANDCASTLE HELP BUILDER
ECHO *******************************************
ECHO.
ECHO Aborting the build process. The Sandcastle Help File Builder
ECHO application is not installed on this machine. Please install
ECHO the software before continuing or set MAKEDOCS to false.
ECHO.
ECHO http://www.codeplex.com/shfb
ECHO.
ECHO Expected Path:
ECHO %SHFBPATH%
ECHO.
PAUSE
GOTO ExitBatch



:MissingSNK

ECHO *******************************************
ECHO * ABORTING! MISSING SNK FILE
ECHO *******************************************
ECHO.
ECHO Aborting the build process. The private key could not be found.
ECHO Please make sure the USB key is inserted in the machine or edit
ECHO this file and change the path to the key file.
ECHO.
ECHO KEYFILE: %KEYFILE%
ECHO.
PAUSE
GOTO ExitBatch



:BuildFailed

ECHO.
ECHO.
ECHO *******************************************
ECHO * BUILD FAILED!
ECHO *******************************************
ECHO.
ECHO The build process failed for the following build config:
ECHO Config: %CONFIG%
ECHO Build: %BUILDVER%
ECHO.
PAUSE
GOTO ExitBatch



:TestsFailed

ECHO.
ECHO.
ECHO *******************************************
ECHO * UNIT TESTING FAILED!
ECHO *******************************************
ECHO.
ECHO One or more unit tests failed the testing process. Please check the unit test 
ECHO results and fix any problem(s) before continuing...
ECHO.
ECHO Config: %CONFIG%
ECHO Build: %BUILDVER%
ECHO.
PAUSE
GOTO ExitBatch



:ExitBatch