## Overview
This project includes the implementation of various features and enhancements to the existing system. The key tasks accomplished are:

1. Implemented the remaining Company controller functions, including all necessary layers down to the data access layer.
2. Converted all Company controller functions to be asynchronous.
3. Created a new repository to get and save employee information with the specified data model properties.
4. Developed an Employee controller to retrieve specific properties for the client side.
5. Added logging functionality to the solution and implemented proper error handling mechanisms.

## Implemented Features

### 1. Company Controller Functions
- Implemented the rest of the Company controller functions.
- Ensured all layers, including service and data access layers, are properly integrated.

### 2. Asynchronous Company Controller Functions
- Refactored all Company controller functions to be asynchronous, improving performance and scalability.

### 3. Employee Information Repository
- Created a new repository to manage employee information.
- Data model properties include:
  - `string SiteId`
  - `string CompanyCode`
  - `string EmployeeCode`
  - `string EmployeeName`
  - `string Occupation`
  - `string EmployeeStatus`
  - `string EmailAddress`
  - `string Phone`
  - `DateTime LastModified`

### 4. Employee Controller
- Developed an Employee controller to retrieve and provide the following properties for the client side:
  - `string EmployeeCode`
  - `string EmployeeName`
  - `string CompanyName`
  - `string OccupationName`
  - `string EmployeeStatus`
  - `string EmailAddress`
  - `string PhoneNumber`
  - `string LastModifiedDateTime`

### 5. Logging and Error Handling
- Integrated a logging mechanism to capture and log important events and errors. We are logging logs in a file outside and not just console so that they are safe.
- Implemented proper error handling throughout the solution to ensure robustness and maintainability.

