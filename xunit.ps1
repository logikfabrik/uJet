& "C:\Tools\xUnit20\xunit.console.x86.exe" $(Get-ChildItem $PSScriptRoot -r -i *.Test.dll | Where { $_.FullName -notlike "*\obj\*" } | % { $($_.FullName) }) -noshadow -appveyor
