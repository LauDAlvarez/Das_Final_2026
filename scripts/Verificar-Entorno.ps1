$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$project = Join-Path $root "TechStore.App\TechStore.App.csproj"
$solution = Join-Path $root "TechStore.sln"

if (-not (Test-Path $solution) -or -not (Test-Path $project)) {
    throw "Carpeta incorrecta. Ejecute este script desde el repositorio que contiene TechStore.sln y TechStore.App."
}

[xml]$projectXml = Get-Content $project
$target = $projectXml.Project.PropertyGroup.TargetFramework
$packages = @($projectXml.Project.ItemGroup.PackageReference.Include)

if ($target -ne "net8.0-windows") {
    throw "Proyecto incorrecto: se esperaba net8.0-windows y se encontró '$target'."
}

if ($packages -notcontains "Microsoft.EntityFrameworkCore.Sqlite") {
    throw "Proyecto incorrecto: no utiliza Microsoft.EntityFrameworkCore.Sqlite."
}

$forbidden = Get-ChildItem $root -Recurse -File -Include *.csproj,packages.config |
    Select-String -Pattern "net472|EntityFramework\.SqlServer"
if ($forbidden) {
    throw "Se detectó un proyecto antiguo de .NET Framework/SQL Server dentro del árbol. No lo use como proyecto de inicio."
}

Write-Host "Entorno correcto: TechStore.App / $target / EF Core SQLite." -ForegroundColor Green
Write-Host "Solución: $solution"
Write-Host "Proyecto: $project"
