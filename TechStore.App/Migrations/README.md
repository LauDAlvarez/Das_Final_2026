# Migración inicial

La creación inicial idempotente del esquema SQLite se ejecuta con `Database.EnsureCreatedAsync` al iniciar. Para despliegues posteriores se utiliza `dotnet ef migrations add <Nombre>` y `dotnet ef database update`.
