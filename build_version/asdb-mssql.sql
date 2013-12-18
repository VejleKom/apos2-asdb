/***** Ret databasenavn hvis det ikke er ASDB *****/
USE [ASDB]
GO
/****** Object:  Table [dbo].[unitlocation]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[unitlocation](
	[unitUuid] [nvarchar](36) NULL,
	[locationUuid] [nvarchar](36) NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.unitlocation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'unitlocation'
GO
/****** Object:  Table [dbo].[unit]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[unit](
	[uuid] [varchar](36) NOT NULL,
	[type] [varchar](5) NOT NULL,
	[objectid] [int] NOT NULL,
	[overordnetid] [int] NOT NULL,
	[navn] [varchar](255) NOT NULL,
	[brugervendtNoegle] [varchar](5) NOT NULL,
 CONSTRAINT [unit$uuid] UNIQUE CLUSTERED 
(
	[uuid] ASC,
	[objectid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.unit' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'unit'
GO
/****** Object:  Table [dbo].[person]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[person](
	[uuid] [nvarchar](36) NULL,
	[objektid] [int] NOT NULL,
	[userKey] [nvarchar](255) NULL,
	[personNumber] [nvarchar](10) NULL,
	[givenName] [nvarchar](255) NULL,
	[surName] [nvarchar](255) NULL,
	[addresseringsnavn] [nvarchar](45) NOT NULL,
	[koen] [nvarchar](5) NOT NULL,
 CONSTRAINT [person$objektid] UNIQUE CLUSTERED 
(
	[objektid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [person$uuid] UNIQUE NONCLUSTERED 
(
	[uuid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.person' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'person'
GO
/****** Object:  Table [dbo].[location]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[location](
	[uuid] [nvarchar](36) NULL,
	[objektid] [int] NOT NULL,
	[postnummer] [int] NOT NULL,
	[postdistrikt] [nvarchar](35) NOT NULL,
	[bynavn] [nvarchar](35) NOT NULL,
	[vejnavn] [nvarchar](35) NOT NULL,
	[husnummer] [nvarchar](5) NOT NULL,
	[name] [nvarchar](255) NULL,
	[coordinate-lat] [nvarchar](12) NULL,
	[coordinate-long] [nvarchar](12) NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.location' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'location'
GO
/****** Object:  Table [dbo].[klassifikation]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[klassifikation](
	[uuid] [varchar](36) NOT NULL,
	[objectid] [int] NOT NULL,
	[brugervendtnoegle] [varchar](35) NOT NULL,
	[kaldenavn] [varchar](35) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.klassifikation' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'klassifikation'
GO
/****** Object:  Table [dbo].[jobtitles]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[jobtitles](
	[uuid] [varchar](36) NOT NULL,
	[objektid] [int] NOT NULL,
	[title] [varchar](45) NOT NULL,
	[brugervendtnoegle] [varchar](45) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.jobtitles' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'jobtitles'
GO
/****** Object:  Table [dbo].[functionunits]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[functionunits](
	[functionUuid] [varchar](36) NOT NULL,
	[unitUuid] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.functionunits' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'functionunits'
GO
/****** Object:  Table [dbo].[functiontasks]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[functiontasks](
	[functionUuid] [varchar](36) NOT NULL,
	[taskUuid] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.functiontasks' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'functiontasks'
GO
/****** Object:  Table [dbo].[functions]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[functions](
	[functionUuid] [varchar](36) NOT NULL,
	[objectid] [int] NOT NULL,
	[name] [varchar](30) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.functions' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'functions'
GO
/****** Object:  Table [dbo].[functionpersons]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[functionpersons](
	[functionUuid] [varchar](36) NOT NULL,
	[personUuid] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.functionpersons' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'functionpersons'
GO
/****** Object:  Table [dbo].[facetter]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[facetter](
	[uuid] [varchar](36) NOT NULL,
	[type] [varchar](5) NOT NULL,
	[objektid] [int] NOT NULL,
	[brugervendtnoegle] [varchar](35) NOT NULL,
	[title] [varchar](75) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.facetter' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'facetter'
GO
/****** Object:  Table [dbo].[engagement]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[engagement](
	[uuid] [nvarchar](36) NULL,
	[stillingUuid] [nvarchar](36) NULL,
	[userKey] [nvarchar](255) NULL,
	[personUuid] [nvarchar](36) NULL,
	[unitUuid] [nvarchar](36) NULL,
	[locationUuid] [nvarchar](36) NULL,
	[name] [nvarchar](255) NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.engagement' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'engagement'
GO
/****** Object:  Table [dbo].[contactchannel]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[contactchannel](
	[uuid] [nvarchar](36) NULL,
	[ownerUuid] [nvarchar](36) NULL,
	[typeUuid] [nvarchar](36) NULL,
	[value] [nvarchar](255) NULL,
	[order_r] [int] NULL,
	[usages] [nvarchar](255) NULL
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.contactchannel' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'contactchannel'
GO
/****** Object:  Table [dbo].[attachedpersons]    Script Date: 12/04/2013 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[attachedpersons](
	[unitUuid] [varchar](36) NOT NULL,
	[personUuid] [varchar](36) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_SSMA_SOURCE', @value=N'asdb.attachedpersons' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'attachedpersons'
GO
/****** Object:  Default [DF__contactcha__uuid__08EA5793]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [uuid]
GO
/****** Object:  Default [DF__contactch__owner__09DE7BCC]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [ownerUuid]
GO
/****** Object:  Default [DF__contactch__typeU__0AD2A005]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [typeUuid]
GO
/****** Object:  Default [DF__contactch__value__0BC6C43E]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [value]
GO
/****** Object:  Default [DF__contactch__order__0CBAE877]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [order_r]
GO
/****** Object:  Default [DF__contactch__usage__0DAF0CB0]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[contactchannel] ADD  DEFAULT (NULL) FOR [usages]
GO
/****** Object:  Default [DF__engagement__uuid__0F975522]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [uuid]
GO
/****** Object:  Default [DF__engagemen__still__108B795B]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [stillingUuid]
GO
/****** Object:  Default [DF__engagemen__userK__117F9D94]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [userKey]
GO
/****** Object:  Default [DF__engagemen__perso__1273C1CD]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [personUuid]
GO
/****** Object:  Default [DF__engagemen__unitU__1367E606]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [unitUuid]
GO
/****** Object:  Default [DF__engagemen__locat__145C0A3F]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [locationUuid]
GO
/****** Object:  Default [DF__engagement__name__15502E78]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[engagement] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__location__uuid__1DE57479]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[location] ADD  DEFAULT (NULL) FOR [uuid]
GO
/****** Object:  Default [DF__location__name__1ED998B2]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[location] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__location__coordi__1FCDBCEB]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[location] ADD  DEFAULT (NULL) FOR [coordinate-lat]
GO
/****** Object:  Default [DF__location__coordi__20C1E124]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[location] ADD  DEFAULT (NULL) FOR [coordinate-long]
GO
/****** Object:  Default [DF__person__uuid__22AA2996]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[person] ADD  DEFAULT (NULL) FOR [uuid]
GO
/****** Object:  Default [DF__person__userKey__239E4DCF]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[person] ADD  DEFAULT (NULL) FOR [userKey]
GO
/****** Object:  Default [DF__person__personNu__24927208]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[person] ADD  DEFAULT (NULL) FOR [personNumber]
GO
/****** Object:  Default [DF__person__givenNam__25869641]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[person] ADD  DEFAULT (NULL) FOR [givenName]
GO
/****** Object:  Default [DF__person__surName__267ABA7A]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[person] ADD  DEFAULT (NULL) FOR [surName]
GO
/****** Object:  Default [DF__unitlocat__unitU__29572725]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[unitlocation] ADD  DEFAULT (NULL) FOR [unitUuid]
GO
/****** Object:  Default [DF__unitlocat__locat__2A4B4B5E]    Script Date: 12/04/2013 11:16:37 ******/
ALTER TABLE [dbo].[unitlocation] ADD  DEFAULT (NULL) FOR [locationUuid]
GO
