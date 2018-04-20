CREATE TABLE [dbo].[Requests](
	[RequestId] [uniqueidentifier] NOT NULL,
	[EntitleId] [varchar](20) NULL,
	[Folio] [varchar](50) NULL,
	[IsComplete] [bit] NOT NULL,
	[NeedsAuthorization] [bit] NOT NULL,
	[IsAuthorized] [bit] NULL,
	[AuthorizeUserId] [varchar](70) NULL,
	[AuthorizedComments] [varchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[StatusNextId] [int] NOT NULL,
	[IdLegalUnit] [int] NULL,
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


