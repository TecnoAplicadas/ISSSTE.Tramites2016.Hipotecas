CREATE TABLE [dbo].[RequestStatus] (
    [RequestStatusId] UNIQUEIDENTIFIER CONSTRAINT [DF_RequestStatus_RequestStatusId] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [RequestId]       UNIQUEIDENTIFIER NOT NULL,
    [StatusId]        INT              NOT NULL,
    [Date]            DATE             NOT NULL,
    [IsCurrentStatus] BIT              NOT NULL,
    [UserId]          VARCHAR (70)     NOT NULL,
    [Observations]    VARCHAR (256)    NULL,
    [ElapsedDays]     INT              NOT NULL,
    [ElapsedWorkDays] INT              NOT NULL,
    [Data]            VARCHAR (1024)   NULL,
    CONSTRAINT [PK_RequestStatus] PRIMARY KEY CLUSTERED ([RequestStatusId] ASC),
    CONSTRAINT [FK_RequestStatus_Request] FOREIGN KEY ([RequestId]) REFERENCES [dbo].[Requests] ([RequestId]),
    CONSTRAINT [FK_RequestStatus_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Status] ([StatusId])
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene información sobre el estatus actual de la solicitud.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación del status de la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'RequestStatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de la solicitud.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'RequestId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación del status.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'StatusId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha del estatus de la solicitud.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena si el estatus es ono el actual de la solicitud.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'IsCurrentStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena la clave del usuario.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'UserId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Observaciones del estatus de la solicitud en caso de requerirse.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'Observations';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena los días transcurridos entre el estatus anterior y el actual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'ElapsedDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena los días naturales  transcurridos entre el estatus anterior y el actual', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RequestStatus', @level2type = N'COLUMN', @level2name = N'ElapsedWorkDays';

