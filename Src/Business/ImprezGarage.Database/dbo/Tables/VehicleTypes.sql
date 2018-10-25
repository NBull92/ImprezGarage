-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'VehicleTypes'
CREATE TABLE [dbo].[VehicleTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NULL
);
GO
-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'VehicleTypes'
ALTER TABLE [dbo].[VehicleTypes]
ADD CONSTRAINT [PK_VehicleTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);