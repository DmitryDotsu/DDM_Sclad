-- =====================================================
-- 1. Таблица складов
-- =====================================================
CREATE TABLE склады (
    id_склада SERIAL PRIMARY KEY,
    наименование VARCHAR(100) NOT NULL,
    адрес TEXT NOT NULL,
    завсклад VARCHAR(100) NOT NULL
);

-- =====================================================
-- 2. Таблица должностей
-- =====================================================
CREATE TABLE должности (
    id_должности SERIAL PRIMARY KEY,
    название VARCHAR(50) NOT NULL UNIQUE,
    оклад DECIMAL(10,2) CHECK (оклад > 0)
);

-- =====================================================
-- 3. Таблица персонала
-- =====================================================
CREATE TABLE персонал (
    id_сотрудника SERIAL PRIMARY KEY,
    id_должности INTEGER NOT NULL,
    id_склада INTEGER NOT NULL,
    инн VARCHAR(12) NOT NULL UNIQUE CHECK (инн ~ '^\d{10,12}$'),
    фио VARCHAR(150) NOT NULL,
    телефон VARCHAR(20) CHECK (телефон ~ '^\+?[0-9]{10,15}$'),
    дата_приема DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (id_должности) REFERENCES должности(id_должности),
    FOREIGN KEY (id_склада) REFERENCES склады(id_склада)
);

-- =====================================================
-- 4. Таблица контрагентов
-- =====================================================
CREATE TABLE контрагенты (
    id_контрагента SERIAL PRIMARY KEY,
    название VARCHAR(150) NOT NULL,
    инн VARCHAR(12) UNIQUE CHECK (инн ~ '^\d{10,12}$'),
    телефон VARCHAR(20) CHECK (телефон ~ '^\+?[0-9]{10,15}$'),
    адрес TEXT,
    является_поставщиком BOOLEAN DEFAULT false,
    является_покупателем BOOLEAN DEFAULT false,
    CHECK (является_поставщиком = true OR является_покупателем = true)
);

-- =====================================================
-- 5. Таблица товаров
-- =====================================================
CREATE TABLE товары (
    id_товара SERIAL PRIMARY KEY,
    название VARCHAR(200) NOT NULL,
    категория VARCHAR(50),
    ед_измерения VARCHAR(10) NOT NULL DEFAULT 'шт'
);

-- =====================================================
-- 6. Таблица прихода товара (шапка документа)
-- =====================================================
CREATE TABLE приход_товара (
    id_прихода SERIAL PRIMARY KEY,
    id_склада INTEGER NOT NULL,
    id_контрагента INTEGER NOT NULL,
    номер_накладной VARCHAR(50) NOT NULL,
    дата_прихода DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (id_склада) REFERENCES склады(id_склада),
    FOREIGN KEY (id_контрагента) REFERENCES контрагенты(id_контрагента),
    UNIQUE (id_склада, номер_накладной)
);

-- =====================================================
-- 7. Табличная часть накладной прихода (строки)
-- =====================================================
CREATE TABLE ТЧ_накладная_прихода (
    id_записи SERIAL PRIMARY KEY,
    id_прихода INTEGER NOT NULL,
    id_товара INTEGER NOT NULL,
    цена DECIMAL(10,2) NOT NULL CHECK (цена > 0),
    количество INTEGER NOT NULL CHECK (количество > 0),
    сумма DECIMAL(12,2) GENERATED ALWAYS AS (цена * количество) STORED,
    FOREIGN KEY (id_прихода) REFERENCES приход_товара(id_прихода) ON DELETE CASCADE,
    FOREIGN KEY (id_товара) REFERENCES товары(id_товара)
);

-- =====================================================
-- 8. Таблица расхода товара (шапка документа)
-- =====================================================
CREATE TABLE расход_товара (
    id_расхода SERIAL PRIMARY KEY,
    id_склада INTEGER NOT NULL,
    id_контрагента INTEGER NOT NULL,
    номер_накладной VARCHAR(50) NOT NULL,
    дата_расхода DATE NOT NULL DEFAULT CURRENT_DATE,
    FOREIGN KEY (id_склада) REFERENCES склады(id_склада),
    FOREIGN KEY (id_контрагента) REFERENCES контрагенты(id_контрагента),
    UNIQUE (id_склада, номер_накладной)
);

-- =====================================================
-- 9. Табличная часть накладной расхода (строки)
-- =====================================================
CREATE TABLE ТЧ_накладная_расхода (
    id_записи SERIAL PRIMARY KEY,
    id_расхода INTEGER NOT NULL,
    id_товара INTEGER NOT NULL,
    цена DECIMAL(10,2) NOT NULL CHECK (цена > 0),
    количество INTEGER NOT NULL CHECK (количество > 0),
    сумма DECIMAL(12,2) GENERATED ALWAYS AS (цена * количество) STORED,
    FOREIGN KEY (id_расхода) REFERENCES расход_товара(id_расхода) ON DELETE CASCADE,
    FOREIGN KEY (id_товара) REFERENCES товары(id_товара)
);
