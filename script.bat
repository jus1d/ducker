@echo off
for /l %%i in (1,1,8) do (
    echo " " >> README.md
    git add .
    git commit -m "some commit" --date="2021-05-13T00:00:00+0300"
    git push
)