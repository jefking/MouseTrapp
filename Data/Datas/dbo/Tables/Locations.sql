CREATE TABLE [dbo].[Locations] (
    [TrapId]   UNIQUEIDENTIFIER NOT NULL,
    [Building] VARCHAR (30)     NOT NULL,
    [Location] VARCHAR (30)     NOT NULL,
    [X]        INT              NOT NULL,
    [y]        INT              NOT NULL,
    CONSTRAINT [PKLocations] PRIMARY KEY CLUSTERED ([TrapId] ASC)
);

