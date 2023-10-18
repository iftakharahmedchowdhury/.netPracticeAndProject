
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/16/2023 07:52:14
-- Generated from EDMX file: E:\AIUB STUDY\11th semeser\adv. dot net\mid\class task\New folder\RegistrationModule\RegistrationModule\EF\UMS.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DemoF23_C];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Courses_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Courses] DROP CONSTRAINT [FK_Courses_Departments];
GO
IF OBJECT_ID(N'[dbo].[FK_CourseStudents_Courses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CourseStudents] DROP CONSTRAINT [FK_CourseStudents_Courses];
GO
IF OBJECT_ID(N'[dbo].[FK_CourseStudents_Students]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CourseStudents] DROP CONSTRAINT [FK_CourseStudents_Students];
GO
IF OBJECT_ID(N'[dbo].[FK_Students_Departments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Students] DROP CONSTRAINT [FK_Students_Departments];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Courses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Courses];
GO
IF OBJECT_ID(N'[dbo].[CourseStudents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CourseStudents];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Students]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Students];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Courses'
CREATE TABLE [dbo].[Courses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [DeptId] int  NOT NULL
);
GO

-- Creating table 'CourseStudents'
CREATE TABLE [dbo].[CourseStudents] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CId] int  NOT NULL,
    [StId] int  NOT NULL,
    [EnrollTime] datetime  NOT NULL
);
GO

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Students'
CREATE TABLE [dbo].[Students] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Cgpa] float  NULL,
    [DeptId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [PK_Courses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CourseStudents'
ALTER TABLE [dbo].[CourseStudents]
ADD CONSTRAINT [PK_CourseStudents]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [PK_Students]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DeptId] in table 'Courses'
ALTER TABLE [dbo].[Courses]
ADD CONSTRAINT [FK_Courses_Departments]
    FOREIGN KEY ([DeptId])
    REFERENCES [dbo].[Departments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Courses_Departments'
CREATE INDEX [IX_FK_Courses_Departments]
ON [dbo].[Courses]
    ([DeptId]);
GO

-- Creating foreign key on [CId] in table 'CourseStudents'
ALTER TABLE [dbo].[CourseStudents]
ADD CONSTRAINT [FK_CourseStudents_Courses]
    FOREIGN KEY ([CId])
    REFERENCES [dbo].[Courses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseStudents_Courses'
CREATE INDEX [IX_FK_CourseStudents_Courses]
ON [dbo].[CourseStudents]
    ([CId]);
GO

-- Creating foreign key on [StId] in table 'CourseStudents'
ALTER TABLE [dbo].[CourseStudents]
ADD CONSTRAINT [FK_CourseStudents_Students]
    FOREIGN KEY ([StId])
    REFERENCES [dbo].[Students]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CourseStudents_Students'
CREATE INDEX [IX_FK_CourseStudents_Students]
ON [dbo].[CourseStudents]
    ([StId]);
GO

-- Creating foreign key on [DeptId] in table 'Students'
ALTER TABLE [dbo].[Students]
ADD CONSTRAINT [FK_Students_Departments]
    FOREIGN KEY ([DeptId])
    REFERENCES [dbo].[Departments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Students_Departments'
CREATE INDEX [IX_FK_Students_Departments]
ON [dbo].[Students]
    ([DeptId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------