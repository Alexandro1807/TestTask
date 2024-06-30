Планы:
+- добавление хранилища обложек книги (не умею корректно сохранять массив байтов и считывать его)
- добавление открытия изображения в большом размере при нажатии по нему

++- добавление новых книг
	- анимация добавления книги
	- вопрос о возврате к предыдущему окну
	- проверка полей на адекватность
	- добавление формата строки для ISBN (***-*-****-****-*)
	- добавление миниатюры обложки
+-- редактирование имеющихся книг
	- анимация изменения книги
	- вопрос о возврате к предыдущему окну (сразу возвращать???)
	- проверка полей на адекватность
	- добавление формата строки для ISBN (***-*-****-****-*)
	- добавление миниатюры обложки
	- добавление красной звёздочки у названия поля, содержимое которого отличается от сохранённого значения
+- удаление существующих книг
	- анимация удаления книги
	+- возврат на предыдущее окно


- добавить всем методам ///Описание метода
- сгруппировать методы по регионам #region

-------------------------ПРОБЛЕМЫ-------------------------
1) При первом вводе текста анимация проигрывается через раз
1.1) Вывести переменную _isInitialized на форму и наблюдать за нейд
2) Подравнять столбцы в DataGridBooks
2.1) Добавить всплывающую контекстную подсказку с полным содержимым ячейки спустя 0.5 сек после наведения мыши на ячейку

Описание работы:
База данных PostgreSQL свежей версии. Сервер на localhost:5432, база данных test.
Данные имеются в строке подключения в PSqlConnection.cs
Бэкап базы данных - testBackup

Хотелки по улучшению:
- Внедрение EntityFrameworkCore. Описание DbContext для всех таблиц, подключение аппарата LINQ для быстрой вставки/выборки/удаления данных.
- Размещение БД на удалённом сервере (для этого необходимо арендовать хостинг).
- Установка всех стандартных значений программы в отдельной таблице; выполнение запроса выборки стандартного значения для каждого соответствующего места программы.



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
WHERE b."Name" LIKE CONCAT('%', bName, '%')
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

DROP FUNCTION IF EXISTS BookAuthorOneWordFilter(int,int,text);
CREATE OR REPLACE FUNCTION BookAuthorOneWordFilter(bLimit int, bOffset int, bWord1 text default 'underfined') RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE b."FirstName" LIKE CONCAT('%', bWord1, '%') OR b."LastName" LIKE CONCAT('%', bWord1, '%') OR b."MiddleName" LIKE CONCAT('%', bWord1, '%')
$$
LANGUAGE SQL;

DROP FUNCTION IF EXISTS BookAuthorTwoWordsFilter(int,int,text,text);
CREATE OR REPLACE FUNCTION BookAuthorTwoWordsFilter(bLimit int, bOffset int, bWord1 text default 'underfined', bWord2 text default 'underfined') RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE (b."FirstName" LIKE CONCAT('%', bWord1, '%') AND b."LastName" LIKE CONCAT('%', bWord2, '%'))
	OR (b."LastName" LIKE CONCAT('%', bWord1, '%') AND b."MiddleName" LIKE CONCAT('%', bWord2, '%'))
	OR (b."MiddleName" LIKE CONCAT('%', bWord1, '%') AND b."FirstName" LIKE CONCAT('%', bWord2, '%'))
$$
LANGUAGE SQL;

DROP FUNCTION IF EXISTS BookAuthorThreeWordsFilter(int,int,text,text,text);
CREATE OR REPLACE FUNCTION BookAuthorThreeWordsFilter(bLimit int, bOffset int, bWord1 text default 'underfined', bWord2 text default 'underfined', bWord3 text default 'underfined') RETURNS SETOF public."Books"
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE (b."FirstName" LIKE CONCAT('%', bWord1, '%') AND b."LastName" LIKE CONCAT('%', bWord2, '%') AND b."MiddleName" LIKE CONCAT('%', bWord3, '%'))
	OR (b."FirstName" LIKE CONCAT('%', bWord1, '%') AND b."LastName" LIKE CONCAT('%', bWord3, '%') AND b."MiddleName" LIKE CONCAT('%', bWord2, '%'))
	OR (b."FirstName" LIKE CONCAT('%', bWord2, '%') AND b."LastName" LIKE CONCAT('%', bWord1, '%') AND b."MiddleName" LIKE CONCAT('%', bWord3, '%'))
	OR (b."FirstName" LIKE CONCAT('%', bWord2, '%') AND b."LastName" LIKE CONCAT('%', bWord3, '%') AND b."MiddleName" LIKE CONCAT('%', bWord1, '%'))
	OR (b."FirstName" LIKE CONCAT('%', bWord3, '%') AND b."LastName" LIKE CONCAT('%', bWord1, '%') AND b."MiddleName" LIKE CONCAT('%', bWord2, '%'))
	OR (b."FirstName" LIKE CONCAT('%', bWord3, '%') AND b."LastName" LIKE CONCAT('%', bWord2, '%') AND b."MiddleName" LIKE CONCAT('%', bWord1, '%'))
	$$
LANGUAGE SQL;

DROP FUNCTION IF EXISTS BookGenreFilter(int,int,text);
CREATE OR REPLACE FUNCTION BookGenreFilter(blimit integer, boffset integer, bGenre text default 'underfined') RETURNS SETOF "Books" 
AS $$
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
JOIN public."GenresOfBook" genre ON b."Id" = genre."BookId"
JOIN public."Genres" genres ON genre."GenreId" = genres."Id"
WHERE genres."Genre" = bGenre
$$
LANGUAGE SQL;

//Функции для поиска автора (фамилия || имя || отчество)
//Функция для поиска по жанру


--Добавление перегрузок одной функции под разные фильтры

--Проверка функции
SELECT * FROM BookPageFilter(3, 3);



















--Одна большая функция на всё (заменить внутренности вызовами более мелких функций)
DROP FUNCTION IF EXISTS BookFilter(int,int,text,text,text,int);
CREATE OR REPLACE FUNCTION BookFilter(bLimit int, bOffset int, bName text default 'underfined', bAuthor text default 'underfined', bGenre text default 'underfined', bYear int default -1) RETURNS SETOF "Books" 
AS
$$
begin
	IF (bName != 'undefined' AND bAuthor = 'undefined' AND bGenre = 'undefined' AND bYear = -1)
	THEN SELECT * FROM BookNameFilter(bLimit, bOffset, bName);
	ELSIF (bName = 'undefined' AND bAuthor != 'undefined' AND bGenre = 'undefined' AND bYear = -1)
	THEN
		IF count(string_to_array(bAuthor, ' ')) = 1
		THEN SELECT * FROM BookAuthorOneWordFilter(bLimit, bOffset, split_part(bAuthor, ' ', 1));
		ELSIF count(string_to_array(bAuthor, ' ')) = 2
		THEN SELECT * FROM BookAuthorTwoWordsFilter(bLimit, bOffset, split_part(bAuthor, ' ', 1), split_part(bAuthor, ' ', 2));
		ELSIF count(string_to_array(bAuthor, ' ')) = 3
		THEN SELECT * FROM BookAuthorThreeWordsFilter(bLimit, bOffset, split_part(bAuthor, ' ', 1), split_part(bAuthor, ' ', 2), split_part(bAuthor, ' ', 3));
		END IF;
	ELSIF (bName = 'undefined' AND bAuthor = 'undefined' AND bGenre != 'undefined' AND bYear = -1)
	THEN SELECT * FROM BookGenreFilter(bLimit, bOffset, bGenre);
	ELSIF (bName = 'undefined' AND bAuthor = 'undefined' AND bGenre = 'undefined' AND bYear != -1)
	THEN SELECT * FROM BookYearOfProductionFilter(bLimit, bOffset, bYear);
	--Выборка по двум полям, по трём полям, по всем полям
	END IF;
end;

$$ LANGUAGE plpgsql;

--Функции, переделанные под plpgsql
DROP FUNCTION IF EXISTS BookPageFilter(integer,integer);
CREATE OR REPLACE FUNCTION BookPageFilter(bLimit int, bOffset int) RETURNS SETOF public."Books"
AS $$
BEGIN
RETURN QUERY
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut" FROM public."Books" b ORDER BY b."LastModified"
LIMIT bLimit OFFSET bOffset;
END;
$$
LANGUAGE plpgsql;

DROP FUNCTION IF EXISTS BookNameFilter(int,int,text);
CREATE OR REPLACE FUNCTION BookNameFilter(bLimit int, bOffset int, bName text default 'underfined') RETURNS SETOF public."Books"
AS $$
BEGIN
RETURN QUERY
SELECT b."Id", b."LastModified", b."Name", b."FirstName", b."LastName", b."MiddleName", b."YearOfProduction", b."ISBN", b."Shortcut"
FROM BookPageFilter(bLimit, bOffset) b
WHERE b."Name" LIKE CONCAT('%', bName, '%');
END;
$$
LANGUAGE plpgsql;