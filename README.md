# FlowTap POS & Repair Management System - Backend API

Complete ASP.NET Core 8 backend for FlowTap POS & Repair Management System.

## 🚀 Features

- **Authentication & Authorization**: ASP.NET Identity with RSA-signed JWT tokens
- **Role-Based Access Control**: JSONB permission matrix stored in roles
- **POS System**: Sales, payments, and refunds
- **Repair Management**: Tickets, diagnostics, and device checklists
- **Inventory Management**: Products, stock tracking, and transactions
- **Customer Management**: CRM with loyalty tracking
- **Employee Management**: Roles and commission rules
- **Reports**: Sales, repairs, inventory, tax, and commissions
- **Integrations**: Shopify, WooCommerce, QuickBooks
- **Dashboard**: Redis-cached KPIs and statistics
- **Background Jobs**: Hangfire for sync tasks

## 🛠️ Tech Stack

- **Framework**: ASP.NET Core 8 (Minimal APIs)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core 8
- **Authentication**: ASP.NET Identity + RSA JWT
- **Caching**: Redis
- **Background Jobs**: Hangfire
- **Validation**: FluentValidation
- **API Docs**: Swagger
- **Logging**: Serilog + Seq

## 📋 Prerequisites

- .NET 8 SDK
- PostgreSQL 12+
- Redis (optional, for caching)
- Seq (optional, for logging)

## 🔧 Setup

1. **Clone the repository**
   ```bash
   cd back-end
   ```

2. **Update connection strings** in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=flowtap_db;Username=postgres;Password=your_password",
       "Redis": "localhost:6379"
     }
   }
   ```

3. **Restore packages and build**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Run migrations**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
   Or migrations will run automatically on startup.

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access Swagger UI**
   - Navigate to `http://localhost:5000` or `https://localhost:5001`
   - Swagger UI is available at the root path

## 🔐 Default Credentials

After seeding, you can login with:
- **Email**: admin@flowtap.com
- **Password**: Admin@123

## 📡 API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login
- `POST /api/auth/refresh` - Refresh token
- `GET /api/auth/me` - Get current user
- `PUT /api/auth/profile` - Update profile
- `PUT /api/auth/change-password` - Change password

### Dashboard
- `GET /api/dashboard/summary` - Get dashboard summary
- `GET /api/dashboard/sales-trends` - Get sales trends
- `GET /api/dashboard/repairs-stats` - Get repairs statistics

### POS
- `POST /api/pos/sale` - Create sale
- `GET /api/pos/sales` - Get sales
- `GET /api/pos/sales/{id}` - Get sale by ID
- `POST /api/pos/refund/{id}` - Process refund

### Repairs
- `POST /api/repairs/ticket` - Create repair ticket
- `GET /api/repairs/tickets` - Get repair tickets
- `GET /api/repairs/{id}` - Get repair ticket by ID
- `POST /api/repairs/diagnostics` - Create diagnostic report

### Inventory
- `GET /api/inventory/products` - Get products
- `POST /api/inventory/products` - Create product
- `PUT /api/inventory/products/{id}` - Update product
- `DELETE /api/inventory/products/{id}` - Delete product

### Customers
- `GET /api/customers` - Get customers
- `POST /api/customers` - Create customer
- `PUT /api/customers/{id}` - Update customer
- `GET /api/customers/{id}/history` - Get customer history

### Employees
- `GET /api/employees` - Get employees
- `POST /api/employees` - Create employee
- `GET /api/employees/roles` - Get roles
- `POST /api/employees/roles` - Create role
- `PUT /api/employees/roles/{id}` - Update role

### Reports
- `GET /api/reports/sales` - Sales report
- `GET /api/reports/repairs` - Repairs report
- `GET /api/reports/inventory` - Inventory report
- `GET /api/reports/tax` - Tax report
- `GET /api/reports/commissions` - Commissions report

### Integrations
- `POST /api/integrations/shopify/connect` - Connect Shopify
- `POST /api/integrations/shopify/sync` - Sync Shopify
- `POST /api/integrations/woocommerce/connect` - Connect WooCommerce
- `POST /api/integrations/quickbooks/connect` - Connect QuickBooks
- `GET /api/integrations/status` - Get integration status
- `POST /api/integrations/api-key/reset` - Reset API key

### Settings
- `GET /api/settings/store` - Get store settings
- `PUT /api/settings/store` - Update store settings
- `GET /api/settings/business` - Get business settings
- `PUT /api/settings/business` - Update business settings

## 🔑 JWT Authentication

All endpoints except `/api/auth/*` require JWT authentication. Include the token in the Authorization header:

```
Authorization: Bearer {your_jwt_token}
```

## 🗄️ Database Schema

The application uses Entity Framework Core Code-First migrations. Key entities:

- **Users** - Application users with Identity
- **Roles** - Roles with JSONB permissions
- **Stores** - Store locations
- **Sales** - Sales transactions
- **RepairTickets** - Repair job tickets
- **Products** - Inventory products
- **Customers** - Customer records
- **Employees** - Employee records
- **Integrations** - Third-party integrations

## 🔄 Background Jobs

Hangfire is configured for background job processing. Access the Hangfire dashboard at `/hangfire` (requires authentication in production).

## 📝 Notes

- RSA keys are automatically generated on first run in the `keys/` directory
- Redis caching is optional but recommended for production
- File uploads are stored locally by default (configurable for S3)
- Email and SMS services require proper configuration

## 🐛 Troubleshooting

1. **Database connection errors**: Ensure PostgreSQL is running and connection string is correct
2. **JWT errors**: Check that RSA keys are generated in the `keys/` directory
3. **Redis errors**: Redis is optional; the app will work without it but caching won't function

## 📄 License

This project is private and proprietary.

