
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
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'VehicleTypes'
CREATE TABLE [dbo].[VehicleTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Vehicles'
CREATE TABLE [dbo].[Vehicles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DateCreated] datetime  NOT NULL,
    [DateModified] datetime  NOT NULL,
    [VehicleType] int  NOT NULL,
    [Registration] nvarchar(50)  NULL,
    [TaxExpiryDate] datetime  NULL,
    [InsuranceRenewalDate] datetime  NULL,
    [FriendlyName] nvarchar(50)  NULL,
    [Model] nvarchar(50)  NULL,
    [Make] nvarchar(50)  NULL,
    [HasInsurance] bit  NULL,
    [HasValidTax] bit  NULL,
    [IsManual] bit  NULL,
    [CurrentMilage] int  NULL,
    [MilageOnPurchase] int  NULL
);
GO

-- Creating table 'MaintenanceChecks'
CREATE TABLE [dbo].[MaintenanceChecks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DatePerformed] datetime  NULL,
    [MaintenanceCheckType] int  NULL,
    [PerformedBy] nvarchar(50)  NULL,
    [CheckedAirFilter] bit  NULL,
    [ReplacedAirFilter] bit  NULL,
    [CheckCoolantLevels] bit  NULL,
    [FlushedSystemAndChangeCoolant] bit  NULL,
    [ChangeFanBelt] bit  NULL,
    [CheckedBattery] bit  NULL,
    [CheckedOilLevels] bit  NULL,
    [ReplacedOilFilter] bit  NULL,
    [CheckAutoTransmissionFluid] bit  NULL,
    [AddedAutoTransmissionFluid] bit  NULL,
    [CheckPowerSteeringFluidLevels] bit  NULL,
    [AirFilterNotes] nvarchar(120)  NULL,
    [CoolantNotes] nvarchar(120)  NULL,
    [BatteryNotes] nvarchar(120)  NULL,
    [OilLevelNotes] nvarchar(120)  NULL,
    [AutoTransmissionFluidNotes] nvarchar(120)  NULL,
    [PowerSteeringNotes] nvarchar(120)  NULL,
    [VehicleId] int  NULL
);
GO

-- Creating table 'MaintenanceCheckTypes'
CREATE TABLE [dbo].[MaintenanceCheckTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(50)  NULL,
    [OccurenceMonthly] int  NULL,
    [OccurenceMiles] int  NULL
);
GO

-- Creating table 'PartsReplacementRecords'
CREATE TABLE [dbo].[PartsReplacementRecords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsWiring] bit  NULL,
    [WiringDate] datetime  NULL,
    [WiringDetails] nvarchar(50)  NULL,
    [IsHoses] bit  NULL,
    [HosesDate] datetime  NULL,
    [HosesDetails] nvarchar(50)  NULL,
    [IsTires] bit  NULL,
    [TiresDate] datetime  NULL,
    [TiresDetails] nvarchar(50)  NULL,
    [Other] bit  NULL,
    [OtherDate] datetime  NULL,
    [OtherDetails] nvarchar(50)  NULL
);
GO

-- Creating table 'PetrolExpenses'
CREATE TABLE [dbo].[PetrolExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] float  NULL,
    [DateEntered] datetime  NULL,
    [VehicleId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'VehicleTypes'
ALTER TABLE [dbo].[VehicleTypes]
ADD CONSTRAINT [PK_VehicleTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [PK_Vehicles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceChecks'
ALTER TABLE [dbo].[MaintenanceChecks]
ADD CONSTRAINT [PK_MaintenanceChecks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceCheckTypes'
ALTER TABLE [dbo].[MaintenanceCheckTypes]
ADD CONSTRAINT [PK_MaintenanceCheckTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PartsReplacementRecords'
ALTER TABLE [dbo].[PartsReplacementRecords]
ADD CONSTRAINT [PK_PartsReplacementRecords]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PetrolExpenses'
ALTER TABLE [dbo].[PetrolExpenses]
ADD CONSTRAINT [PK_PetrolExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [VehicleType] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_Vehicles_VehicleType]
    FOREIGN KEY ([VehicleType])
    REFERENCES [dbo].[VehicleTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Vehicles_VehicleType'
CREATE INDEX [IX_FK_Vehicles_VehicleType]
ON [dbo].[Vehicles]
    ([VehicleType]);
GO

-- Creating foreign key on [MaintenanceCheckType] in table 'MaintenanceChecks'
ALTER TABLE [dbo].[MaintenanceChecks]
ADD CONSTRAINT [FK_MaintenanceChecks_MaintenanceCheckType]
    FOREIGN KEY ([MaintenanceCheckType])
    REFERENCES [dbo].[MaintenanceCheckTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceChecks_MaintenanceCheckType'
CREATE INDEX [IX_FK_MaintenanceChecks_MaintenanceCheckType]
ON [dbo].[MaintenanceChecks]
    ([MaintenanceCheckType]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------