CREATE TABLE [dbo].[Delegations] (
    [DelegationId] INT           NOT NULL,
    [StateKey]     VARCHAR (5)   NOT NULL,
    [Name]         VARCHAR (150) NOT NULL,
    [IsActive]     BIT           NOT NULL,
    [Address]      VARCHAR (MAX) NULL,
    [Phone]        VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Delegations] PRIMARY KEY CLUSTERED ([DelegationId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Datos de las delegaciones, contiene  la clave, nombre y  estado de cada una de ellas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Delegations';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de la delegación.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Delegations', @level2type = N'COLUMN', @level2name = N'DelegationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código del estado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Delegations', @level2type = N'COLUMN', @level2name = N'StateKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la delegación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Delegations', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Almacena si esta activa o no la delegación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Delegations', @level2type = N'COLUMN', @level2name = N'IsActive';

