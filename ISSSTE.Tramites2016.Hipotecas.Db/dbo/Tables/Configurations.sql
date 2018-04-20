CREATE TABLE [dbo].[Configurations] (
    [ConfiguratonId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (256) NOT NULL,
    [Value]          VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Configurations] PRIMARY KEY CLUSTERED ([ConfiguratonId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene valores a configurar para el correcto funcionamiento de la  aplicación. ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Configurations';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de la configuración.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Configurations', @level2type = N'COLUMN', @level2name = N'ConfiguratonId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del componente a configurar', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Configurations', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Valor asignado al componente a configurar.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Configurations', @level2type = N'COLUMN', @level2name = N'Value';

