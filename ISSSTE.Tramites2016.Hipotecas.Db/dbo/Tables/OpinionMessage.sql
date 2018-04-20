CREATE TABLE [dbo].[OpinionMessage] (
    [OpinionMessageId] INT            IDENTITY (1, 1) NOT NULL,
    [Message]          NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_OpinionMessage] PRIMARY KEY CLUSTERED ([OpinionMessageId] ASC)
);

