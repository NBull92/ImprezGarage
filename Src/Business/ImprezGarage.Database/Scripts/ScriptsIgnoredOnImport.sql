
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/25/2018 21:00:07
-- Generated from EDMX file: D:\Documents\Nick\GitHub\ImprezGarage\Src\Business\ImprezGarage.Infrastructure\Model\ImprezGarageDatabase.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO

USE [ImprezGarage];
GO

IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Vehicles_VehicleType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_Vehicles_VehicleType];
GO

IF OBJECT_ID(N'[dbo].[FK_MaintenanceChecks_MaintenanceCheckType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceChecks] DROP CONSTRAINT [FK_MaintenanceChecks_MaintenanceCheckType];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[VehicleTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleTypes];
GO

IF OBJECT_ID(N'[dbo].[Vehicles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehicles];
GO

IF OBJECT_ID(N'[dbo].[MaintenanceChecks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceChecks];
GO

IF OBJECT_ID(N'[dbo].[MaintenanceCheckTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceCheckTypes];
GO

IF OBJECT_ID(N'[dbo].[PartsReplacementRecords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PartsReplacementRecords];
GO

IF OBJECT_ID(N'[dbo].[PetrolExpenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PetrolExpenses];
GO


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

GO
