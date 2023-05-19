USE [Acumatica22202]
GO

DROP TABLE IF EXISTS [dbo].[PMSampleTesting]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PMSampleTesting](
	[ProjectID] [int] NOT NULL,
    [PlanName] [nvarchar](255),
	[AdditionalNotes] [nvarchar](510),
	[Complete] [bit] NOT NULL,
    [Tester] [int],
    [RecordKeeperName] [nvarchar](3),
    [Priority] [nvarchar](1),
	[FirstContactDate] [datetime],
	[TPA] [nvarchar](255),
	[CO01] [nvarchar](2),
	[CO01LastModifiedDateTime] [datetime],
	[CO01LastModUserName] [nvarchar](256),
	[CO02] [nvarchar](2),
	[CO02LastModifiedDateTime] [datetime],
	[CO02LastModUserName] [nvarchar](256),
	[CO03] [nvarchar](2),
	[CO03LastModifiedDateTime] [datetime],
	[CO03LastModUserName] [nvarchar](256),
	[CO04] [nvarchar](2),
	[CO04LastModifiedDateTime] [datetime],
	[CO04LastModUserName] [nvarchar](256),
	[CO05] [nvarchar](2),
	[CO05LastModifiedDateTime] [datetime],
	[CO05LastModUserName] [nvarchar](256),
	[CO06] [nvarchar](2),
	[CO06LastModifiedDateTime] [datetime],
	[CO06LastModUserName] [nvarchar](256),
	[CO07] [nvarchar](2),
	[CO07LastModifiedDateTime] [datetime],
	[CO07LastModUserName] [nvarchar](256),
	[CO08] [nvarchar](2),
	[CO08LastModifiedDateTime] [datetime],
	[CO08LastModUserName] [nvarchar](256),
	[BP01] [nvarchar](2),
	[BP01LastModifiedDateTime] [datetime],
	[BP01LastModUserName] [nvarchar](256),
	[LO01] [nvarchar](2),
	[LO01LastModifiedDateTime] [datetime],
	[LO01LastModUserName] [nvarchar](256),
	[IA08] [nvarchar](2),
	[IA08LastModifiedDateTime] [datetime],
	[IA08LastModUserName] [nvarchar](256),
	[CreatedDateTime] [datetime] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[tstamp] [timestamp] NOT NULL,
	[NoteID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PMSampleTesting] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PMSampleTesting] ADD  CONSTRAINT [DF_PMSampleTesting_ProjectID]  DEFAULT ((0)) FOR [ProjectID]
GO

CREATE UNIQUE NONCLUSTERED INDEX [PMSampleTesting_ProjectID_Uindex] ON [dbo].[PMSampleTesting]
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
