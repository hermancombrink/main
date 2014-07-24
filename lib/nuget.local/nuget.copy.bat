
@echo off
set x=%2%packages
set y=%1%Packages
echo source      : %x%
echo destination : %y%
@echo off
for /R %x% %%f in (*.nupkg) do copy %%f %y% /Y
@echo on
