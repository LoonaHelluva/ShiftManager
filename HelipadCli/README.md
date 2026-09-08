# HelipadCli

Консольный клиент для ручной проверки HelipadManager по HTTP.

Сначала запустите сервер в корне проекта:

```powershell
dotnet run --launch-profile http
```

В другом терминале запустите CLI:

```powershell
dotnet run --project .\HelipadCli\HelipadCli.csproj
```

Для другого адреса сервера передайте URL первым аргументом:

```powershell
dotnet run --project .\HelipadCli\HelipadCli.csproj -- http://localhost:5241
```
