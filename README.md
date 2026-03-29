# PSM_Agent

### SecureTech CLR Command Procedure

This project provides a **SQL Server CLR stored procedure** that acts as a secure bridge between SQL Server and the Shaiya game server.  

It enables administrators to send server commands directly from SQL while enforcing strict **validation, logging, and security controls**.

---

## Project Structure

### StoredProcedures.cs
Entry point for SQL Server.  
- Exposes `ExecuteCommand` as a SQL CLR procedure  
- Receives parameters from SQL  
- Calls the handler  
- Sends output back via `SqlPipe`

### CommandHandler.cs
Core execution logic.  
- Validates the command  
- Enforces rate limits  
- Builds the packet  
- Sends it to `ps_game` via socket  
- Logs the result

### CommandValidator.cs
Ensures commands are safe and allowed.  
- Checks format, length, and whitelist (`/nt`, `/ntcn`, `/kick`, `/mmake`)  
- Throws exceptions for invalid input

### Logger.cs
Handles logging of command attempts.  
- Appends success or error entries to `PSM_Agent.txt` (or SQL table if configured)  
- Records timestamp, user, service, command, and error message

### PacketBuilder.cs
Constructs the binary packet to send to `ps_game`.  
- Combines header, service name, and command text  
- Produces correct byte structure

### RateLimiter.cs
Prevents flooding.  
- Reads the last log entry timestamp from `PSM_Agent.txt`  
- Blocks if a new command is issued within 1 second
