USE [master]
GO
/****** Object:  Database [WebApiDatabase]    Script Date: 2023/2/23 下午 02:37:23 ******/
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'WebApiDatabase')
CREATE DATABASE [WebApiDatabase] 
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WebApiDatabase', FILENAME = N'/var/opt/mssql/data/WebApiDatabase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WebApiDatabase_log', FILENAME = N'/var/opt/mssql/data/WebApiDatabase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WebApiDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WebApiDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WebApiDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WebApiDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WebApiDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WebApiDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WebApiDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [WebApiDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WebApiDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WebApiDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WebApiDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WebApiDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WebApiDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WebApiDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WebApiDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WebApiDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WebApiDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WebApiDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WebApiDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WebApiDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WebApiDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WebApiDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WebApiDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WebApiDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WebApiDatabase] SET RECOVERY FULL 
GO
ALTER DATABASE [WebApiDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [WebApiDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WebApiDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WebApiDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WebApiDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WebApiDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WebApiDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'WebApiDatabase', N'ON'
GO
ALTER DATABASE [WebApiDatabase] SET QUERY_STORE = OFF
GO

USE [WebApiDatabase]
GO
/****** Object:  User [webapi]    Script Date: 2023/2/24 上午 03:20:31 ******/

/* Users are typically mapped to logins, as OP's question implies, 
so make sure an appropriate login exists. */
IF NOT EXISTS(SELECT principal_id FROM sys.server_principals WHERE name = 'webapi') 
BEGIN
    /* Syntax for SQL server login.  See BOL for domain logins, etc. */
    CREATE LOGIN [webapi]
    WITH PASSWORD = '!QAZ@WSX',check_policy = off
END

/* Create the user for the specified login. */
IF NOT EXISTS(SELECT principal_id FROM sys.database_principals WHERE name = 'webapi') 
BEGIN
    CREATE USER [webapi] FOR LOGIN [webapi]
END
GO
ALTER ROLE [db_owner] ADD MEMBER [webapi]
GO
ALTER ROLE [db_datareader] ADD MEMBER [webapi]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [webapi]
GO
/****** Object:  Table [dbo].[CUSTOMER]    Script Date: 2023/2/24 上午 03:20:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CUSTOMER' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[CUSTOMER](
		[SN] [bigint] IDENTITY(1,1) NOT NULL,
		[NAME] [nvarchar](64) NOT NULL,
		[PHONE_NO] [nvarchar](64) NULL,
		[IS_DELETE] [bit] NOT NULL,
		[CREATE_DATE] [datetime] NOT NULL,
		[UPDATE_DATE] [datetime] NULL
	) ON [PRIMARY]
	END
GO
/****** Object:  Table [dbo].[HOUSE_FOR_SALE]    Script Date: 2023/2/24 上午 03:20:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='HOUSE_FOR_SALE' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[HOUSE_FOR_SALE](
		[SN] [bigint] IDENTITY(1,1) NOT NULL,
		[CUSTOMER_SN] [bigint] NOT NULL,
		[PRICE] [decimal](18, 2) NULL,
		[ADDRESS] [nvarchar](64) NULL,
		[CREATE_DATE] [datetime] NOT NULL,
		[UPDATE_DATE] [datetime] NULL,
		[SOLD_DATE] [datetime] NULL,
		[IS_DELETE] [bit] NULL,
	 CONSTRAINT [PK_HOUSE_FOR_SALE] PRIMARY KEY CLUSTERED 
	(
		[SN] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[CUSTOMER] ADD  CONSTRAINT [DF_CUSTOMER_IS_DELETE]  DEFAULT ((0)) FOR [IS_DELETE]
GO
ALTER TABLE [dbo].[CUSTOMER] ADD  CONSTRAINT [DF_CUSTOMER_CREATE_DATE]  DEFAULT (getdate()) FOR [CREATE_DATE]
GO
ALTER TABLE [dbo].[HOUSE_FOR_SALE] ADD  CONSTRAINT [DF_HOUSE_FOR_SALE_PUBLISH_DATE]  DEFAULT (getdate()) FOR [CREATE_DATE]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1表示為刪除狀態；0表示有效狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CUSTOMER', @level2type=N'COLUMN',@level2name=N'IS_DELETE'
GO