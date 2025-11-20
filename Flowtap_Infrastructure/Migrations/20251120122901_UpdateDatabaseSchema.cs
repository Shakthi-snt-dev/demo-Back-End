using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flowtap_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Data = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StreetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    BusinessName = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Currency = table.Column<string>(type: "text", nullable: false, defaultValue: "USD"),
                    TimeZone = table.Column<string>(type: "text", nullable: false, defaultValue: "UTC"),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SubscriptionId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrialStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialStatus = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StoreIds = table.Column<string>(type: "text", nullable: false),
                    DefaultStoreId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StreetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    TotalOrders = table.Column<int>(type: "integer", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "active"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Role = table.Column<string>(type: "text", nullable: true),
                    AccessPinHash = table.Column<string>(type: "text", nullable: true),
                    LinkedAppUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CanSwitchRole = table.Column<bool>(type: "boolean", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StreetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EmployeeCode = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "text", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "text", nullable: true),
                    HourlyRate = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Integrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Connected = table.Column<bool>(type: "boolean", nullable: false),
                    ConnectedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSyncAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SettingsJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Tax = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    MinStock = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CustomerPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    Device = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Issue = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "pending"),
                    Priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "medium"),
                    EstimatedCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepositPaid = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedToEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StoreType = table.Column<string>(type: "text", nullable: true),
                    StoreCategory = table.Column<string>(type: "text", nullable: true),
                    StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StreetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    EmployeeIds = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MaxStores = table.Column<int>(type: "integer", nullable: false),
                    PricePerMonth = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BillingInterval = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PaymentProvider = table.Column<string>(type: "text", nullable: true),
                    ExternalSubscriptionId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    PasswordSalt = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    EmailVerificationToken = table.Column<string>(type: "text", nullable: true),
                    EmailVerificationTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExternalProvider = table.Column<string>(type: "text", nullable: true),
                    ExternalProviderId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreEmail = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    AlternateName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StoreLogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EnablePOS = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    EnableInventory = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "UTC"),
                    TimeFormat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "12h"),
                    Language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "en"),
                    DefaultCurrency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "USD"),
                    PriceFormat = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "$0.00"),
                    DecimalFormat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "2"),
                    ChargeSalesTax = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultTaxClass = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TaxPercentage = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartTime = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    EndTime = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    DefaultAddress_StreetNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DefaultAddress_StreetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DefaultAddress_City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DefaultAddress_State = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DefaultAddress_PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ApiKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ApiKeyCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AccountingMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Cash Basis"),
                    CompanyEmail = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: true),
                    CompanyEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    RequireTwoFactorForAllUsers = table.Column<bool>(type: "boolean", nullable: false),
                    ChargeRestockingFee = table.Column<bool>(type: "boolean", nullable: false),
                    DiagnosticBenchFee = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    ChargeDepositOnRepairs = table.Column<bool>(type: "boolean", nullable: false),
                    LockScreenTimeoutMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 15),
                    BusinessHoursJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreSettings_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_AppUserId",
                table: "ActivityLogs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EmployeeId",
                table: "ActivityLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_StoreId",
                table: "ActivityLogs",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Email",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Phone",
                table: "Customers",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Status",
                table: "Customers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StoreId",
                table: "Customers",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StoreId_Email",
                table: "Customers",
                columns: new[] { "StoreId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true,
                filter: "\"EmployeeCode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StoreId",
                table: "Employees",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StoreId_Email",
                table: "Employees",
                columns: new[] { "StoreId", "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StoreId_IsActive",
                table: "Employees",
                columns: new[] { "StoreId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StoreId_Role",
                table: "Employees",
                columns: new[] { "StoreId", "Role" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_AppUserId",
                table: "Integrations",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_AppUserId_Type",
                table: "Integrations",
                columns: new[] { "AppUserId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_CreatedAt",
                table: "Orders",
                columns: new[] { "CustomerId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId",
                table: "Orders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId_CreatedAt",
                table: "Orders",
                columns: new[] { "StoreId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StoreId_Status",
                table: "Orders",
                columns: new[] { "StoreId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId_Category",
                table: "Products",
                columns: new[] { "StoreId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId_SKU",
                table: "Products",
                columns: new[] { "StoreId", "SKU" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_AssignedToEmployeeId",
                table: "RepairTickets",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_CreatedDate",
                table: "RepairTickets",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_CustomerId",
                table: "RepairTickets",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_DueDate",
                table: "RepairTickets",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_Priority",
                table: "RepairTickets",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_Status",
                table: "RepairTickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_StoreId",
                table: "RepairTickets",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_StoreId_AssignedToEmployeeId",
                table: "RepairTickets",
                columns: new[] { "StoreId", "AssignedToEmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_StoreId_Status",
                table: "RepairTickets",
                columns: new[] { "StoreId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_TicketNumber",
                table: "RepairTickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreSettings_StoreId",
                table: "StoreSettings",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ExternalSubscriptionId",
                table: "Subscriptions",
                column: "ExternalSubscriptionId",
                unique: true,
                filter: "\"ExternalSubscriptionId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_EmailVerificationToken",
                table: "UserAccounts",
                column: "EmailVerificationToken",
                unique: true,
                filter: "\"EmailVerificationToken\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Username",
                table: "UserAccounts",
                column: "Username",
                unique: true,
                filter: "\"Username\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Integrations");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "RepairTickets");

            migrationBuilder.DropTable(
                name: "StoreSettings");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
