CREATE DATABASE [DevLearningCategoryAPI]
GO

USE [DevLearningCategoryAPI]
GO

CREATE TABLE [Category]
(
    [Id] uniqueidentifier NOT NULL,
    [Title] NVARCHAR(160) NOT NULL,
    [Url] NVARCHAR(1024) NOT NULL,
    [Summary] NVARCHAR(2000) NOT NULL,
    [Order] int NOT NULL,
    [Description] TEXT NOT NULL,
    [Featured] BIT NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO