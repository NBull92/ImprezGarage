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
-- Creating foreign key on [MaintenanceCheckType] in table 'MaintenanceChecks'
ALTER TABLE [dbo].[MaintenanceChecks]
ADD CONSTRAINT [FK_MaintenanceChecks_MaintenanceCheckType]
    FOREIGN KEY ([MaintenanceCheckType])
    REFERENCES [dbo].[MaintenanceCheckTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'MaintenanceChecks'
ALTER TABLE [dbo].[MaintenanceChecks]
ADD CONSTRAINT [PK_MaintenanceChecks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceChecks_MaintenanceCheckType'
CREATE INDEX [IX_FK_MaintenanceChecks_MaintenanceCheckType]
ON [dbo].[MaintenanceChecks]
    ([MaintenanceCheckType]);