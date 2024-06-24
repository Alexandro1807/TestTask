Планы:
+- добавление хранилища обложек книги (не умею корректно сохранять массив байтов и считывать его)

+- добавление новых книг
	- проверка полей на адекватность
	- добавление формата строки для ISBN (***-*-****-****-*)
- редактирование имеющихся книг
- удаление существующих книг


- поиск по полям; реализация на стороне СУБД (хранимые процедуры??? в PostgreSQL)
	(Название)
	(ФИО автора)
	(Год выпуска)
	(Жанр)
+- постраничный вывод результатов; реализация на стороне СУБД (хранимые процедуры??? в PostgreSQL) (SELECT TOP(10)) ??? (SELECT RANGE(10, 20))


- добавить всем методам ///Описание метода
- сгруппировать методы по регионам #region


Описание работы:
База данных PostgreSQL свежей версии. Сервер на localhost:5432, база данных test.
Данные имеются в строке подключения в PSqlConnection.cs
Бэкап базы данных - testBackup

Хотелки по улучшению:
- Внедрение EntityFrameworkCore. Описание DbContext для всех таблиц, подключение аппарата LINQ для быстрой вставки/выборки/удаления данных.
- Размещение БД на удалённом сервере (для этого необходимо арендовать хостинг).


--Функция 1

CREATE FUNCTION BookPageFilter(in intLimit int, in intOffset int) RETURNS public."Books"
    AS $$
	SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
	FROM public."Books" b
	LIMIT intLimit OFFSET intOffset
	$$
    LANGUAGE SQL;

SELECT * FROM BookPageFilter(3, 3);