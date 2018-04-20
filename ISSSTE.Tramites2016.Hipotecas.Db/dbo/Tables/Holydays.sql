CREATE TABLE [dbo].[Holydays] (
    [Date]        DATE          NOT NULL,
    [Description] VARCHAR (256) NULL,
    CONSTRAINT [PK_Holydays] PRIMARY KEY CLUSTERED ([Date] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los días festivos por año.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holydays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena la fecha de un dia festivo.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holydays', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena la descripción en caso de requerirse del día festivo.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Holydays', @level2type = N'COLUMN', @level2name = N'Description';

