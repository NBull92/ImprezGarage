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
-- Creating primary key on [Id] in table 'PartsReplacementRecords'
ALTER TABLE [dbo].[PartsReplacementRecords]
ADD CONSTRAINT [PK_PartsReplacementRecords]
    PRIMARY KEY CLUSTERED ([Id] ASC);