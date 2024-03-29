
/****** Object:  Database [exam]    Script Date: 14.11.2019 9:40:01 ******/

USE [exam]
GO
/****** Object:  UserDefinedTableType [dbo].[Contracts]    Script Date: 14.11.2019 9:40:01 ******/
CREATE TYPE [dbo].[Contracts] AS TABLE(
	[CustomerCode] [nvarchar](50) NOT NULL,
	[CurrentBalance] [decimal](18, 2) NOT NULL,
	[DateAccountOpened] [datetime] NOT NULL,
	[DateOfLastPayment] [datetime] NOT NULL,
	[InstallmentAmount] [decimal](18, 2) NOT NULL,
	[InstallmentAmountCurrency] [int] NOT NULL,
	[NextPaymentDate] [datetime] NOT NULL,
	[OriginalAmount] [decimal](18, 2) NOT NULL,
	[OriginalAmountCurrency] [int] NOT NULL,
	[OverdueBalance] [decimal](18, 2) NOT NULL,
	[OverdueBalanceCurrency] [int] NOT NULL,
	[PhaseOfContract] [int] NOT NULL,
	[RealEndDate] [datetime] NOT NULL,
	[CurrentBalanceCurrency] [int] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Errors]    Script Date: 14.11.2019 9:40:01 ******/
CREATE TYPE [dbo].[Errors] AS TABLE(
	[CustomerCode] [nvarchar](50) NOT NULL,
	[StringValue] [nvarchar](500) NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[Individual]    Script Date: 14.11.2019 9:40:01 ******/
CREATE TYPE [dbo].[Individual] AS TABLE(
	[CustomerCode] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Gender] [int] NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[NationalID] [nvarchar](50) NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[StringValuesTable]    Script Date: 14.11.2019 9:40:01 ******/
CREATE TYPE [dbo].[StringValuesTable] AS TABLE(
	[StringValue] [nvarchar](500) NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[SubjectRole]    Script Date: 14.11.2019 9:40:01 ******/
CREATE TYPE [dbo].[SubjectRole] AS TABLE(
	[CustomerCode] [nvarchar](50) NOT NULL,
	[GuaranteeAmount] [decimal](18, 2) NULL,
	[GuaranteeAmountCurrency] [int] NULL,
	[RoleOfCustomer] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[ContractCode] [nvarchar](50) NOT NULL,
	[PhaseOfContract] [int] NULL,
	[OriginalAmount] [decimal](18, 2) NULL,
	[OriginalAmountCurrency] [nvarchar](50) NULL,
	[InstallmentAmount] [decimal](18, 2) NULL,
	[InstallmentAmountCurrency] [nvarchar](50) NULL,
	[CurrentBalance] [decimal](18, 2) NULL,
	[CurrentBalanceCurrency] [nvarchar](50) NULL,
	[OverdueBalance] [decimal](18, 2) NULL,
	[OverdueBalanceCurrency] [nvarchar](50) NULL,
	[DateOfLastPayment] [datetime] NULL,
	[NextPaymentDate] [datetime] NULL,
	[DateAccountOpened] [datetime] NULL,
	[RealEndDate] [datetime] NULL,
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[ContractCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[IdErrorLog] [int] IDENTITY(1,1) NOT NULL,
	[ContractCode] [nvarchar](50) NOT NULL,
	[ErrorMessage] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[IdErrorLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Individual]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Individual](
	[CustomerCode] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Gender] [int] NULL,
	[DateOfBirth] [datetime] NULL,
	[NationalID] [nvarchar](50) NULL,
 CONSTRAINT [PK_Individual] PRIMARY KEY CLUSTERED 
(
	[CustomerCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectRole]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectRole](
	[SubjectRoleId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerCode] [nvarchar](50) NULL,
	[RoleOfCustomer] [int] NULL,
	[GuaranteeAmount] [decimal](18, 2) NULL,
	[GuaranteeAmountCurrency] [int] NULL,
 CONSTRAINT [PK_SubjectRole] PRIMARY KEY CLUSTERED 
(
	[SubjectRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[InsertContract]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertContract]
(@ContractCode nvarchar(50) 
,@CurrentBalance  decimal (18,2)
,@DateAccountOpened datetime 
,@DateOfLastPayment datetime
,@InstallmentAmount decimal (18,2)
,@InstallmentAmountCurrency int 
,@NextPaymentDate datetime
,@OriginalAmount decimal (18,2)
,@OriginalAmountCurrency int 
,@OverdueBalance decimal (18,2)
,@OverdueBalanceCurrency int
,@PhaseOfContract int
,@RealEndDate datetime

,@Individuals [dbo].[Individual] readonly
,@SubjectRoles [dbo].[SubjectRole] readonly
,@CurrentBalanceCurrency int
)
as begin
set nocount on;
set xact_abort on;
	begin try

        declare @TranCount int = @@TRANCOUNT
				,@TranName char(32) = replace(newid(), '-', '')

INSERT INTO [dbo].[Contract]
           ([ContractCode]
           ,[PhaseOfContract]
           ,[OriginalAmount]
           ,[OriginalAmountCurrency]
           ,[InstallmentAmount]
           ,[InstallmentAmountCurrency]
           ,[CurrentBalance]
           ,[CurrentBalanceCurrency]
           ,[OverdueBalance]
           ,[OverdueBalanceCurrency]
           ,[DateOfLastPayment]
           ,[NextPaymentDate]
           ,[DateAccountOpened]
           ,[RealEndDate])
     VALUES
           (@ContractCode
           ,@PhaseOfContract
           ,@OriginalAmount
           ,@OriginalAmountCurrency
           ,@InstallmentAmount
           ,@InstallmentAmountCurrency
           ,@CurrentBalance
           ,@CurrentBalanceCurrency
           ,@OverdueBalance
           ,@OverdueBalanceCurrency
           ,@DateOfLastPayment
           ,@NextPaymentDate
           ,@DateAccountOpened
           ,@RealEndDate)
	
insert into [Individual]
select [CustomerCode]
      ,[FirstName]
      ,[LastName]
      ,[Gender]
      ,[DateOfBirth]
      ,[NationalID]
from @Individuals

insert into [SubjectRole]
select [CustomerCode]
      ,[RoleOfCustomer]
      ,[GuaranteeAmount]
      ,[GuaranteeAmountCurrency]
from @SubjectRoles

		if @TranCount = 0
			begin transaction @TranName;
		else
			save transaction @TranName;

    	if @TranCount = 0
    		commit transaction @TranName;
    end try
	begin catch


		if XACT_STATE() = 1 or (@TranCount = 0 AND XACT_STATE() <> 0) 
			rollback transaction @TranName;
	end catch

end
GO
/****** Object:  StoredProcedure [dbo].[InsertContractBulk]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertContractBulk]
(
@Contracts [dbo].[Contracts] readonly
,@Individuals [dbo].[Individual] readonly
,@SubjectRoles [dbo].[SubjectRole] readonly
)
as begin
set nocount on;
set xact_abort on;
	begin try

        declare @TranCount int = @@TRANCOUNT
				,@TranName char(32) = replace(newid(), '-', '')

insert into [Contract]
select [CustomerCode]
           ,[PhaseOfContract]
           ,[OriginalAmount]
           ,[OriginalAmountCurrency]
           ,[InstallmentAmount]
           ,[InstallmentAmountCurrency]
           ,[CurrentBalance]
           ,[CurrentBalanceCurrency]
           ,[OverdueBalance]
           ,[OverdueBalanceCurrency]
           ,[DateOfLastPayment]
           ,[NextPaymentDate]
           ,[DateAccountOpened]
           ,[RealEndDate]
from @Contracts
     
          
	
insert into [Individual]
select [CustomerCode]
      ,[FirstName]
      ,[LastName]
      ,[Gender]
      ,[DateOfBirth]
      ,[NationalID]
from @Individuals

insert into [SubjectRole]
select [CustomerCode]
      ,[RoleOfCustomer]
      ,[GuaranteeAmount]
      ,[GuaranteeAmountCurrency]
from @SubjectRoles

		if @TranCount = 0
			begin transaction @TranName;
		else
			save transaction @TranName;

    	if @TranCount = 0
    		commit transaction @TranName;
    end try
	begin catch


		if XACT_STATE() = 1 or (@TranCount = 0 AND XACT_STATE() <> 0) 
			rollback transaction @TranName;
	end catch

end
GO
/****** Object:  StoredProcedure [dbo].[InsertErrorBulkLog]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[InsertErrorBulkLog]
(
@Errors [dbo].[Errors] readonly

)
as begin
set nocount on;
set xact_abort on;
	begin try

        declare @TranCount int = @@TRANCOUNT
				,@TranName char(32) = replace(newid(), '-', '')

INSERT INTO [dbo].[ErrorLog]
           ([ContractCode]
           ,[ErrorMessage]
           )
    select [CustomerCode], [StringValue] from @Errors


		if @TranCount = 0
			begin transaction @TranName;
		else
			save transaction @TranName;

    	if @TranCount = 0
    		commit transaction @TranName;
    end try
	begin catch


		if XACT_STATE() = 1 or (@TranCount = 0 AND XACT_STATE() <> 0) 
			rollback transaction @TranName;
	end catch

end
GO
/****** Object:  StoredProcedure [dbo].[InsertErrorLog]    Script Date: 14.11.2019 9:40:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertErrorLog]
(@ContractCode nvarchar(50) 
,@Messages [dbo].[StringValuesTable] readonly

)
as begin
set nocount on;
set xact_abort on;
	begin try

        declare @TranCount int = @@TRANCOUNT
				,@TranName char(32) = replace(newid(), '-', '')

INSERT INTO [dbo].[ErrorLog]
           ([ContractCode]
           ,[ErrorMessage]
           )
    select @ContractCode, StringValue from @Messages


		if @TranCount = 0
			begin transaction @TranName;
		else
			save transaction @TranName;

    	if @TranCount = 0
    		commit transaction @TranName;
    end try
	begin catch


		if XACT_STATE() = 1 or (@TranCount = 0 AND XACT_STATE() <> 0) 
			rollback transaction @TranName;
	end catch

end
GO
USE [master]
GO
ALTER DATABASE [exam] SET  READ_WRITE 
GO
