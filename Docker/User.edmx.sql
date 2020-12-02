
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/02/2018 13:40:41
-- Generated from EDMX file: C:\Users\root\OneDrive\c#\Docker\Docker\Docker\User.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DockerUserController];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserCloudEdit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CloudEditSet] DROP CONSTRAINT [FK_UserCloudEdit];
GO
IF OBJECT_ID(N'[dbo].[FK_UserEdit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EditSet] DROP CONSTRAINT [FK_UserEdit];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[CloudEditSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CloudEditSet];
GO
IF OBJECT_ID(N'[dbo].[EditSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EditSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [EncodeKey] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CloudEditSet'
CREATE TABLE [dbo].[CloudEditSet] (
    [Username] nvarchar(max)  NULL,
    [OpenStack] nvarchar(max)  NULL,
    [Docker] nvarchar(max)  NULL,
    [TimeOS] nvarchar(max)  NULL,
    [TimeD] nvarchar(max)  NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'EditSet'
CREATE TABLE [dbo].[EditSet] (
    [Username] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Money] real  NOT NULL,
    [CanUseNum] int  NOT NULL,
    [Identity] int  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [User_Id] in table 'CloudEditSet'
ALTER TABLE [dbo].[CloudEditSet]
ADD CONSTRAINT [PK_CloudEditSet]
    PRIMARY KEY CLUSTERED ([User_Id] ASC);
GO

-- Creating primary key on [User_Id] in table 'EditSet'
ALTER TABLE [dbo].[EditSet]
ADD CONSTRAINT [PK_EditSet]
    PRIMARY KEY CLUSTERED ([User_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------