# PSM_Agent

### SecureTech CLR Command Procedure

This project provides a **SQL Server CLR stored procedure** that acts as a secure bridge between SQL Server and the Shaiya game server.  

It enables administrators to send server commands directly from SQL while enforcing strict **validation, logging, and security controls**.

---

### Environment
- Windows  
- Visual Studio 2026  
- .NET Framework 4.8.1  
- C#  

---

## Project Structure

### AgentProcedures.cs
Entry point for SQL Server.  
- Exposes `RunServiceCommand` as a SQL CLR procedure  
- Receives parameters from SQL  
- Calls `CommandExecutor`  
- Sends output back via `SqlPipe`

### CommandExecutor.cs
Core execution logic.  
- Validates the command via `CommandRules`  
- Enforces rate limits via `RequestThrottle`  
- Builds the packet via `PacketFactory`  
- Sends it to `ps_game` via socket  
- Logs the result via `ActivityLogger`  
- Uses centralized constants (`BufferSize`, `Host`, `Port`) from `ServiceConfig`

### CommandRules.cs
Ensures commands are safe.  
- Checks format and length  
- Requires that all commands start with `/`  
- Throws exceptions for invalid input  
- Uses `ServiceConfig.MaxCommandLength` for length validation

### ActivityLogger.cs
Handles logging of command attempts.  
- Appends success or error entries to the log file  
- Records timestamp, service, command, and error message  
- Uses `ServiceConfig.LogFilePath` for centralized log location

### PacketFactory.cs
Constructs the binary packet to send to `ps_game`.  
- Uses fixed header size and marker from `ServiceConfig`  
- Combines header, service name, and command text  
- Produces correct byte structure

### RequestThrottle.cs
Prevents flooding.  
- Reads the last log entry timestamp from `ServiceConfig.LogFilePath`  
- Blocks if a new command is issued within `ServiceConfig.MinIntervalSeconds`

### SocketHelper.cs
Handles socket communication.  
- Connects to `ServiceConfig.Host:ServiceConfig.Port`  
- Sends packet data  
- Receives server response

### ServiceConfig.cs
Centralized configuration.  
- Host, Port, HeaderSize, HeaderMarker  
- MaxCommandLength  
- BufferSize  
- LogFilePath (combines directory + filename)  
- MinIntervalSeconds

### ErrorHandler.cs
Standardized error formatting.  
- Produces consistent error strings for logging and SQL output

### Utilities.cs
Helper methods.  
- Timestamp formatting  
- String checks and other small utilities
