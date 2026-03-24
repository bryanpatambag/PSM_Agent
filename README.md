# PSM_Agent

## SecureTech CLR Command Procedure

This project provides a SQL Server CLR stored procedure that acts as a secure bridge between SQL Server and the Shaiya game server.  
It enables administrators to send server commands directly from SQL while enforcing strict validation, logging, and security controls.

---

## Features

- **Input validation**: Rejects empty, overly long, or malformed commands.  
- **Whitelist security**: Only allows approved commands (`/nt`, `/ntcn`, `/kick`, `/mmake`).  
- **Rate limiting**: Enforces a 1‑second delay between commands to prevent spam.  
- **Logging**: Records all activity (`SUCCESS` or `ERROR`) with timestamp, user identity, service, and command in `PSM_Agent.txt`.  
- **Socket communication**: Sends packets securely to the Shaiya server (`127.0.0.1:40900`).  
- **Error handling**: Captures and logs errors, returning clear feedback to SQL Server.  

---
## Demo Video

[![Watch the video](https://img.youtube.com/vi/xLphPV79nss/maxresdefault.jpg)](https://youtu.be/xLphPV79nss)

## Installation

```sql
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
