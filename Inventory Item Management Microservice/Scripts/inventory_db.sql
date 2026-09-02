-- ============================
-- Database: inventory_db
-- ============================

IF DB_ID(N'inventory_db') IS NULL
BEGIN
    CREATE DATABASE inventory_db;
END
GO

USE inventory_db;
GO

-- ============================
-- Table: inventory_items
-- ============================

IF OBJECT_ID(N'dbo.inventory_items', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.inventory_items (
        item_id        UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_inventory_items PRIMARY KEY DEFAULT NEWID(),
        item_name      NVARCHAR(150) NOT NULL,
        category       NVARCHAR(100) NULL,
        quantity       INT NOT NULL CONSTRAINT CK_inventory_items_quantity CHECK (quantity >= 0),
        is_active      BIT NOT NULL CONSTRAINT DF_inventory_items_is_active DEFAULT 1,
        created_at     DATETIME2 NOT NULL CONSTRAINT DF_inventory_items_created_at DEFAULT GETDATE(),
        updated_at     DATETIME2 NULL
    );
END
GO

-- ============================
-- Indexes
-- ============================

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_inventory_item_name' AND object_id = OBJECT_ID(N'dbo.inventory_items'))
BEGIN
    CREATE INDEX idx_inventory_item_name ON dbo.inventory_items(item_name);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_inventory_is_active' AND object_id = OBJECT_ID(N'dbo.inventory_items'))
BEGIN
    CREATE INDEX idx_inventory_is_active ON dbo.inventory_items(is_active);
END
GO
