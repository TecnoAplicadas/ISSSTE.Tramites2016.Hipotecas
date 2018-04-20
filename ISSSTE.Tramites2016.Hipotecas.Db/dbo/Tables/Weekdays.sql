CREATE TABLE [dbo].[Weekdays] (
    [WeekdayId]   INT           NOT NULL,
    [Description] VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Weekdays] PRIMARY KEY CLUSTERED ([WeekdayId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene días de la semana como catalogo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Weekdays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código que indica el día de la semana.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Weekdays', @level2type = N'COLUMN', @level2name = N'WeekdayId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción sobre el día de la semana del que se trata.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Weekdays', @level2type = N'COLUMN', @level2name = N'Description';

