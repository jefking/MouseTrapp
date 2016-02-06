CREATE TABLE [dbo].[TrapEvents] (
    [Id]        INT              IDENTITY (1, 1) NOT NULL,
    [TrapId]    UNIQUEIDENTIFIER NOT NULL,
    [Building]  VARCHAR (30)     NOT NULL,
    [Location]  VARCHAR (30)     NOT NULL,
    [EventDate] DATETIME         NOT NULL,
    [EventType] TINYINT          NOT NULL,
    CONSTRAINT [PKTrapevents] PRIMARY KEY CLUSTERED ([Id] ASC)
);

