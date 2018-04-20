CREATE TABLE [dbo].[Roles] (
    [RoleId]      INT CONSTRAINT [DF_Roles_RoleId] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]        VARCHAR (256)    NOT NULL,
    [Description] VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los diferentes roles que se manejaran para gestionar los derechos de procesos a través de privilegios.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación del rol.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'RoleId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre del rol.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del rol.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Roles', @level2type = N'COLUMN', @level2name = N'Description';

