-- Creating table 'MaintenanceCheckTypes'
CREATE TABLE [dbo].[MaintenanceCheckTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(50)  NULL,
    [OccurenceMonthly] int  NULL,
    [OccurenceMiles] int  NULL
);
GO
-- Creating primary key on [Id] in table 'MaintenanceCheckTypes'
ALTER TABLE [dbo].[MaintenanceCheckTypes]
ADD CONSTRAINT [PK_MaintenanceCheckTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);