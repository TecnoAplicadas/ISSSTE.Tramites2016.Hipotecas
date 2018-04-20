CREATE TABLE [dbo].[Messages] (
    [MessageId]   INT           IDENTITY (1, 1) NOT NULL,
    [Key]         VARCHAR (20)  NOT NULL,
    [Description] VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([MessageId] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene los mensajes para la aplicación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Messages';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de identificación de los mensajes.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Messages', @level2type = N'COLUMN', @level2name = N'MessageId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Mnemónico del mensaje.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Messages', @level2type = N'COLUMN', @level2name = N'Key';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Descripción del mensaje.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Messages', @level2type = N'COLUMN', @level2name = N'Description';

