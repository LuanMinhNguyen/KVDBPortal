USE [Infor_OS_Integrate]
GO
/****** Object:  Table [dbo].[TagIntegrate]    Script Date: 4/7/2021 11:27:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagIntegrate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EAMTagCode] [varchar](500) NULL,
	[ICSSTagCode] [varchar](500) NULL,
	[TotalRunningHour] [varchar](200) NULL,
	[TagStatus] [varchar](200) NULL,
	[SyncTime] [varchar](200) NULL,
 CONSTRAINT [PK_TagIntegrate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagIntegrate_TEMP]    Script Date: 4/7/2021 11:27:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagIntegrate_TEMP](
	[ID] [int] NOT NULL,
	[EAMTagCode] [varchar](500) NULL,
	[ICSSTagCode] [varchar](500) NULL,
	[TotalRunningHour] [varchar](200) NULL,
	[TagStatus] [varchar](200) NULL,
	[SyncTime] [varchar](200) NULL,
 CONSTRAINT [PK_TagIntegrate_TEMP] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[TagIntegrate] ON 

INSERT [dbo].[TagIntegrate] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (1, N'SV-80-EC-001A', N'Root.AREA.CMMS.CANRGBHOURS.PV.Value', N'100', N'1', N'Mar 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (2, N'SV-80-EC-001B', N'Root.AREA.CMMS.DANRGBHOURS.PV.Value', N'200', N'1', N'Mar 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (3, N'SV-23-DT-003A', N'Root.AREA.CMMS.AANRGBHOURS.PV.Value', N'300', N'1', N'Mar 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (4, N'SV-23-DT-003B', N'Root.AREA.CMMS.BANRGBHOURS.PV.Value', N'400', N'1', N'Jan 27 2021 12:00AM')
SET IDENTITY_INSERT [dbo].[TagIntegrate] OFF
GO
INSERT [dbo].[TagIntegrate_TEMP] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (1, N'SV-80-EC-001A', N'Root.AREA.CMMS.CANRGBHOURS.PV.Value', N'0', N'1', N'Jan 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate_TEMP] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (2, N'SV-80-EC-001B', N'Root.AREA.CMMS.DANRGBHOURS.PV.Value', N'0', N'1', N'Jan 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate_TEMP] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (3, N'SV-23-DT-003A', N'Root.AREA.CMMS.AANRGBHOURS.PV.Value', N'0', N'1', N'Jan 27 2021 12:00AM')
INSERT [dbo].[TagIntegrate_TEMP] ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]) VALUES (4, N'SV-23-DT-003B', N'Root.AREA.CMMS.BANRGBHOURS.PV.Value', N'0', N'1', N'Jan 27 2021 12:00AM')
GO
/****** Object:  StoredProcedure [dbo].[PutTagIntegrate]    Script Date: 4/7/2021 11:27:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PutTagIntegrate]
@xmlData XML
AS
BEGIN
  -- Prevent extra result sets from interfering with SELECT statements
  SET NOCOUNT ON;

  -- Parse XML
  DECLARE @parsedXmlData int
  EXEC sp_xml_preparedocument @parsedXmlData OUTPUT, @xmlData

  -- Insert TagIntegrate
  INSERT INTO TagIntegrate_TEMP ([ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime])
  SELECT [ID], [EAMTagCode], [ICSSTagCode], [TotalRunningHour], [TagStatus], [SyncTime]  
  FROM OPENXML (@parsedXmlData, '/DataArea/TagIntegrate', 2)
  WITH (ID		int			'ID',
        EAMTagCode	varchar(500) 'EAMTagCode',
        ICSSTagCode	varchar(500) 'ICSSTagCode',
		TotalRunningHour	varchar(500) 'TotalRunningHour',
		TagStatus	varchar(500) 'TagStatus',
		SyncTime	varchar(500) 'SyncTime')

END
GO
/****** Object:  StoredProcedure [dbo].[TagIntegrate_Read]    Script Date: 4/7/2021 11:27:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TagIntegrate_Read] 
	
AS
BEGIN
--	SELECT 1 AS Tag,
--NULL AS Parent,
--null AS [DataArea!1!ID],
--null AS [Tag!2!EAMTagCode],
--NULL AS [Tag!2!ICSSTagCode],
--NULL AS [Tag!2!TotalRunningHour],
--NULL AS [Tag!2!TagStatus],
--NULL AS [Tag!2!SyncTime]
--FROM [dbo].[TagIntegrate]
--UNION
--SELECT 2 as Tag,
--       1 as Parent,
--	   ID,
--       EAMTagCode, 
--       ICSSTagCode, 
--       TotalRunningHour, 
--       TagStatus, 
--       SyncTime
--FROM [dbo].[TagIntegrate] 
--FOR XML EXPLICIT;

-- Prevent extra result sets from interfering with SELECT statements
SET NOCOUNT ON;

  -- Declare variables
SET ARITHABORT ON

DECLARE @i  INT

	SELECT(
	SELECT ID,
       EAMTagCode, 
       ICSSTagCode, 
       TotalRunningHour, 
       TagStatus, 
       SyncTime
	FROM TagIntegrate      
	--WHERE  employee.lastdate > DATEADD(day,DATEDIFF(day,0,GETDATE())-1,0)
	   --FOR XML AUTO, ELEMENTS, root('DataArea');
	   for xml auto, ELEMENTS, type).query('for $i in /TagIntegrate return <DataArea>{$i}</DataArea>');

END
GO
