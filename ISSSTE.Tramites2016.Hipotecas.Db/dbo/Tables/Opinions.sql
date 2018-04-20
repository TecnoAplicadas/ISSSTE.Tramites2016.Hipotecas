CREATE TABLE [dbo].[Opinions] (
    [OpinionId] INT              IDENTITY (1, 1) NOT NULL,
    [RequestId] UNIQUEIDENTIFIER NOT NULL,
    [Opinion]   VARCHAR (MAX)    NOT NULL,
    CONSTRAINT [PK_Opinions] PRIMARY KEY CLUSTERED ([OpinionId] ASC),
    CONSTRAINT [FK_Opinions_Request] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Requests] ([RequestId])
);



