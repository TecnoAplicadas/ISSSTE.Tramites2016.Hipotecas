CREATE TABLE [dbo].[MaximumSalaries](
	[SalaryId] [int] IDENTITY(1,1) NOT NULL,
	[SalaryCap] [decimal](18, 2) NULL,
	[year] [bigint] NULL,
 CONSTRAINT [PK_SalaryId] PRIMARY KEY CLUSTERED 
([SalaryId] ASC)
);


