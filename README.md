# PSM_Agent

SecureTech CLR Command Procedure
This project provides a SQL Server CLR stored procedure that acts as a secure bridge between SQL Server and the Shaiya game server. It enables administrators to send server commands directly from SQL while enforcing strict validation, logging, and security controls.

Features

Input validation: Rejects empty, overly long, or malformed commands.

Whitelist security: Only allows approved commands (/nt, /ntcn, /kick, /mmake).

Rate limiting: Enforces a 1‑second delay between commands to prevent spam.

Logging: Records all activity (SUCCESS or ERROR) with timestamp, user identity, service, and command in PSM_Agent.txt.

Socket communication: Sends packets securely to the Shaiya server (127.0.0.1:40900).

Error handling: Captures and logs errors, returning clear feedback to SQL Server.

Installation
Enable CLR integration in SQL Server.

Create a database (e.g., securetech).

Set the database to TRUSTWORTHY ON.

Register the assembly (PSM_Agent.dll).

Create the stored procedure dbo.Command.

Test by executing a sample command (/nt Test message).
