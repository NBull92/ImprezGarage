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
-- Creating primary key on [Id] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [PK_Vehicles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_Vehicles_VehicleType'
CREATE INDEX [IX_FK_Vehicles_VehicleType]
ON [dbo].[Vehicles]
    ([VehicleType]);