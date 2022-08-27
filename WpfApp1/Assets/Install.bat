pushd "%~dp0"
VC_redist.x86.exe /install /quiet /norestart
VC_redist.x64.exe /install /quiet /norestart
EdgeSetup.exe /silent /install
echo "installed" > installed.txt