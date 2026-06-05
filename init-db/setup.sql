IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Client')
BEGIN
    CREATE DATABASE Client;
END
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Server')
BEGIN
    CREATE DATABASE Server;
END
GO
