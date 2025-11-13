# Troubleshooting Guide

## PostgreSQL Connection Issues

### Error: `password authentication failed for user "postgres"`

This error occurs when:
1. PostgreSQL is not running
2. The password in `appsettings.json` is incorrect
3. The database doesn't exist
4. PostgreSQL is configured to use a different authentication method

### Solutions:

#### 1. Check if PostgreSQL is running
```powershell
# Windows - Check if PostgreSQL service is running
Get-Service -Name postgresql*

# Start PostgreSQL if not running
Start-Service postgresql-x64-14  # Adjust version number as needed
```

#### 2. Verify your PostgreSQL password
```sql
-- Connect to PostgreSQL using psql
psql -U postgres

-- Or test connection
psql -U postgres -h localhost -d postgres
```

#### 3. Update appsettings.json
Update the connection string with the correct password:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=flowtap_db;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
  }
}
```

#### 4. Create the database
```sql
-- Connect to PostgreSQL
psql -U postgres

-- Create database
CREATE DATABASE flowtap_db;

-- Exit
\q
```

#### 5. Reset PostgreSQL password (if needed)
```sql
-- Connect as superuser
psql -U postgres

-- Change password
ALTER USER postgres WITH PASSWORD 'your_new_password';

-- Update appsettings.json with new password
```

### Running Without PostgreSQL (Development Only)

The application will now start even if PostgreSQL is not available, but database features won't work. You'll see warnings in the console.

To fully use the application, you need:
1. PostgreSQL running
2. Correct connection string in `appsettings.json`
3. Database created (will be created automatically on first run)

### Quick Test

Test your connection string:
```powershell
# PowerShell
$connString = "Host=localhost;Port=5432;Database=flowtap_db;Username=postgres;Password=vel@123"
# Try connecting (requires Npgsql package or psql)
```

Or use pgAdmin or DBeaver to test the connection manually.

