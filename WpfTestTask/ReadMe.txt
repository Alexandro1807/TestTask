Планы:
+- добавление хранилища обложек книги (не умею корректно сохранять массив байтов и считывать его)

+- добавление новых книг
	- анимация добавления книги
	- проверка полей на адекватность
	- добавление формата строки для ISBN (***-*-****-****-*)
- редактирование имеющихся книг
- удаление существующих книг


+-- поиск по полям; реализация на стороне СУБД (хранимые процедуры??? в PostgreSQL)
	(Название)
	(ФИО автора) - ввод любой части ФИО
	(Год выпуска)
	(Жанр) - выбор одного жанра из ComboBox
- запуск скрипта фильтрации через 0,5 сек после обновления любого из полей (кроме ComboBox)

- добавить всем методам ///Описание метода
- сгруппировать методы по регионам #region

-------------------------ПРОБЛЕМЫ
1) При первом вводе текста анимация проигрывается через раз
1.1) Вывести переменную _isInitialized на форму и наблюдать за ней


Описание работы:
База данных PostgreSQL свежей версии. Сервер на localhost:5432, база данных test.
Данные имеются в строке подключения в PSqlConnection.cs
Бэкап базы данных - testBackup

Хотелки по улучшению:
- Внедрение EntityFrameworkCore. Описание DbContext для всех таблиц, подключение аппарата LINQ для быстрой вставки/выборки/удаления данных.
- Размещение БД на удалённом сервере (для этого необходимо арендовать хостинг).


--Функции
DROP FUNCTION IF EXISTS BookPageFilter(integer,integer);
CREATE OR REPLACE FUNCTION BookPageFilter(bLimit int, bOffset int) RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM public."Books" b
ORDER BY b."LastModified"
LIMIT bLimit OFFSET bOffset
$$
LANGUAGE SQL;

DROP FUNCTION IF EXISTS BookNameFilter(int,int,text);
CREATE OR REPLACE FUNCTION BookNameFilter(bLimit int, bOffset int, bName text default 'underfined') RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE b."Name" = bName
$$
LANGUAGE SQL;

DROP FUNCTION IF EXISTS BookYearOfProductionFilter(int,int,int);
CREATE OR REPLACE FUNCTION BookYearOfProductionFilter(bLimit int, bOffset int, bYear int default -1) RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE b."YearOfProduction" = bYear
$$
LANGUAGE SQL;

//Функции для поиска автора (фамилия || имя || отчество)
//Функция для поиска по жанру


--Добавление перегрузок одной функции под разные фильтры

--Проверка функции
SELECT * FROM BookPageFilter(3, 3);