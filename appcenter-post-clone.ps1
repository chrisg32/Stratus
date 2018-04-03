$configFiles = Get-ChildItem . App.xaml.cs -rec
foreach ($file in $configFiles)
{
    (Get-Content $file.PSPath) |
    Foreach-Object { $_ -replace "{{APP_CENTER_ID}}", "testy" } |
    Set-Content $file.PSPath
}