CREATE TABLE [dbo].[SpecialDays] (
    [DelegationId] INT  NOT NULL,
    [Date]         DATE NOT NULL,
    [IsNonWorking] BIT  NOT NULL,
    [IsOverrided]  BIT  NOT NULL,
    CONSTRAINT [PK_SpecialDays] PRIMARY KEY CLUSTERED ([DelegationId] ASC, [Date] ASC),
    CONSTRAINT [FK_SpecialDays_Delegations] FOREIGN KEY ([DelegationId]) REFERENCES [dbo].[Delegations] ([DelegationId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Contiene fechas de días especiales.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDays';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código de la delegación donde aplica el día.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDays', @level2type = N'COLUMN', @level2name = N'DelegationId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha exacta del día.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDays', @level2type = N'COLUMN', @level2name = N'Date';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el día es o no laborable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDays', @level2type = N'COLUMN', @level2name = N'IsNonWorking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Indica si el día es anulado.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpecialDays', @level2type = N'COLUMN', @level2name = N'IsOverrided';

