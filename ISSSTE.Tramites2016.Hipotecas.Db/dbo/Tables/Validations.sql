CREATE TABLE [dbo].[Validations] (
    [ValidationId]         INT IDENTITY (1, 1) NOT NULL,
    [IsGeneralDataCorrect] BIT NOT NULL,
    [IsWorkHistoryCorrect] BIT NOT NULL,
    [IsDebtorsCorrect]     BIT NOT NULL,
    [IsBeneficiaries]      BIT NOT NULL,
    [IsCurpCorrect]        BIT NOT NULL,
    CONSTRAINT [PK_Validations] PRIMARY KEY CLUSTERED ([ValidationId] ASC)
);

