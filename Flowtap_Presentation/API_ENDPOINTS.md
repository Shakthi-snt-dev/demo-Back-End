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

## Profile (`/api/profile`)

### Get User Profile
- **Method:** `GET`
- **Endpoint:** `/api/profile`
- **Description:** Get user profile. Extracts UserAccountId from JWT token.
- **Response:** `ApiResponseDto<AppUserProfileResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Update User Profile
- **Method:** `PUT`
- **Endpoint:** `/api/profile`
- **Description:** Update user profile. Extracts UserAccountId from JWT token.
- **Request Body:** `AppUserProfileRequestDto`
- **Response:** `ApiResponseDto<AppUserProfileResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Store Settings (`/api/storesettings`)

### Get Store Settings
- **Method:** `GET`
- **Endpoint:** `/api/storesettings/stores/{storeId}`
- **Description:** Get store settings by store ID. Extracts UserAccountId from JWT token.
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<StoreSettingsDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Update Store Settings
- **Method:** `PUT`
- **Endpoint:** `/api/storesettings/stores/{storeId}`
- **Description:** Update store settings by store ID. Extracts UserAccountId from JWT token.
- **Path Parameters:** `storeId` (Guid)
- **Request Body:** `UpdateStoreSettingsRequestDto`
- **Response:** `ApiResponseDto<StoreSettingsDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Reset API Key
- **Method:** `POST`
- **Endpoint:** `/api/storesettings/stores/{storeId}/reset-api-key`
- **Description:** Reset store API key by store ID. Extracts UserAccountId from JWT token.
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<string>`
- **Authorization:** Requires `AppUserOrOwner` role

### Send Company Email Verification
- **Method:** `POST`
- **Endpoint:** `/api/storesettings/stores/{storeId}/send-verification-email`
- **Description:** Send company email verification. Extracts UserAccountId from JWT token.
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Verify Company Email
- **Method:** `POST`
- **Endpoint:** `/api/storesettings/stores/{storeId}/verify-email?verificationToken={token}`
- **Description:** Verify company email. Extracts UserAccountId from JWT token.
- **Path Parameters:** `storeId` (Guid)
- **Query Parameters:** `verificationToken` (string)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Employees (`/api/employees`)

### Get Employees by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/employees/store/{storeId}?role={role}`
- **Description:** Get employees by store ID with optional role filter. Role parameter: "all" (default) or specific role (e.g., "Owner", "Manager", "Technician")
- **Path Parameters:** `storeId` (Guid)
- **Query Parameters:** `role` (string?, optional, default: "all")
- **Response:** `ApiResponseDto<IEnumerable<EmployeeResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

### Get Employee by ID
- **Method:** `GET`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Get employee by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Create Employee
- **Method:** `POST`
- **Endpoint:** `/api/employees`
- **Description:** Create a new employee for a store (StoreId required in request body)
- **Request Body:** `CreateEmployeeRequestDto` (must include StoreId)
- **Response:** `ApiResponseDto<EmployeeResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Update Employee
- **Method:** `PUT`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Update employee
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateEmployeeRequestDto`
- **Response:** `ApiResponseDto<EmployeeResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Delete Employee
- **Method:** `DELETE`
- **Endpoint:** `/api/employees/{id}`
- **Description:** Delete employee
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Stores (`/api/stores`)

### Get Stores List (ID and Name only)
- **Method:** `GET`
- **Endpoint:** `/api/stores/list`
- **Description:** Get stores for the current user (ID and Name only) - for dropdown selection. Extracts UserAccountId from JWT token.
- **Response:** `ApiResponseDto<IEnumerable<StoreListItemDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

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

## Inventory Bounded Context

### Products (`/api/products`)

#### Get Product by ID
- **Method:** `GET`
- **Endpoint:** `/api/products/{id}`
- **Description:** Get product by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<ProductResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get All Products
- **Method:** `GET`
- **Endpoint:** `/api/products`
- **Description:** Get all products
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Search Products
- **Method:** `GET`
- **Endpoint:** `/api/products/search?searchTerm={searchTerm}`
- **Description:** Search products by name, SKU, or brand
- **Query Parameters:** `searchTerm` (string)
- **Response:** `ApiResponseDto<IEnumerable<ProductResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Product
- **Method:** `POST`
- **Endpoint:** `/api/products`
- **Description:** Create a new product
- **Request Body:** `CreateProductRequestDto`
- **Response:** `ApiResponseDto<ProductResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Product
- **Method:** `PUT`
- **Endpoint:** `/api/products/{id}`
- **Description:** Update product
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateProductRequestDto`
- **Response:** `ApiResponseDto<ProductResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Product
- **Method:** `DELETE`
- **Endpoint:** `/api/products/{id}`
- **Description:** Delete product
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Inventory Items (`/api/inventoryitems`)

#### Get Inventory Item by ID
- **Method:** `GET`
- **Endpoint:** `/api/inventoryitems/{id}`
- **Description:** Get inventory item by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Inventory Items by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/inventoryitems/store/{storeId}`
- **Description:** Get all inventory items for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<InventoryItemResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Low Stock Items
- **Method:** `GET`
- **Endpoint:** `/api/inventoryitems/store/{storeId}/low-stock`
- **Description:** Get low stock items for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<InventoryItemResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Inventory Item
- **Method:** `POST`
- **Endpoint:** `/api/inventoryitems`
- **Description:** Create a new inventory item
- **Request Body:** `CreateInventoryItemRequestDto`
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Inventory Item
- **Method:** `PUT`
- **Endpoint:** `/api/inventoryitems/{id}`
- **Description:** Update inventory item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateInventoryItemRequestDto`
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Add Quantity
- **Method:** `POST`
- **Endpoint:** `/api/inventoryitems/{id}/add-quantity`
- **Description:** Add quantity to inventory item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `quantity` (int)
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Remove Quantity
- **Method:** `POST`
- **Endpoint:** `/api/inventoryitems/{id}/remove-quantity`
- **Description:** Remove quantity from inventory item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `quantity` (int)
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Reserve Quantity
- **Method:** `POST`
- **Endpoint:** `/api/inventoryitems/{id}/reserve-quantity`
- **Description:** Reserve quantity for inventory item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `quantity` (int)
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Release Reserved Quantity
- **Method:** `POST`
- **Endpoint:** `/api/inventoryitems/{id}/release-reserved-quantity`
- **Description:** Release reserved quantity from inventory item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `quantity` (int)
- **Response:** `ApiResponseDto<InventoryItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Inventory Item
- **Method:** `DELETE`
- **Endpoint:** `/api/inventoryitems/{id}`
- **Description:** Delete inventory item
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Procurement Bounded Context

### Suppliers (`/api/suppliers`)

#### Get Supplier by ID
- **Method:** `GET`
- **Endpoint:** `/api/suppliers/{id}`
- **Description:** Get supplier by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<SupplierResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get All Suppliers
- **Method:** `GET`
- **Endpoint:** `/api/suppliers`
- **Description:** Get all suppliers
- **Response:** `ApiResponseDto<IEnumerable<SupplierResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Active Suppliers
- **Method:** `GET`
- **Endpoint:** `/api/suppliers/active`
- **Description:** Get active suppliers only
- **Response:** `ApiResponseDto<IEnumerable<SupplierResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Supplier
- **Method:** `POST`
- **Endpoint:** `/api/suppliers`
- **Description:** Create a new supplier
- **Request Body:** `CreateSupplierRequestDto`
- **Response:** `ApiResponseDto<SupplierResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Supplier
- **Method:** `PUT`
- **Endpoint:** `/api/suppliers/{id}`
- **Description:** Update supplier
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateSupplierRequestDto`
- **Response:** `ApiResponseDto<SupplierResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Supplier
- **Method:** `DELETE`
- **Endpoint:** `/api/suppliers/{id}`
- **Description:** Delete supplier
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Purchase Orders (`/api/purchaseorders`)

#### Get Purchase Order by ID
- **Method:** `GET`
- **Endpoint:** `/api/purchaseorders/{id}`
- **Description:** Get purchase order by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<PurchaseOrderResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Purchase Orders by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/purchaseorders/store/{storeId}`
- **Description:** Get all purchase orders for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<PurchaseOrderResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Purchase Order
- **Method:** `POST`
- **Endpoint:** `/api/purchaseorders`
- **Description:** Create a new purchase order
- **Request Body:** `CreatePurchaseOrderRequestDto`
- **Response:** `ApiResponseDto<PurchaseOrderResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Submit Purchase Order
- **Method:** `POST`
- **Endpoint:** `/api/purchaseorders/{id}/submit`
- **Description:** Submit purchase order (change status to Submitted)
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<PurchaseOrderResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Mark Purchase Order as Received
- **Method:** `POST`
- **Endpoint:** `/api/purchaseorders/{id}/mark-received`
- **Description:** Mark purchase order as received
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<PurchaseOrderResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Cancel Purchase Order
- **Method:** `POST`
- **Endpoint:** `/api/purchaseorders/{id}/cancel`
- **Description:** Cancel purchase order
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<PurchaseOrderResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Purchase Order
- **Method:** `DELETE`
- **Endpoint:** `/api/purchaseorders/{id}`
- **Description:** Delete purchase order
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Service Bounded Context

### Device Categories (`/api/devicecategories`)

#### Get All Device Categories
- **Method:** `GET`
- **Endpoint:** `/api/devicecategories`
- **Description:** Get all device categories
- **Response:** `ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Device Category by ID
- **Method:** `GET`
- **Endpoint:** `/api/devicecategories/{id}`
- **Description:** Get device category by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<DeviceCategoryResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Device Category
- **Method:** `POST`
- **Endpoint:** `/api/devicecategories`
- **Description:** Create a new device category
- **Request Body:** `CreateDeviceCategoryRequestDto`
- **Response:** `ApiResponseDto<DeviceCategoryResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Device Category
- **Method:** `PUT`
- **Endpoint:** `/api/devicecategories/{id}`
- **Description:** Update device category
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateDeviceCategoryRequestDto`
- **Response:** `ApiResponseDto<DeviceCategoryResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Device Category
- **Method:** `DELETE`
- **Endpoint:** `/api/devicecategories/{id}`
- **Description:** Delete device category
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Device Brands (`/api/devicebrands`)

#### Get All Device Brands
- **Method:** `GET`
- **Endpoint:** `/api/devicebrands`
- **Description:** Get all device brands
- **Response:** `ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Device Brand by ID
- **Method:** `GET`
- **Endpoint:** `/api/devicebrands/{id}`
- **Description:** Get device brand by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<DeviceBrandResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Device Brands by Category ID
- **Method:** `GET`
- **Endpoint:** `/api/devicebrands/category/{categoryId}`
- **Description:** Get device brands by category ID (cascading filter)
- **Path Parameters:** `categoryId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Device Brand
- **Method:** `POST`
- **Endpoint:** `/api/devicebrands`
- **Description:** Create a new device brand
- **Request Body:** `CreateDeviceBrandRequestDto`
- **Response:** `ApiResponseDto<DeviceBrandResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Device Brand
- **Method:** `PUT`
- **Endpoint:** `/api/devicebrands/{id}`
- **Description:** Update device brand
- **Path Parameters:** `id` (Guid)
- **Request Body:** `CreateDeviceBrandRequestDto`
- **Response:** `ApiResponseDto<DeviceBrandResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Device Brand
- **Method:** `DELETE`
- **Endpoint:** `/api/devicebrands/{id}`
- **Description:** Delete device brand
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Device Models (`/api/devicemodels`)

#### Get All Device Models
- **Method:** `GET`
- **Endpoint:** `/api/devicemodels`
- **Description:** Get all device models
- **Response:** `ApiResponseDto<IEnumerable<DeviceModelResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Device Model by ID
- **Method:** `GET`
- **Endpoint:** `/api/devicemodels/{id}`
- **Description:** Get device model by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<DeviceModelResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Device Models by Brand ID
- **Method:** `GET`
- **Endpoint:** `/api/devicemodels/brand/{brandId}`
- **Description:** Get device models by brand ID (cascading filter)
- **Path Parameters:** `brandId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<DeviceModelResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Device Model
- **Method:** `POST`
- **Endpoint:** `/api/devicemodels`
- **Description:** Create a new device model
- **Request Body:** `CreateDeviceModelRequestDto`
- **Response:** `ApiResponseDto<DeviceModelResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Device Model
- **Method:** `PUT`
- **Endpoint:** `/api/devicemodels/{id}`
- **Description:** Update device model
- **Path Parameters:** `id` (Guid)
- **Request Body:** `CreateDeviceModelRequestDto`
- **Response:** `ApiResponseDto<DeviceModelResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Device Model
- **Method:** `DELETE`
- **Endpoint:** `/api/devicemodels/{id}`
- **Description:** Delete device model
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Device Hierarchy (`/api/devicehierarchy`)

#### Get Full Device Hierarchy
- **Method:** `GET`
- **Endpoint:** `/api/devicehierarchy/full-hierarchy`
- **Description:** Get full device hierarchy (Category → Brand → Model → Problems)
- **Response:** `ApiResponseDto<IEnumerable<DeviceCategoryResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Manufacturers by Category
- **Method:** `GET`
- **Endpoint:** `/api/devicehierarchy/category/{categoryId}/manufacturers`
- **Description:** Get manufacturers by category ID (cascading filter)
- **Path Parameters:** `categoryId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<DeviceBrandResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Models by Brand
- **Method:** `GET`
- **Endpoint:** `/api/devicehierarchy/brand/{brandId}/models`
- **Description:** Get device models by brand ID (cascading filter)
- **Path Parameters:** `brandId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<DeviceModelResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Ticket Creation Data
- **Method:** `GET`
- **Endpoint:** `/api/devicehierarchy/ticket-creation-data`
- **Description:** Get cascading data for ticket creation (Category → Manufacturer → Device → Problems)
- **Response:** `ApiResponseDto<object>`
- **Authorization:** Requires `AppUserOrOwner` role

### Services (`/api/services`)

#### Get Service by ID
- **Method:** `GET`
- **Endpoint:** `/api/services/{id}`
- **Description:** Get service by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<ServiceResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Services by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/services/store/{storeId}`
- **Description:** Get all services for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<ServiceResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Active Services by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/services/store/{storeId}/active`
- **Description:** Get active services for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<ServiceResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Service
- **Method:** `POST`
- **Endpoint:** `/api/services`
- **Description:** Create a new repair service
- **Request Body:** `CreateServiceRequestDto`
- **Response:** `ApiResponseDto<ServiceResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Service
- **Method:** `PUT`
- **Endpoint:** `/api/services/{id}`
- **Description:** Update service
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateServiceRequestDto`
- **Response:** `ApiResponseDto<ServiceResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Service
- **Method:** `DELETE`
- **Endpoint:** `/api/services/{id}`
- **Description:** Delete service
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Pre-Check Items (`/api/precheckitems`)

#### Get Pre-Check Item by ID
- **Method:** `GET`
- **Endpoint:** `/api/precheckitems/{id}`
- **Description:** Get pre-check item by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<PreCheckItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Pre-Check Items by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/precheckitems/store/{storeId}`
- **Description:** Get all pre-check items for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Active Pre-Check Items by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/precheckitems/store/{storeId}/active`
- **Description:** Get active pre-check items for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<PreCheckItemResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Pre-Check Item
- **Method:** `POST`
- **Endpoint:** `/api/precheckitems`
- **Description:** Create a new pre-check item
- **Request Body:** `CreatePreCheckItemRequestDto`
- **Response:** `ApiResponseDto<PreCheckItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Pre-Check Item
- **Method:** `PUT`
- **Endpoint:** `/api/precheckitems/{id}`
- **Description:** Update pre-check item
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdatePreCheckItemRequestDto`
- **Response:** `ApiResponseDto<PreCheckItemResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Pre-Check Item
- **Method:** `DELETE`
- **Endpoint:** `/api/precheckitems/{id}`
- **Description:** Delete pre-check item
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

### Special Order Parts (`/api/specialorderparts`)

#### Get Special Order Part by ID
- **Method:** `GET`
- **Endpoint:** `/api/specialorderparts/{id}`
- **Description:** Get special order part by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<SpecialOrderPartResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Special Order Parts by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/specialorderparts/store/{storeId}`
- **Description:** Get all special order parts for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Get Pending Special Order Parts by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/specialorderparts/store/{storeId}/pending`
- **Description:** Get pending special order parts for a store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<SpecialOrderPartResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Create Special Order Part
- **Method:** `POST`
- **Endpoint:** `/api/specialorderparts`
- **Description:** Create a new special order part
- **Request Body:** `CreateSpecialOrderPartRequestDto`
- **Response:** `ApiResponseDto<SpecialOrderPartResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Update Special Order Part
- **Method:** `PUT`
- **Endpoint:** `/api/specialorderparts/{id}`
- **Description:** Update special order part
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateSpecialOrderPartRequestDto`
- **Response:** `ApiResponseDto<SpecialOrderPartResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Mark Special Order Part as Received
- **Method:** `POST`
- **Endpoint:** `/api/specialorderparts/{id}/mark-received`
- **Description:** Mark special order part as received
- **Path Parameters:** `id` (Guid)
- **Request Body:** `receivedDate` (DateTime?, optional)
- **Response:** `ApiResponseDto<SpecialOrderPartResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

#### Delete Special Order Part
- **Method:** `DELETE`
- **Endpoint:** `/api/specialorderparts/{id}`
- **Description:** Delete special order part
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<bool>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Customers (`/api/customers`)

### Get Customer by ID
- **Method:** `GET`
- **Endpoint:** `/api/customers/{id}`
- **Description:** Get customer by ID
- **Path Parameters:** `id` (Guid)
- **Response:** `ApiResponseDto<CustomerResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Get All Customers
- **Method:** `GET`
- **Endpoint:** `/api/customers`
- **Description:** Get all customers for the authenticated AppUser's stores. Extracts UserAccountId from JWT token.
- **Response:** `ApiResponseDto<IEnumerable<CustomerResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

### Get Customers by Store ID
- **Method:** `GET`
- **Endpoint:** `/api/customers/store/{storeId}`
- **Description:** Get all customers for a specific store
- **Path Parameters:** `storeId` (Guid)
- **Response:** `ApiResponseDto<IEnumerable<CustomerResponseDto>>`
- **Authorization:** Requires `AppUserOrOwner` role

### Create Customer
- **Method:** `POST`
- **Endpoint:** `/api/customers`
- **Description:** Create a new customer (StoreId required in request body)
- **Request Body:** `CreateCustomerRequestDto` (must include StoreId)
- **Response:** `ApiResponseDto<CustomerResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

### Update Customer
- **Method:** `PUT`
- **Endpoint:** `/api/customers/{id}`
- **Description:** Update customer
- **Path Parameters:** `id` (Guid)
- **Request Body:** `UpdateCustomerRequestDto`
- **Response:** `ApiResponseDto<CustomerResponseDto>`
- **Authorization:** Requires `AppUserOrOwner` role

---

## Summary

**Total API Endpoints:** 100

### By Controller:
- **Auth:** 3 endpoints
- **Profile:** 2 endpoints
- **StoreSettings:** 5 endpoints
- **Employees:** 5 endpoints
- **Stores:** 1 endpoint
- **Roles:** 6 endpoints
- **Products:** 6 endpoints
- **InventoryItems:** 10 endpoints
- **Suppliers:** 6 endpoints
- **PurchaseOrders:** 7 endpoints
- **DeviceCategories:** 5 endpoints
- **DeviceBrands:** 6 endpoints
- **DeviceModels:** 6 endpoints
- **DeviceHierarchy:** 4 endpoints
- **Services:** 6 endpoints
- **PreCheckItems:** 6 endpoints
- **SpecialOrderParts:** 7 endpoints
- **Customers:** 5 endpoints

### By HTTP Method:
- **GET:** 55 endpoints
- **POST:** 30 endpoints
- **PUT:** 13 endpoints
- **DELETE:** 12 endpoints

### By Bounded Context:
- **Identity:** 3 endpoints (Auth)
- **Store:** 6 endpoints (StoreSettings, Stores)
- **HR:** 5 endpoints (Employees)
- **Identity:** 6 endpoints (Roles)
- **Inventory:** 16 endpoints (Products, InventoryItems)
- **Procurement:** 13 endpoints (Suppliers, PurchaseOrders)
- **Service:** 40 endpoints (DeviceCategories, DeviceBrands, DeviceModels, DeviceHierarchy, Services, PreCheckItems, SpecialOrderParts)
- **Sales:** 5 endpoints (Customers)
- **Identity:** 2 endpoints (Profile)

---

**Note:** All endpoints return responses wrapped in `ApiResponseDto<T>` format. Check Swagger UI at `/swagger` for detailed request/response schemas and to test the APIs interactively.

**Authentication:** Most endpoints require JWT authentication. Include the token in the `Authorization` header as `Bearer {token}`. Endpoints with `AppUserOrOwner` authorization require the user to be an Employee with Owner role.
