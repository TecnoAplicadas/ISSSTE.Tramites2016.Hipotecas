CREATE TABLE [dbo].[Appoinments](
	[AppoinmentId] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [uniqueidentifier] NOT NULL,
	[DelegationId] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[Time] [time](7) NOT NULL,
	[IsAttended] [bit] NOT NULL,
	[IsCancelled] [bit] NOT NULL,
 CONSTRAINT [PK_Appoinments] PRIMARY KEY CLUSTERED 
(
	[AppoinmentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
