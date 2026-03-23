/* STEP 1: Enable CLR integration */
EXEC sp_configure 'clr enabled', 1;
RECONFIGURE;
GO

/* STEP 2: Set database TRUSTWORTHY ON */
ALTER DATABASE [securetech] SET TRUSTWORTHY ON;
GO

/* STEP 3: Switch to the database (dito ka nasa loob ng securetech database) */
USE [securetech];
GO

/* STEP 4: Register the assembly (DLL file must exist at this path) */
CREATE ASSEMBLY [PSM_Agent]
AUTHORIZATION dbo
FROM 'C:\ShaiyaServer\PSM_Client\PSM_Agent.dll'
WITH PERMISSION_SET = EXTERNAL_ACCESS;
GO

/* STEP 5: Create the stored procedure linked to the assembly */
CREATE PROCEDURE [dbo].[Command]
  @serviceName NVARCHAR(4000),
  @cmmd NVARCHAR(4000)
AS EXTERNAL NAME [PSM_Agent].[StoredProcedures].[Command];
GO

/* STEP 6: Test run of the procedure */
DECLARE @return_value INT;

EXEC @return_value = dbo.Command
    @serviceName = N'ps_game',
    @cmmd = N'/nt Test message';

SELECT 'Return Value' = @return_value;
GO
