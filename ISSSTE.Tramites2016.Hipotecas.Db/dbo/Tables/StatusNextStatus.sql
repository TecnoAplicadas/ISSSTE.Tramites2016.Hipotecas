CREATE TABLE [dbo].[StatusNextStatus] (
    [StatusNextStatusId] INT IDENTITY (1, 1) NOT NULL,
    [StatusId]           INT NOT NULL,
    [NextStatusId]       INT NOT NULL,
    CONSTRAINT [PK_StatusNextStatus] PRIMARY KEY CLUSTERED ([StatusNextStatusId] ASC),
    CONSTRAINT [FK_StatusNextStatus_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusId]),
    CONSTRAINT [FK_StatusNextStatus_Status1] FOREIGN KEY ([NextStatusId]) REFERENCES [dbo].[Status] ([StatusId])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene el estatus siguiente de la solicitud dependiendo del flujo de proceso.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'StatusNextStatus';

