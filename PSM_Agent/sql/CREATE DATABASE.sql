/* STEP 1: Create a database named securetech */
CREATE DATABASE securetech
ON PRIMARY (
    NAME = securetech_data,
    FILENAME = 'C:\SQLData\securetech_data.mdf',
    SIZE = 10MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 5MB
)
LOG ON (
    NAME = securetech_log,
    FILENAME = 'C:\SQLData\securetech_log.ldf',
    SIZE = 5MB,
    MAXSIZE = 25MB,
    FILEGROWTH = 5MB
);

/* STEP 2: Enable CLR integration */
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
GO

/* STEP 3: Set database TRUSTWORTHY ON */
ALTER DATABASE [securetech] SET TRUSTWORTHY ON;
GO

/* STEP 4: Switch to the database */
USE [securetech];
GO

/* STEP 5: Register the assembly (DLL file must exist at this path) */
CREATE ASSEMBLY [PSM_Agent]
AUTHORIZATION dbo
FROM 'C:\ShaiyaServer\PSM_Client\PSM_Agent.dll'
WITH PERMISSION_SET = EXTERNAL_ACCESS;
GO

/* STEP 6: Create the stored procedure linked to the assembly */
CREATE PROCEDURE [dbo].[Command]
  @serviceName NVARCHAR(4000),
  @cmmd NVARCHAR(4000)
AS EXTERNAL NAME [PSM_Agent].[StoredProcedures].[Command];
GO

/* STEP 7: Test run of the procedure */
DECLARE @return_value INT;

EXEC @return_value = dbo.Command
    @serviceName = N'ps_game',
    @cmmd = N'/nt Test message';

SELECT 'Return Value' = @return_value;
GO
