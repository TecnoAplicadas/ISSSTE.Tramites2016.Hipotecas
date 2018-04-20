CREATE TABLE [dbo].[Entitles] (
    [EntitleId]        VARCHAR (20)  NOT NULL,
    [NoISSSTE]         VARCHAR (15)  NOT NULL,
    [PaternalLastName] VARCHAR (50)  NULL,
    [MaternalLastName] VARCHAR (50)  NULL,
    [Name]             VARCHAR (50)  NULL,
    [RFC]              VARCHAR (15)  NULL,
    [Age]              INT           NULL,
    [Birthdate]        DATE          NULL,
    [Gender]           VARCHAR (10)  NULL,
    [Street]           VARCHAR (256) NULL,
    [NumExt]           VARCHAR (10)  NULL,
    [NumInt]           VARCHAR (10)  NULL,
    [Colony]           VARCHAR (256) NULL,
    [ZipCode]          VARCHAR (7)   NULL,
    [Email]            VARCHAR (100) NULL,
    [Telephone]        VARCHAR (18)  NULL,
    [City]             VARCHAR (70)  NULL,
    [CURP]             VARCHAR (20)  NULL,
    [MaritalStatus]    VARCHAR (20)  NULL,
    [Birthplace]       VARCHAR (50)  NULL,
    [RegimeType]       VARCHAR (256) NULL,
    [DelegationId] INT NULL, 
    CONSTRAINT [PK_Entitles] PRIMARY KEY CLUSTERED ([EntitleId] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Datos generales de los derechohabientes, contiene la información de cada uno de ellos', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'EntitleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero de ISSSTE del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'NoISSSTE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Apellido paterno del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'PaternalLastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Apellido materno del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'MaternalLastName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ciudad donde reside el derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'City';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CURP del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'CURP';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado civil del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'MaritalStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lugar de nacimiento del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Birthplace';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tipo de régimen del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'RegimeType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero interior del domicilio del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'NumInt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la colonia del domicilio del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Colony';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código postal del domicilio del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Dirección de correo electrónico del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Email';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Número telefónico del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Telephone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'RFC del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'RFC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Edad del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Age';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de nacimiento del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Birthdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Genero del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Gender';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la calle del domicilio del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'Street';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Numero exterior del domicilio del derechohabiente.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Entitles', @level2type = N'COLUMN', @level2name = N'NumExt';

