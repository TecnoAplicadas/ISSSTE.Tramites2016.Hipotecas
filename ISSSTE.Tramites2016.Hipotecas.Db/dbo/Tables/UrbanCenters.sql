CREATE TABLE [dbo].[UrbanCenters](
	[IdUrbanCenter] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](250) NOT NULL,
	[Ubication] [varchar](max) NOT NULL,
	[NoEdif] [int] NOT NULL,
	[NoDeptos] [int] NOT NULL,
	[ComercialLocals] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[Locality] [varchar](150) NULL,
 CONSTRAINT [PK_UrbanCenters] PRIMARY KEY CLUSTERED 
(
	[IdUrbanCenter] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
