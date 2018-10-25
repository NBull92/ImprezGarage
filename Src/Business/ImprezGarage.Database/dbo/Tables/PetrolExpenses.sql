-- Creating table 'PetrolExpenses'
CREATE TABLE [dbo].[PetrolExpenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] float  NULL,
    [DateEntered] datetime  NULL,
    [VehicleId] int  NULL
);
GO
-- Creating primary key on [Id] in table 'PetrolExpenses'
ALTER TABLE [dbo].[PetrolExpenses]
ADD CONSTRAINT [PK_PetrolExpenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);