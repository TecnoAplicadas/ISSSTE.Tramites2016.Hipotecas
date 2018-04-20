CREATE TABLE [dbo].[Status] (
    [StatusId]       INT              NOT NULL,
    [RoleId]         INT NOT NULL,
    [Name]           VARCHAR (50)     NOT NULL,
    [IsCancel]       BIT              NOT NULL,
    [IsNotify]       BIT              NOT NULL,
    [IsShowInReport] BIT              NOT NULL,
    [DueDays]        INT              NOT NULL,
    [NotifyMessage]  VARCHAR (MAX)    NULL,
    [Data]           VARCHAR (1024)   NULL,
    [IsFinal]        BIT              NULL,
    [IsInitial]      BIT              NULL,
    CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED ([StatusId] ASC),
    CONSTRAINT [FK_Status_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([RoleId])
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los diferentes estatus por los que puede atravesar una solicitud además de información relevante sobre estas. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación del status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'StatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Codigo de identificación del rol', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'RoleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del status', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena si la solicitud está o no, en el estado de cancelada', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'IsCancel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena si se enviara o no notificación en el cambio de estatus ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'IsNotify';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena si el status se usara o no en algún reporte.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'IsShowInReport';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Duración en días entre el status anterior y el actual.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Status', @level2type = N'COLUMN', @level2name = N'DueDays';

