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

### CommandRules.cs
Ensures commands are safe and allowed.  
- Checks format, length, and whitelist (`/nt`, `/ntcn`, `/kick`, `/mmake`, plus any added)  
- Throws exceptions for invalid input

### ActivityLogger.cs
Handles logging of command attempts.  
- Appends success or error entries to `PSM_Agent.txt`  
- Records timestamp, user, service, command, and error message

### PacketFactory.cs
Constructs the binary packet to send to `ps_game`.  
- Uses fixed 258‑byte header (protocol requirement)  
- Combines header, service name, and command text  
- Produces correct byte structure

### RequestThrottle.cs
Prevents flooding.  
- Reads the last log entry timestamp from `PSM_Agent.txt`  
- Blocks if a new command is issued within 1 second
