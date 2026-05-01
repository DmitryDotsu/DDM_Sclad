-- Индексы для внешних ключей
CREATE INDEX idx_персонал_должность ON персонал(id_должности);
CREATE INDEX idx_персонал_склад ON персонал(id_склада);

CREATE INDEX idx_приход_склад ON приход_товара(id_склада);
CREATE INDEX idx_приход_контрагент ON приход_товара(id_контрагента);
CREATE INDEX idx_приход_дата ON приход_товара(дата_прихода);

CREATE INDEX idx_расход_склад ON расход_товара(id_склада);
CREATE INDEX idx_расход_контрагент ON расход_товара(id_контрагента);
CREATE INDEX idx_расход_дата ON расход_товара(дата_расхода);

CREATE INDEX idx_тч_прихода_приход ON ТЧ_накладная_прихода(id_прихода);
CREATE INDEX idx_тч_прихода_товар ON ТЧ_накладная_прихода(id_товара);

CREATE INDEX idx_тч_расхода_расход ON ТЧ_накладная_расхода(id_расхода);
CREATE INDEX idx_тч_расхода_товар ON ТЧ_накладная_расхода(id_товара);

-- Индексы для поиска
CREATE INDEX idx_персонал_фио ON персонал(фио);
CREATE INDEX idx_контрагенты_название ON контрагенты(название);
CREATE INDEX idx_товары_название ON товары(название);
CREATE INDEX idx_товары_категория ON товары(категория);

--30.04.2026 FIX - мешает реализации
-- 1. Разрешаем NULL для телефона, адреса, email
ALTER TABLE контрагенты ALTER COLUMN телефон DROP NOT NULL;
ALTER TABLE контрагенты ALTER COLUMN адрес DROP NOT NULL;
ALTER TABLE контрагенты ALTER COLUMN email DROP NOT NULL;

-- 2. Удаляем ограничение CHECK, которое требует, чтобы была хотя бы одна галочка
ALTER TABLE контрагенты DROP CONSTRAINT IF EXISTS контрагенты_check;

--свяжем зав склада по id персонала
ALTER TABLE склады ADD COLUMN id_завсклада INTEGER REFERENCES персонал(id_сотрудника);
ALTER TABLE склады DROP COLUMN завсклад;
