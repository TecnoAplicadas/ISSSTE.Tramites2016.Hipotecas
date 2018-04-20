CREATE TABLE [dbo].[Schedules] (
    [ScheduleId]   UNIQUEIDENTIFIER CONSTRAINT [DF_Schedules_ScheduleId] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [DelegationId] INT              NOT NULL,
    [WeekdayId]    INT              NOT NULL,
    [Time]         TIME (7)         NOT NULL,
    [Capacity]     INT              NOT NULL,
    CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED ([ScheduleId] ASC),
    CONSTRAINT [FK_Schedules_Delegations] FOREIGN KEY ([DelegationId]) REFERENCES [dbo].[Delegations] ([DelegationId])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los horarios para las citas por delegacion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de los horarios.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules', @level2type = N'COLUMN', @level2name = N'ScheduleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de la delegación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules', @level2type = N'COLUMN', @level2name = N'DelegationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Día de la semana ligada al horario.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules', @level2type = N'COLUMN', @level2name = N'WeekdayId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Hora del día ligada al horario.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Capacidad de atención en el horario determinado.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Schedules', @level2type = N'COLUMN', @level2name = N'Capacity';

