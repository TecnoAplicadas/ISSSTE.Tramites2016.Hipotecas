CREATE TABLE [dbo].[SpecialDaysSchedules] (
    [SpecialDayScheduleId] UNIQUEIDENTIFIER CONSTRAINT [DF_SpecialDaysSchedules_SpecialDayScheduleId] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [DelegationId]         INT              NOT NULL,
    [Date]                 DATE             NOT NULL,
    [Time]                 TIME (7)         NOT NULL,
    [Capacity]             INT              NOT NULL,
    CONSTRAINT [PK_SpecialDaysSchedules] PRIMARY KEY CLUSTERED ([SpecialDayScheduleId] ASC),
    CONSTRAINT [FK_SpecialDaysSchedules_Delegations] FOREIGN KEY ([DelegationId]) REFERENCES [dbo].[Delegations] ([DelegationId])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los días especiales donde habrá citas de cierto tipo.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de los horarios que aplican en ese día.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules', @level2type = N'COLUMN', @level2name = N'SpecialDayScheduleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código  de la delegación donde aplica el día.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules', @level2type = N'COLUMN', @level2name = N'DelegationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha donde aplicaran los horarios.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora del día ligada al horario.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Capacidad de atención en el horario determinado.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDaysSchedules', @level2type = N'COLUMN', @level2name = N'Capacity';

