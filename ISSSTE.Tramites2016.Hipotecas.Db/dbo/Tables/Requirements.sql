CREATE TABLE [dbo].[Requirements] (
    [RequirementId] INT           IDENTITY (1, 1) NOT NULL,
    [Description]   VARCHAR (256) NULL,
    [PensionId]     INT           NOT NULL,
    CONSTRAINT [PK_Requirements] PRIMARY KEY CLUSTERED ([RequirementId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los requisitos para cada una de las pensiones.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Requirements';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de los requisitos por cada tipo de pensión.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Requirements', @level2type = N'COLUMN', @level2name = N'RequirementId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción de la pensión a la que corresponden los requisitos actuales.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Requirements', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de la Pensión a la que corresponden los requisitos actuales.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Requirements', @level2type = N'COLUMN', @level2name = N'PensionId';

