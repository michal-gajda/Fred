#!/bin/bash
echo "Starting database initialization script..."
/opt/mssql-tools/bin/sqlcmd -S sqledge -U sa -P "$MSSQL_SA_PASSWORD" -i /usr/config/setup.sql
echo "Database initialization completed successfully."
