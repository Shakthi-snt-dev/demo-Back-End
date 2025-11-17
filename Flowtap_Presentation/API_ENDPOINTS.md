# Flowtap API Endpoints Documentation

This document lists all available API endpoints in the Flowtap Presentation layer.

**Base URL:** `https://localhost:7050` or `http://localhost:5113`

---

## Authentication (`/api/auth`)

### Register
- **Method:** `POST`
- **Endpoint:** `/api/auth/register`
- **Description:** Register a new user account
- **Request Body:** `RegisterRequestDto`
- **Response:** `ApiResponseDto<RegisterResponseDto>`

### Verify Email
- **Method:** `GET`
- **Endpoint:** `/api/auth/verify-email?token={token}`
- **Description:** Verify email address using verification token
- **Query Parameters:** `token` (string)
- **Response:** `ApiResponseDto<VerifyEmailResponseDto>`

### Login
- **Method:** `POST`
- **Endpoint:** `/api/auth/login`
- **Description:** Login with email and password
- **Request Body:** `LoginRequestDto`
- **Response:** `ApiResponseDto<LoginResponseDto>`

---

## Onboarding (`/api/onboarding`)

### Complete Step 1 - Profile Settings
- **Method:** `POST`
- **Endpoint:** `/api/onboarding/step-1/{appUserId}`
- **Description:** Complete onboarding step 1: Profile settings
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `OnboardingStep1RequestDto`
- **Response:** `ApiResponseDto<OnboardingResponseDto>`

### Complete Step 2 - Store Settings
- **Method:** `POST`
- **Endpoint:** `/api/onboarding/step-2/{appUserId}`
- **Description:** Complete onboarding step 2: Store settings
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `OnboardingStep2RequestDto`
- **Response:** `ApiResponseDto<OnboardingResponseDto>`

### Complete Step 3 - Choose Action
- **Method:** `POST`
- **Endpoint:** `/api/onboarding/step-3/{appUserId}`
- **Description:** Complete onboarding step 3: Choose action (Dive in or Demo)
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `OnboardingStep3RequestDto`
- **Response:** `ApiResponseDto<OnboardingResponseDto>`

### Upgrade to Subscription
- **Method:** `POST`
- **Endpoint:** `/api/onboarding/upgrade-subscription/{appUserId}`
- **Description:** Upgrade from trial to paid subscription
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `UpgradeToSubscriptionRequestDto`
- **Response:** `ApiResponseDto<UpgradeToSubscriptionResponseDto>`

---

## Customers (`/api/customer`)

### Create Customer
- **Method:** `POST`
- **Endpoint:** `/api/customer`
- **Description:** Create a new customer
- **Request Body:** `CreateCustomerRequestDto`
- **Response:** `ApiResponseDto<CustomerResponseDto>`

### Get Customer by ID
- **Method:** `GET`
- **Endpoint:** `/api/customer/{id}`
- **Description:** Get customer by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<CustomerResponseDto>`

### Get All Customers
- **Method:** `GET`
- **Endpoint:** `/api/customer`
- **Description:** Get all customers
- **Response:** `ApiResponseDto<IEnumerable<CustomerResponseDto>>`

### Search Customers
- **Method:** `GET`
- **Endpoint:** `/api/customer/search?term={term}`
- **Description:** Search customers
- **Query Parameters:** `term` (string, required)
- **Response:** `ApiResponseDto<IEnumerable<CustomerResponseDto>>`

### Get Customers by Status
- **Method:** `GET`
- **Endpoint:** `/api/customer/status/{status}`
- **Description:** Get customers by status
- **Path Parameters:** `status` (string)
- **Response:** `ApiResponseDto<IEnumerable<CustomerResponseDto>>`

### Update Customer
- **Method:** `PUT`
- **Endpoint:** `/api/customer/{id}`
- **Description:** Update customer
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateCustomerRequestDto`
- **Response:** `ApiResponseDto<CustomerResponseDto>`

### Mark Customer as VIP
- **Method:** `POST`
- **Endpoint:** `/api/customer/{id}/vip`
- **Description:** Mark customer as VIP
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<CustomerResponseDto>`

### Delete Customer
- **Method:** `DELETE`
- **Endpoint:** `/api/customer/{id}`
- **Description:** Delete customer
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Products (`/api/product`)

### Create Product
- **Method:** `POST`
- **Endpoint:** `/api/product`
- **Description:** Create a new product
- **Request Body:** `CreateProductRequestDto`
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Get Product by ID
- **Method:** `GET`
- **Endpoint:** `/api/product/{id}`
- **Description:** Get product by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Get Product by SKU
- **Method:** `GET`
- **Endpoint:** `/api/product/sku/{sku}`
- **Description:** Get product by SKU
- **Path Parameters:** `sku` (string)
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Get All Products
- **Method:** `GET`
- **Endpoint:** `/api/product`
- **Description:** Get all products
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`

### Get Products by Category
- **Method:** `GET`
- **Endpoint:** `/api/product/category/{category}`
- **Description:** Get products by category
- **Path Parameters:** `category` (string)
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`

### Get Low Stock Products
- **Method:** `GET`
- **Endpoint:** `/api/product/low-stock`
- **Description:** Get low stock products
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`

### Search Products
- **Method:** `GET`
- **Endpoint:** `/api/product/search?term={term}`
- **Description:** Search products
- **Query Parameters:** `term` (string, required)
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`

### Update Product
- **Method:** `PUT`
- **Endpoint:** `/api/product/{id}`
- **Description:** Update product
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateProductRequestDto`
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Update Product Stock
- **Method:** `PUT`
- **Endpoint:** `/api/product/{id}/stock`
- **Description:** Update product stock
- **Path Parameters:** `id` (Guid)
- **Request Body:** `int` (quantity)
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Add Stock to Product
- **Method:** `POST`
- **Endpoint:** `/api/product/{id}/stock`
- **Description:** Add stock to product
- **Path Parameters:** `id` (Guid)
- **Request Body:** `int` (quantity)
- **Response:** `ApiResponseDto<ProductResponseDto>`

### Delete Product
- **Method:** `DELETE`
- **Endpoint:** `/api/product/{id}`
- **Description:** Delete product
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Orders (`/api/order`)

### Create Order
- **Method:** `POST`
- **Endpoint:** `/api/order`
- **Description:** Create a new order
- **Request Body:** `CreateOrderRequestDto`
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Get Order by ID
- **Method:** `GET`
- **Endpoint:** `/api/order/{id}`
- **Description:** Get order by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Get Order by Order Number
- **Method:** `GET`
- **Endpoint:** `/api/order/number/{orderNumber}`
- **Description:** Get order by order number
- **Path Parameters:** `orderNumber` (string)
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Get All Orders
- **Method:** `GET`
- **Endpoint:** `/api/order`
- **Description:** Get all orders
- **Response:** `ApiResponseDto<IEnumerable<OrderResponseDto>>`

### Get Orders by Customer ID
- **Method:** `GET`
- **Endpoint:** `/api/order/customer/{customerId}`
- **Description:** Get orders by customer ID
- **Path Parameters:** `customerId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<OrderResponseDto>>`

### Get Orders by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/order/store/{storeId}`
- **Description:** Get orders by store ID
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<OrderResponseDto>>`

### Get Orders by Status
- **Method:** `GET`
- **Endpoint:** `/api/order/status/{status}`
- **Description:** Get orders by status
- **Path Parameters:** `status` (string)
- **Response:** `ApiResponseDto<IEnumerable<OrderResponseDto>>`

### Get Orders by Date Range
- **Method:** `GET`
- **Endpoint:** `/api/order/date-range?startDate={startDate}&endDate={endDate}`
- **Description:** Get orders by date range
- **Query Parameters:** `startDate` (DateTime), `endDate` (DateTime)
- **Response:** `ApiResponseDto<IEnumerable<OrderResponseDto>>`

### Update Order
- **Method:** `PUT`
- **Endpoint:** `/api/order/{id}`
- **Description:** Update order
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateOrderRequestDto`
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Complete Order
- **Method:** `POST`
- **Endpoint:** `/api/order/{id}/complete`
- **Description:** Complete order
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Cancel Order
- **Method:** `POST`
- **Endpoint:** `/api/order/{id}/cancel`
- **Description:** Cancel order
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<OrderResponseDto>`

### Delete Order
- **Method:** `DELETE`
- **Endpoint:** `/api/order/{id}`
- **Description:** Delete order
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Repair Tickets (`/api/repairticket`)

### Create Repair Ticket
- **Method:** `POST`
- **Endpoint:** `/api/repairticket`
- **Description:** Create a new repair ticket
- **Request Body:** `CreateRepairTicketRequestDto`
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Get Ticket by ID
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/{id}`
- **Description:** Get ticket by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Get Ticket by Ticket Number
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/number/{ticketNumber}`
- **Description:** Get ticket by ticket number
- **Path Parameters:** `ticketNumber` (string)
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Get All Tickets
- **Method:** `GET`
- **Endpoint:** `/api/repairticket`
- **Description:** Get all tickets
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Get Tickets by Status
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/status/{status}`
- **Description:** Get tickets by status
- **Path Parameters:** `status` (string)
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Get Tickets by Priority
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/priority/{priority}`
- **Description:** Get tickets by priority
- **Path Parameters:** `priority` (string)
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Get Tickets by Employee ID
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/employee/{employeeId}`
- **Description:** Get tickets by employee ID
- **Path Parameters:** `employeeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Get Overdue Tickets
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/overdue`
- **Description:** Get overdue tickets
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Search Tickets
- **Method:** `GET`
- **Endpoint:** `/api/repairticket/search?term={term}`
- **Description:** Search tickets
- **Query Parameters:** `term` (string, required)
- **Response:** `ApiResponseDto<IEnumerable<RepairTicketResponseDto>>`

### Update Ticket
- **Method:** `PUT`
- **Endpoint:** `/api/repairticket/{id}`
- **Description:** Update ticket
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateRepairTicketRequestDto`
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Update Ticket Status
- **Method:** `PUT`
- **Endpoint:** `/api/repairticket/{id}/status`
- **Description:** Update ticket status
- **Path Parameters:** `id` (Guid)
- **Request Body:** `string` (status)
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Assign Ticket to Employee
- **Method:** `POST`
- **Endpoint:** `/api/repairticket/{id}/assign/{employeeId}`
- **Description:** Assign ticket to employee
- **Path Parameters:** `id` (Guid), `employeeId` (Guid)
- **Response:** `ApiResponseDto<RepairTicketResponseDto>`

### Delete Ticket
- **Method:** `DELETE`
- **Endpoint:** `/api/repairticket/{id}`
- **Description:** Delete ticket
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Employees (`/api/employee`)

### Create Employee
- **Method:** `POST`
- **Endpoint:** `/api/employee`
- **Description:** Create a new employee
- **Request Body:** `CreateEmployeeRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Get Employee by ID
- **Method:** `GET`
- **Endpoint:** `/api/employee/{id}`
- **Description:** Get employee by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Get All Employees
- **Method:** `GET`
- **Endpoint:** `/api/employee`
- **Description:** Get all employees
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`

### Get Employees by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/employee/store/{storeId}`
- **Description:** Get employees by store ID
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`

### Get Employees by Role
- **Method:** `GET`
- **Endpoint:** `/api/employee/role/{role}`
- **Description:** Get employees by role
- **Path Parameters:** `role` (string)
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`

### Get Active Employees
- **Method:** `GET`
- **Endpoint:** `/api/employee/active`
- **Description:** Get active employees
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`

### Update Employee
- **Method:** `PUT`
- **Endpoint:** `/api/employee/{id}`
- **Description:** Update employee
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateEmployeeRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Activate Employee
- **Method:** `POST`
- **Endpoint:** `/api/employee/{id}/activate`
- **Description:** Activate employee
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Deactivate Employee
- **Method:** `POST`
- **Endpoint:** `/api/employee/{id}/deactivate`
- **Description:** Deactivate employee
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Delete Employee
- **Method:** `DELETE`
- **Endpoint:** `/api/employee/{id}`
- **Description:** Delete employee
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Employees (Plural Route) (`/api/employees`)

### Get All Employees
- **Method:** `GET`
- **Endpoint:** `/api/employees?page={page}&limit={limit}`
- **Description:** Get all employees with optional pagination
- **Query Parameters:** `page` (int?, optional), `limit` (int?, optional)
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`

### Get Employee by ID
- **Method:** `GET`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Get employee by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Create Employee
- **Method:** `POST`
- **Endpoint:** `/api/employees`
- **Description:** Create a new employee
- **Request Body:** `CreateEmployeeRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Update Employee
- **Method:** `PUT`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Update employee
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateEmployeeRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Delete Employee
- **Method:** `DELETE`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Delete employee
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

### Update Employee Role
- **Method:** `PATCH`
- **Endpoint:** `/api/employees/{id}/role`
- **Description:** Update employee role
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateEmployeeRoleRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

### Add Partner/Admin
- **Method:** `POST`
- **Endpoint:** `/api/employees/partners?ownerAppUserId={ownerAppUserId}`
- **Description:** Add a partner/admin to a store (only owner can add partners)
- **Query Parameters:** `ownerAppUserId` (Guid)
- **Request Body:** `AddPartnerRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`

---

## Integrations (`/api/integration`)

### Create QuickBooks Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/quickbooks`
- **Description:** Create QuickBooks integration
- **Request Body:** `CreateQuickBooksIntegrationRequestDto`
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Create Shopify Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/shopify`
- **Description:** Create Shopify integration
- **Request Body:** `CreateShopifyIntegrationRequestDto`
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Get Integration by ID
- **Method:** `GET`
- **Endpoint:** `/api/integration/{id}`
- **Description:** Get integration by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Get Integrations by User ID
- **Method:** `GET`
- **Endpoint:** `/api/integration/user/{appUserId}`
- **Description:** Get all integrations for user
- **Path Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<IntegrationResponseDto>>`

### Get Integration by Type
- **Method:** `GET`
- **Endpoint:** `/api/integration/user/{appUserId}/type/{type}`
- **Description:** Get integration by type
- **Path Parameters:** `appUserId` (Guid), `type` (string)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Update Integration
- **Method:** `PUT`
- **Endpoint:** `/api/integration/{id}`
- **Description:** Update integration
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateIntegrationRequestDto`
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Connect Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/{id}/connect`
- **Description:** Connect integration
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Disconnect Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/{id}/disconnect`
- **Description:** Disconnect integration
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Enable Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/{id}/enable`
- **Description:** Enable integration
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Disable Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/{id}/disable`
- **Description:** Disable integration
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<IntegrationResponseDto>`

### Sync Integration
- **Method:** `POST`
- **Endpoint:** `/api/integration/{id}/sync`
- **Description:** Sync integration
- **Path Parameters:** `id` (Guid)
- **Request Body:** `SyncIntegrationRequestDto` (optional)
- **Response:** `ApiResponseDto<SyncResultDto>`

### Delete Integration
- **Method:** `DELETE`
- **Endpoint:** `/api/integration/{id}`
- **Description:** Delete integration
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Dashboard (`/api/dashboard`)

### Get Dashboard Data
- **Method:** `GET`
- **Endpoint:** `/api/dashboard?storeId={storeId}&startDate={startDate}&endDate={endDate}`
- **Description:** Get complete dashboard data
- **Query Parameters:** `storeId` (Guid?, optional), `startDate` (DateTime?, optional), `endDate` (DateTime?, optional)
- **Response:** `ApiResponseDto<DashboardResponseDto>`

### Get Dashboard Statistics
- **Method:** `GET`
- **Endpoint:** `/api/dashboard/stats?storeId={storeId}`
- **Description:** Get dashboard statistics
- **Query Parameters:** `storeId` (Guid?, optional)
- **Response:** `ApiResponseDto<DashboardStatsResponseDto>`

### Get Sales Chart Data
- **Method:** `GET`
- **Endpoint:** `/api/dashboard/sales-chart?storeId={storeId}&days={days}`
- **Description:** Get sales chart data
- **Query Parameters:** `storeId` (Guid?, optional), `days` (int, default: 7)
- **Response:** `ApiResponseDto<List<SalesChartDataDto>>`

### Get Top Products
- **Method:** `GET`
- **Endpoint:** `/api/dashboard/top-products?storeId={storeId}&limit={limit}`
- **Description:** Get top products
- **Query Parameters:** `storeId` (Guid?, optional), `limit` (int, default: 5)
- **Response:** `ApiResponseDto<List<TopProductDto>>`

### Get Recent Orders
- **Method:** `GET`
- **Endpoint:** `/api/dashboard/recent-orders?storeId={storeId}&limit={limit}`
- **Description:** Get recent orders
- **Query Parameters:** `storeId` (Guid?, optional), `limit` (int, default: 10)
- **Response:** `ApiResponseDto<List<RecentOrderDto>>`

---

## User (`/api/user`)

### Get User Profile
- **Method:** `GET`
- **Endpoint:** `/api/user/profile?appUserId={appUserId}`
- **Description:** Get user profile
- **Query Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<UserProfileDto>`

### Update User Profile
- **Method:** `PUT`
- **Endpoint:** `/api/user/profile?appUserId={appUserId}`
- **Description:** Update user profile
- **Query Parameters:** `appUserId` (Guid)
- **Request Body:** `UpdateUserProfileRequestDto`
- **Response:** `ApiResponseDto<UserProfileDto>`

### Upload User Avatar
- **Method:** `POST`
- **Endpoint:** `/api/user/avatar?appUserId={appUserId}`
- **Description:** Upload user avatar
- **Query Parameters:** `appUserId` (Guid)
- **Request Body:** `IFormFile` (multipart/form-data)
- **Response:** `ApiResponseDto<object>`

### Change Password
- **Method:** `POST`
- **Endpoint:** `/api/user/change-password?appUserId={appUserId}`
- **Description:** Change user password
- **Query Parameters:** `appUserId` (Guid)
- **Request Body:** `UpdateSecuritySettingsRequestDto`
- **Response:** `ApiResponseDto<object>`

---

## Settings (`/api/settings`)

### Get Settings
- **Method:** `GET`
- **Endpoint:** `/api/settings/user/{appUserId}`
- **Description:** Get all settings
- **Path Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<SettingsResponseDto>`

### Update General Settings
- **Method:** `PUT`
- **Endpoint:** `/api/settings/user/{appUserId}/general`
- **Description:** Update general settings
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `UpdateGeneralSettingsRequestDto`
- **Response:** `ApiResponseDto<GeneralSettingsDto>`

### Update Inventory Settings
- **Method:** `PUT`
- **Endpoint:** `/api/settings/store/{storeId}/inventory`
- **Description:** Update inventory settings
- **Path Parameters:** `storeId` (Guid)
- **Request Body:** `UpdateInventorySettingsRequestDto`
- **Response:** `ApiResponseDto<InventorySettingsDto>`

### Update Notification Settings
- **Method:** `PUT`
- **Endpoint:** `/api/settings/user/{appUserId}/notifications`
- **Description:** Update notification settings
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `UpdateNotificationSettingsRequestDto`
- **Response:** `ApiResponseDto<NotificationSettingsDto>`

### Update Payment Settings
- **Method:** `PUT`
- **Endpoint:** `/api/settings/store/{storeId}/payment`
- **Description:** Update payment settings
- **Path Parameters:** `storeId` (Guid)
- **Request Body:** `UpdatePaymentSettingsRequestDto`
- **Response:** `ApiResponseDto<PaymentSettingsDto>`

### Update Password
- **Method:** `PUT`
- **Endpoint:** `/api/settings/user/{appUserId}/password`
- **Description:** Update password
- **Path Parameters:** `appUserId` (Guid)
- **Request Body:** `UpdateSecuritySettingsRequestDto`
- **Response:** `ApiResponseDto<object>`

### Enable Two-Factor Authentication
- **Method:** `POST`
- **Endpoint:** `/api/settings/user/{appUserId}/two-factor`
- **Description:** Enable two-factor authentication
- **Path Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<object>`

### Get Store Settings
- **Method:** `GET`
- **Endpoint:** `/api/settings/store/{storeId}`
- **Description:** Get store settings
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<StoreSettingsDto>`

### Update Store Settings
- **Method:** `PUT`
- **Endpoint:** `/api/settings/store/{storeId}`
- **Description:** Update store settings
- **Path Parameters:** `storeId` (Guid)
- **Request Body:** `UpdateStoreSettingsRequestDto`
- **Response:** `ApiResponseDto<StoreSettingsDto>`

### Reset API Key
- **Method:** `POST`
- **Endpoint:** `/api/settings/store/{storeId}/api-key/reset`
- **Description:** Reset API key for store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<object>` (contains new API key)

---

## Form Configuration (`/api/formconfiguration`)

### Get Form Configuration
- **Method:** `GET`
- **Endpoint:** `/api/formconfiguration/{formId}?appUserId={appUserId}&storeId={storeId}`
- **Description:** Get form configuration by form ID
- **Path Parameters:** `formId` (string)
- **Query Parameters:** `appUserId` (Guid?, optional), `storeId` (Guid?, optional)
- **Response:** `ApiResponseDto<FormConfigurationDto>`

### Get User Profile Form Configuration
- **Method:** `GET`
- **Endpoint:** `/api/formconfiguration/user-profile/{appUserId}`
- **Description:** Get user profile form configuration
- **Path Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<FormConfigurationDto>`

### Get Store Settings Form Configuration
- **Method:** `GET`
- **Endpoint:** `/api/formconfiguration/store-settings/{storeId}`
- **Description:** Get store settings form configuration
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<FormConfigurationDto>`

### Get Employee Form Configuration
- **Method:** `GET`
- **Endpoint:** `/api/formconfiguration/employee?storeId={storeId}`
- **Description:** Get employee form configuration
- **Query Parameters:** `storeId` (Guid?, optional)
- **Response:** `ApiResponseDto<FormConfigurationDto>`

---

## AppUserAdmin (`/api/appuseradmin`)

### Get Business Owners by AppUser ID
- **Method:** `GET`
- **Endpoint:** `/api/appuseradmin/appuser/{appUserId}`
- **Description:** Get all business owners (AppUserAdmin) for an AppUser
- **Path Parameters:** `appUserId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<AppUserAdminResponseDto>>`

### Get Business Owner by ID
- **Method:** `GET`
- **Endpoint:** `/api/appuseradmin/{id}`
- **Description:** Get business owner by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<AppUserAdminResponseDto>`

### Create Business Owner
- **Method:** `POST`
- **Endpoint:** `/api/appuseradmin?appUserId={appUserId}`
- **Description:** Create a new business owner (AppUserAdmin). Also creates an Employee record with Owner role if CreateAsEmployee is true.
- **Query Parameters:** `appUserId` (Guid)
- **Request Body:** `CreateAppUserAdminRequestDto`
- **Response:** `ApiResponseDto<AppUserAdminResponseDto>`

### Update Business Owner
- **Method:** `PUT`
- **Endpoint:** `/api/appuseradmin/{id}`
- **Description:** Update business owner
- **Path Parameters:** `id` (Guid)
- **Request Body:** `CreateAppUserAdminRequestDto`
- **Response:** `ApiResponseDto<AppUserAdminResponseDto>`

### Delete Business Owner
- **Method:** `DELETE`
- **Endpoint:** `/api/appuseradmin/{id}`
- **Description:** Delete business owner
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Stores (`/api/stores`)

### Get All Stores
- **Method:** `GET`
- **Endpoint:** `/api/stores`
- **Description:** Get all stores for the authenticated user (extracts appUserId from JWT token)
- **Response:** `ApiResponseDto<IEnumerable<StoreResponseDto>>`

### Get Store by ID
- **Method:** `GET`
- **Endpoint:** `/api/stores/{id}`
- **Description:** Get store by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<StoreResponseDto>`

### Get Stores by Type
- **Method:** `GET`
- **Endpoint:** `/api/stores/type/{storeType}`
- **Description:** Get stores by type
- **Path Parameters:** `storeType` (string)
- **Response:** `ApiResponseDto<IEnumerable<StoreResponseDto>>`

### Create Store
- **Method:** `POST`
- **Endpoint:** `/api/stores`
- **Description:** Create a new store (extracts appUserId from JWT token)
- **Request Body:** `CreateStoreRequestDto`
- **Response:** `ApiResponseDto<StoreResponseDto>`

### Update Store
- **Method:** `PUT`
- **Endpoint:** `/api/stores/{id}`
- **Description:** Update store
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateStoreRequestDto`
- **Response:** `ApiResponseDto<StoreResponseDto>`

### Delete Store
- **Method:** `DELETE`
- **Endpoint:** `/api/stores/{id}`
- **Description:** Delete store
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Store Types (`/api/storetypes`)

### Get All Store Types
- **Method:** `GET`
- **Endpoint:** `/api/storetypes`
- **Description:** Get all store types
- **Response:** `ApiResponseDto<IEnumerable<StoreTypeResponseDto>>`

### Create Store Type
- **Method:** `POST`
- **Endpoint:** `/api/storetypes`
- **Description:** Create a new store type
- **Request Body:** `CreateStoreTypeRequestDto`
- **Response:** `ApiResponseDto<StoreTypeResponseDto>`

### Delete Store Type
- **Method:** `DELETE`
- **Endpoint:** `/api/storetypes/{name}`
- **Description:** Delete store type
- **Path Parameters:** `name` (string)
- **Response:** `ApiResponseDto<object>`

---

## Roles (`/api/roles`)

### Get All Roles
- **Method:** `GET`
- **Endpoint:** `/api/roles`
- **Description:** Get all roles
- **Response:** `ApiResponseDto<IEnumerable<RoleResponseDto>>`

### Get Role by ID
- **Method:** `GET`
- **Endpoint:** `/api/roles/{id}`
- **Description:** Get role by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<RoleResponseDto>`

### Get Role by Name
- **Method:** `GET`
- **Endpoint:** `/api/roles/name/{name}`
- **Description:** Get role by name
- **Path Parameters:** `name` (string)
- **Response:** `ApiResponseDto<RoleResponseDto>`

### Create Role
- **Method:** `POST`
- **Endpoint:** `/api/roles`
- **Description:** Create a new role
- **Request Body:** `CreateRoleRequestDto`
- **Response:** `ApiResponseDto<RoleResponseDto>`

### Update Role
- **Method:** `PUT`
- **Endpoint:** `/api/roles/{id}`
- **Description:** Update role
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateRoleRequestDto`
- **Response:** `ApiResponseDto<RoleResponseDto>`

### Delete Role
- **Method:** `DELETE`
- **Endpoint:** `/api/roles/{id}`
- **Description:** Delete role
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<object>`

---

## Summary

**Total API Endpoints:** 120

### By Controller:
- **Auth:** 3 endpoints
- **Onboarding:** 4 endpoints
- **Customer:** 8 endpoints
- **Product:** 11 endpoints
- **Order:** 11 endpoints
- **RepairTicket:** 13 endpoints
- **Employee:** 10 endpoints
- **Employees (Plural):** 7 endpoints
- **Integration:** 12 endpoints
- **Dashboard:** 5 endpoints
- **User:** 4 endpoints
- **Settings:** 10 endpoints
- **FormConfiguration:** 4 endpoints
- **AppUserAdmin:** 5 endpoints
- **Stores:** 6 endpoints
- **StoreTypes:** 3 endpoints
- **Roles:** 6 endpoints

### By HTTP Method:
- **GET:** 58 endpoints
- **POST:** 33 endpoints
- **PUT:** 22 endpoints
- **PATCH:** 1 endpoint
- **DELETE:** 11 endpoints

---

**Note:** All endpoints return responses wrapped in `ApiResponseDto<T>` format. Check Swagger UI at `/swagger` for detailed request/response schemas and to test the APIs interactively.

**Authentication:** Most endpoints require JWT authentication. Include the token in the `Authorization` header as `Bearer {token}`. Some endpoints extract `appUserId` from the JWT token automatically.

