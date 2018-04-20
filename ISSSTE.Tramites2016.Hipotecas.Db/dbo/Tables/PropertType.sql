﻿CREATE TABLE [dbo].[PropertType](
	[IdPropertyType] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](250) NULL,
	[Active] [bit] NULL,
	[Acronym] [varchar](2) NULL,
 CONSTRAINT [PK_PropertType] PRIMARY KEY CLUSTERED 
(
	[IdPropertyType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
