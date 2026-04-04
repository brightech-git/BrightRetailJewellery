USE [RTMSAVINGS]
GO

/****** Object:  Table [dbo].[GiftTran]    Script Date: 2026/02/28 3:28:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GiftVoucher](
	[COSTID] [varchar](2) NULL,
	[TranNo] [varchar](32) NULL,
	[TranDate] [smalldatetime] NULL,
	[TranCode] [int] NULL,
	[GiftId] [int] NULL,
	[SupplierId] [int] NULL,
	[ISSREC] [varchar](1) NULL,
	[Rate] [int] NULL,
	[CompanyId] [varchar](3) NULL,
	[SchemeId] [int] NULL,
	[InsAmount] [float] NULL,
	[GROUPCODE] [varchar](15) NULL,
	[RegNo] [int] NULL,
	[Pieces] [int] NULL,
	[Remarks] [varchar](50) NULL,
	[MultiTrans] [varchar](1) NULL,
	[Cancel] [varchar](1) NULL,
	[UserId] [int] NULL,
	[Updatetime] [smalldatetime] NULL,
	[EntRefNo] [varchar](30) NULL,
	[RECEIVERNAME] [varchar](50) NULL,
	[EXTRAAMOUNT] [numeric](12, 2) NULL,
	[APPVER] [varchar](15) NULL,
	[TAGNO] [varchar](15) NULL
) ON [PRIMARY]
GO


