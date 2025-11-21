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

## Summary

**Total API Endpoints:** 23

### By Controller:
- **Auth:** 3 endpoints
- **Profile:** 2 endpoints
- **StoreSettings:** 5 endpoints
- **Employees:** 5 endpoints
- **Stores:** 1 endpoint
- **Roles:** 6 endpoints

### By HTTP Method:
- **GET:** 12 endpoints
- **POST:** 7 endpoints
- **PUT:** 4 endpoints
- **DELETE:** 2 endpoints

---

**Note:** All endpoints return responses wrapped in `ApiResponseDto<T>` format. Check Swagger UI at `/swagger` for detailed request/response schemas and to test the APIs interactively.

**Authentication:** Most endpoints require JWT authentication. Include the token in the `Authorization` header as `Bearer {token}`. Endpoints with `AppUserOrOwner` authorization require the user to be an Employee with Owner role.
