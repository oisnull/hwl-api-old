
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/13/2018 11:04:09
-- Generated from EDMX file: F:\haowuliao2\hwl-api-v1.0.0\HWL\HWL.Entity\HWLModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [hwl];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[t_admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_admin];
GO
IF OBJECT_ID(N'[dbo].[t_user]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_user];
GO
IF OBJECT_ID(N'[dbo].[t_user_code]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_user_code];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 't_admin'
CREATE TABLE [dbo].[t_admin] (
    [id] int  NOT NULL,
    [login_name] nvarchar(50)  NOT NULL,
    [login_pwd] nvarchar(50)  NOT NULL,
    [real_name] nvarchar(30)  NOT NULL,
    [create_date] datetime  NOT NULL
);
GO

-- Creating table 't_user'
CREATE TABLE [dbo].[t_user] (
    [id] int  NOT NULL,
    [symbol] nvarchar(20)  NULL,
    [name] nvarchar(30)  NULL,
    [mobile] nvarchar(11)  NOT NULL,
    [email] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [head_image] nvarchar(100)  NULL,
    [life_notes] nvarchar(200)  NULL,
    [sex] int  NOT NULL,
    [status] int  NOT NULL,
    [circle_back_image] nvarchar(100)  NOT NULL,
    [register_country] int  NOT NULL,
    [register_province] int  NOT NULL,
    [register_city] int  NOT NULL,
    [register_district] int  NOT NULL,
    [register_date] datetime  NOT NULL,
    [update_date] datetime  NULL
);
GO

-- Creating table 't_user_code'
CREATE TABLE [dbo].[t_user_code] (
    [id] int  NOT NULL,
    [user_id] int  NOT NULL,
    [user_account] nvarchar(50)  NOT NULL,
    [code_type] int  NOT NULL,
    [code] nvarchar(6)  NOT NULL,
    [create_date] datetime  NOT NULL,
    [expire_time] datetime  NOT NULL,
    [remark] nvarchar(100)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 't_admin'
ALTER TABLE [dbo].[t_admin]
ADD CONSTRAINT [PK_t_admin]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 't_user'
ALTER TABLE [dbo].[t_user]
ADD CONSTRAINT [PK_t_user]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 't_user_code'
ALTER TABLE [dbo].[t_user_code]
ADD CONSTRAINT [PK_t_user_code]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------