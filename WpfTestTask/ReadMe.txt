﻿Описание работы:
База данных PostgreSQL, СУБД PgAdmin 4 версии 8.7. Сервер на localhost:5432, база данных "test".
Строка подключения находится в файле в PSqlConnection.cs
Бэкап базы данных - файл testBackup
Для корректной работы необходимо:
1) создать базу данных test;
2) выполнить restore (файл testBackup).
Файл релиза: ~\TestTask\WpfTestTask\bin\Release\net8.0-windows\WpfTestTask.exe

Долгосрочные планы по улучшению проекта:
- Внедрение EntityFrameworkCore. Описание DbContext для всех таблиц, подключение аппарата LINQ для быстрой вставки/выборки/удаления данных.
- Размещение БД на удалённом сервере (для этого необходимо арендовать хостинг).
- Установка всех стандартных значений программы в отдельной таблице; выполнение запроса выборки стандартного значения для каждого соответствующего места программы.

Краткосрочные планы по улучшению проекта:
+- добавление хранилища обложек книги (не умею корректно сохранять массив байтов и считывать его)
- добавление открытия изображения в большом размере при нажатии по нему
+ окно просмотра книг
	- всплывающая контекстная подсказка с полным содержимым ячейки спустя 0.5 сек после наведения мыши на ячейку
+ окно добавления книги
	- добавление миниатюры обложки
+ окно редактирования книги
	- добавление миниатюры обложки
	- добавление красной звёздочки у названия поля, содержимое которого отличается от сохранённого значения
- добавление всем методам ///Описание метода



-------------------------ПРОБЛЕМЫ-------------------------
1) При первом вводе текста в поля фильтрации окна "Книги" анимация проигрывается через раз
1.1) Вывести переменную _isInitialized на форму и наблюдать за ней