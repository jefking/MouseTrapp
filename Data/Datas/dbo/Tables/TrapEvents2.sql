CREATE TABLE [dbo].[TrapEvents2] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [TrapId]    VARCHAR (50) NULL,
    [Building]  VARCHAR (30) NULL,
    [Location]  VARCHAR (30) NULL,
    [EventDate] VARCHAR (30) NULL,
    [EventType] VARCHAR (30) NULL,
    CONSTRAINT [PKTrapevents2] PRIMARY KEY CLUSTERED ([Id] ASC)
);

