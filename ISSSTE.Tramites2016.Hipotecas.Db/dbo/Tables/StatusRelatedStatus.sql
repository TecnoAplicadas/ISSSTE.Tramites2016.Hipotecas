CREATE TABLE [dbo].[StatusRelatedStatus] (
    [StatusNextStatusId] INT IDENTITY (1, 1) NOT NULL,
    [StatusId]           INT NOT NULL,
    [RelatesStatusId]    INT NOT NULL,
    CONSTRAINT [PK_StatusRelatedStatus] PRIMARY KEY CLUSTERED ([StatusNextStatusId] ASC),
    CONSTRAINT [FK_StatusRelatedStatus_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusId]),
    CONSTRAINT [FK_StatusRelatedStatus_Status1] FOREIGN KEY ([RelatesStatusId]) REFERENCES [dbo].[Status] ([StatusId])
);

