USE [master]
GO
/****** Object:  Database [millionandup_msproperty]    Script Date: 8/10/2023 5:45:05 p. m. ******/
CREATE DATABASE [millionandup_msproperty]
GO

/****** Object:  Table [dbo].[Owner]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Owner](
	[OwnerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](80) NULL,
	[Address] [nvarchar](125) NULL,
	[Photo] [nvarchar](2048) NULL,
	[Birthday] [date] NULL,
 CONSTRAINT [PK_Owner] PRIMARY KEY CLUSTERED 
(
	[OwnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OwnerByProperty]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OwnerByProperty](
	[OwnerId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OwnerByProperty] PRIMARY KEY CLUSTERED 
(
	[OwnerId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Property]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[PropertyId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](80) NULL,
	[Address] [nvarchar](125) NULL,
	[Price] [money] NULL,
	[CodeInternal] [nvarchar](50) NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyImage]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyImage](
	[PropertyImageId] [uniqueidentifier] NOT NULL,
	[File] [nvarchar](2048) NULL,
	[Enabled] [bit] NULL,
 CONSTRAINT [PK_PropertyImage] PRIMARY KEY CLUSTERED 
(
	[PropertyImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyImageByProperty]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyImageByProperty](
	[PropertyImageId] [uniqueidentifier] NOT NULL,
	[PropertyId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PropertyImageByProperty] PRIMARY KEY CLUSTERED 
(
	[PropertyImageId] ASC,
	[PropertyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyTrace]    Script Date: 8/10/2023 5:45:05 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyTrace](
	[PropertyTraceId] [uniqueidentifier] NOT NULL,
	[DateSale] [datetime] NULL,
	[Name] [nvarchar](80) NULL,
	[Value] [money] NULL,
	[Tax] [money] NULL,
	[PropertyId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PropertyTrace] PRIMARY KEY CLUSTERED 
(
	[PropertyTraceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [DateDale_Desc]    Script Date: 8/10/2023 5:45:05 p. m. ******/
CREATE NONCLUSTERED INDEX [DateDale_Desc] ON [dbo].[PropertyTrace]
(
	[DateSale] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OwnerByProperty]  WITH CHECK ADD  CONSTRAINT [FK_OwnerByProperty_Owner] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Owner] ([OwnerId])
GO
ALTER TABLE [dbo].[OwnerByProperty] CHECK CONSTRAINT [FK_OwnerByProperty_Owner]
GO
ALTER TABLE [dbo].[OwnerByProperty]  WITH CHECK ADD  CONSTRAINT [FK_OwnerByProperty_Property] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Property] ([PropertyId])
GO
ALTER TABLE [dbo].[OwnerByProperty] CHECK CONSTRAINT [FK_OwnerByProperty_Property]
GO
ALTER TABLE [dbo].[PropertyImageByProperty]  WITH CHECK ADD  CONSTRAINT [FK_PropertyImageByProperty_Property] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Property] ([PropertyId])
GO
ALTER TABLE [dbo].[PropertyImageByProperty] CHECK CONSTRAINT [FK_PropertyImageByProperty_Property]
GO
ALTER TABLE [dbo].[PropertyImageByProperty]  WITH CHECK ADD  CONSTRAINT [FK_PropertyImageByProperty_PropertyImage] FOREIGN KEY([PropertyImageId])
REFERENCES [dbo].[PropertyImage] ([PropertyImageId])
GO
ALTER TABLE [dbo].[PropertyImageByProperty] CHECK CONSTRAINT [FK_PropertyImageByProperty_PropertyImage]
GO
ALTER TABLE [dbo].[PropertyTrace]  WITH CHECK ADD  CONSTRAINT [FK_PropertyTrace_Property] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Property] ([PropertyId])
GO
ALTER TABLE [dbo].[PropertyTrace] CHECK CONSTRAINT [FK_PropertyTrace_Property]
GO
USE [master]
GO
ALTER DATABASE [millionandup_msproperty] SET  READ_WRITE 
GO
