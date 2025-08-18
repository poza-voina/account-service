# account-service
Сервис для работы с банковскими счетами.

## Возможности сервиса
- Создавать, удалять, изменять банковские счета
- Выдывать выписку по счету
- Зарегистрировать транзакцию
- Регистрировать и выполнять транзакцию
- Переносить деньги между счетами (т.е создать две транзакции. Одна транзакция на списание, другая на пополнение. После их создания они применяются.)

## Где, что запускается

### API

`http://localhost/swagger/index.html` 

### RabbitMq Management
`http://localhost:15672`

Username = testuser

Password = testpassword

### Hangfire

`http://localhost/hangfire`

### Keycloak

`http://localhost:8080`

**Админ кейклока**

Username = admin

Password = admin

**Клиент приложения**

Username = testuser

Password = testpassword

## Примеры эвентов
Нужно указывать message_id (уникальное)
### Пример эвента для блокировки аккаунта
```
{
  "eventId": "10000000-0000-0000-0000-000000000000",
  "occurredAt": "2025-08-18T12:34:56.789Z",
  "meta": {
    "source": "system",
    "version": "v1",
    "correlationId": "20000000-0000-0000-0000-000000000000",
    "causationId": "20000000-0000-0000-0000-000000000000"
  },
  "payload": {
    "eventId": "10000000-0000-0000-0000-000000000000",
    "occurredAt": "2025-08-18T12:34:56.789Z",
    "clientId": "7c9fcf13-6df1-4eb1-9404-0e4380e2bba5"
  },
  "eventType": "ClientBlocked"
}
```
### Пример эвента для разблокировки счета
```
{
  "eventId": "10000000-0000-0000-0000-000000000000",
  "occurredAt": "2025-08-18T12:34:56.789Z",
  "meta": {
    "source": "system",
    "version": "v1",
    "correlationId": "20000000-0000-0000-0000-000000000000",
    "causationId": "20000000-0000-0000-0000-000000000000"
  },
  "payload": {
    "eventId": "10000000-0000-0000-0000-000000000000",
    "occurredAt": "2025-08-18T12:34:56.789Z",
    "clientId": "7c9fcf13-6df1-4eb1-9404-0e4380e2bba5"
  },
  "eventType": "ClientUnblocked"
}
```

## [Документация API](./docs/api.md)
## [Задание 1](./docs/task1.md)
## [Задание 2](./docs/task2.md)
## [Задание 3](./docs/task3.md)
## [Задание 4](./docs/task4.md)

## Запуск в Docker
```docker compose up -d```
порядок запуска сервисов
keycloak -> keycloak-init -> postgres -> migrationRunner -> accountservice.api

## Запуск в VS + Docker
<img width="327" height="86" alt="image" src="https://github.com/user-attachments/assets/6d84f909-3e56-4816-926f-d51adf63f64c" />

Для того чтобы контейнеры удалились из `Docker` нужно сделать очистку решения

## Запуск в VS
Требуется чтобы работал keycloak

<img width="385" height="53" alt="image" src="https://github.com/user-attachments/assets/ebfed2d3-0466-4dc2-87fd-a14608dd2480" />

## Тонкости проекта:
- БД не было использовано по условиям задания, вместо этого зависимости с данными внедряются через DI
- Была проблема: при запуке через проект `docker-compose.dcproj` профиль `DockerCompose` сервис `api` считал, что keycloak был в статусе unhealhy (проблема вроде бы ушла)
- `Realm`, `client`, `user` при использование профиля `visual studio` или `docker-compose` создаются автоматически. Пароли находятся в [файле](./.env).

### Тонкости работы транзакций
Работа транзакции состоит из двух этапов:
- Регистрация транзакции (Нету валидации на количество денег)
- Применение транзакции (Валидация на количество денег)
Транзакция, которую невозможно применить остается сохраненной. Флаг, показывающий применение транзакции `Transaction.IsApply`

### Технологии которые были использованы при разработке:
- Docker
- Keycloak
- Mediator
- FluentValidation
- python (был написан скрипт для генерации шаблона CQRS на основе команды и query)
