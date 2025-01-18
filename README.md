# Hdr.4kfilms

## Описание
HDR.4KFilm — это платформа, предназначенная для любителей кино и видео высокого разрешения. Мы предоставляем доступ к широкому ассортименту фильмов, сериалов и документальных фильмов в формате 4K с поддержкой HDR (High Dynamic Range), что обеспечивает непревзойденное качество изображения и звука. Наш сайт предлагает пользователям удобный интерфейс для поиска и просмотра контента

## Содержание
- [Установка](#установка)
- [Использование](#использование)
- [Вклад](#вклад)
- [Контакты](#контакты)

## Установка
1. Клонируйте репозиторий:

   git clone https://github.com/NikitinRadmir/HDR.4KFILM.git

   Для получения больше проработанного проекта - перемещаться по более поздним созданным веткам.

2. Установите Docker и добавьте SQL Server в вашу среду разработки.

3. В MyHttpServer/config.json и в HttpServerLibrary/Core/Configurations/AppConfig.cs укажите свой ConnectionString.

4. Для своей базы данных создайте SQL-запросы на создание таблиц.

   CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR (255) NOT NULL,
    [Password] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
   );

   CREATE TABLE [dbo].[Movies] (
    [MovieId]  INT            IDENTITY (1, 1) NOT NULL,
    [Title]    NVARCHAR (255) NULL,
    [ImageUrl] NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC)
   );

   CREATE TABLE [dbo].[MovieDatas] (
    [MovieId]       INT            NOT NULL,
    [Title]         NVARCHAR (MAX) NULL,
    [CoverImageUrl] NVARCHAR (MAX) NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [OriginalTitle] NVARCHAR (MAX) NULL,
    [Year]          INT            NULL,
    [Country]       NVARCHAR (MAX) NULL,
    [Genre]         NVARCHAR (MAX) NULL,
    [Quality]       NVARCHAR (MAX) NULL,
    [Sound]         NVARCHAR (MAX) NULL,
    [Director]      NVARCHAR (MAX) NULL,
    [Cast]          NVARCHAR (MAX) NULL,
    [MoviePlayer]   NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([MovieId] ASC)
   );

   CREATE TABLE [dbo].[Genres] (
       [Id]        INT            IDENTITY (1, 1) NOT NULL,
       [GenreName] NVARCHAR (255) NOT NULL,
       PRIMARY KEY CLUSTERED ([Id] ASC)
   );

   CREATE TABLE [dbo].[Countrys] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CountryName] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
   );

  CREATE TABLE [dbo].[Admins] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Login]    NVARCHAR (50) NULL,
    [Password] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
   );

5. В MyHttpServer/SQL Tables Вы найдете SQL-запросы заполнения данных для каждой таблице.

6.ПРОСЬБА!!! не удалять уже готовые данные таблиц. Если вы хотите попробовать Docker Compose откройте ветку backlogDocker, а если хотите без то используйте ветку backlog

## Использование

1. Перед запуском решения убедитесь, что Вы сделали все пункты из "Установка".

2. После запуска решения в консоли будет выводиться URL сайта. Кликнете по адресу в консоли или откройте браузер и введите адрес в поиск вручную, чтобы перейти на сайт.

3. Вас встретил главная страница, где представлены фильмы, которые будут подгружаться из вашей таблицы Movies.

4. Кликнув по фильму "Дорогой Санта (2024)", Вы попадаете на страницу с фильмом "Дорогой Санта (2024)". Все данные, касаемые этого фильма, подгружаются из таблицы MovieData.

5. На каждой странице присутствует кнопка "Войти", нажав на которую Вы попадаете на страницу авторизации. После ввода корректных данных пользователя из таблицы Users, Вы попадете на главную страницу и увидите Ваш логин на месте кнопки "Войти". Если данные пользователя были некорректны, Вы увидите сообщение о том, что такого пользователя не существует и Вас попросят ввести данные снова.

6. После авторизации на странице с фильмом Вам будет доступна функция "Добавить в закладки".

7. После авторизации при клике на кнопку с Вашим логином Вам откроется всплывающее окно с 3-мя кнопками: Админ-панель, Войти в другой аккаут и Остаться на сайте. Кнопка "Войти в другой аккаунт" перебросит Вас на страницу с авторизацией. Кнопка "Остаться на сайте" закроет всплывающее окно. Кнопка "Админ-панель" перебросит вас на страницу авторизации админ-пользователя.

8. После авторизации админ-пользователя Вы попадаете на страницу Админ-панели, где вам будут представлены Ваши таблицы, в которых мы можете добавлять или удалять данные.

## Вклад

1. В случае нахождения багов и уязвимостей просьба написать на почту, указанной в "Контакты"

## Контакты

1. Никитин Радмир 11-308 email: RANikitin@stud.kpfu.ru
