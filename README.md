# CompanyManagement test app (тестовое задание)

## Для запуска нужно, чтобы было установлены `ef tools`.

1. В файле `appsettings.json` настроить строку соединения с БД (`PostgreSQL`).
2. из проекта `WebApi` в командной консоли набрать `dotnet ef database update -c DataContext --project ..\CompanyManagement.Migrations\`
3. Запустить проект `WebApi` и перейти на url `swagger`
