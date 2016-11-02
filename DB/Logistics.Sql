exec sp_configure 'show advanced options', 1
reconfigure
exec sp_configure 'Ad Hoc Distributed Queries', 1
reconfigure
go

--[Cars]:

CREATE TABLE [dbo].[Cars] (
   [License_Plate_Number] nvarchar(255) PRIMARY KEY not null ,
   [Tonnage] real Default (0) not null ,
   [Load_Bearing] real not null ,
   [Fuel_Consumption] int not null ,
   [Driver] nvarchar(255) not null ,
   [Phone_Number] nvarchar(255) not null ,
   [Minimum_Temperature] real Default (0) not null ,
   [Maximum_Temperature] real not null ,
   [Company] nvarchar(255) not null ,
   [Register_Place] nvarchar(255) not null 
  )
 go

--[Cars_Status]:

CREATE TABLE [dbo].[Cars_Status] (
   [License_Plate_Number] nvarchar(255) PRIMARY KEY not null ,
   [Status] nvarchar(255) not null ,
   [Setting_Temperature] real Default (0) null ,
   [Realtime_Temperature] real Default (0) null ,
   [Realtime_Position] nvarchar(255) not null 
  )
 go

--[Cold_Storage_Inventories]:

CREATE TABLE [dbo].[Cold_Storage_Inventories] (
   [Id] int PRIMARY KEY IDENTITY (1,1) not null,
   [Number] nvarchar(255) not null ,
   [Usage_Rate] real not null ,
   [Commodity] nvarchar(255) not null ,
   [Quantity] int not null 
  )
 go

--[Cold_Storages]:

CREATE TABLE [dbo].[Cold_Storages] (
   [Number] nvarchar(255) PRIMARY KEY not null ,
   [Address] nvarchar(255) not null ,
   [Scale] int Default (0) not null ,
   [Company] nvarchar(255) not null 
  )
 go

--[Customers]:

CREATE TABLE [dbo].[Customers] (
   [Number] nvarchar(255) PRIMARY KEY not null ,
   [CName] nvarchar(255) not null ,
   [Address] nvarchar(255) not null ,
   [Phone_Number] nvarchar(255) not null 
  )
 go

--[Orders]:

CREATE TABLE [dbo].[Orders] (
   [Number] nvarchar(255) PRIMARY KEY not null ,
   [Supplier_Number] nvarchar(255) not null ,
   [Customer_Number] nvarchar(255) not null ,
   [Supplier_Cold_Storage_Number] nvarchar(255) not null ,
   [Customer_Cold_Storage_Number] nvarchar(255) not null ,
   [Commodity] nvarchar(255) not null ,
   [Quantity] int Default (0) not null ,
   [Status] nvarchar(255) not null ,
   [Car_Assigned] nvarchar(255) not null ,
   [Deadline] nvarchar(255) not null ,
   [Departure_Time] nvarchar(255) not null ,
   [Arrival_Time] nvarchar(255) not null 
  )
 go

--[Suppliers]:

CREATE TABLE [dbo].[Suppliers] (
   [Number] nvarchar(255) PRIMARY KEY not null ,
   [SName] nvarchar(255) not null ,
   [Address] nvarchar(255) not null ,
   [Phone_Number] nvarchar(255) not null 
  )
 go

--[Cars]:
INSERT INTO [dbo].[Cars] ([License_Plate_Number],[Tonnage],[Load_Bearing],[Fuel_Consumption],[Driver],[Phone_Number],[Minimum_Temperature],[Maximum_Temperature],[Company],[Register_Place]) 
 SELECT [License_Plate_Number],[Tonnage],[Load_Bearing],[Fuel_Consumption],[Driver],[Phone_Number],[Minimum_Temperature],[Maximum_Temperature],[Company],[Register_Place] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cars]
 go 

--[Cars_Status]:
INSERT INTO [dbo].[Cars_Status] ([License_Plate_Number],[Status],[Setting_Temperature],[Realtime_Temperature],[Realtime_Position]) 
 SELECT [License_Plate_Number],[Status],[Setting_Temperature],[Realtime_Temperature],[Realtime_Position] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cars_Status]
 go 

--[Cold_Storage_Inventories]:
SET IDENTITY_INSERT [dbo].[Cold_Storage_Inventories] ON
 go 

INSERT INTO [dbo].[Cold_Storage_Inventories] ([Id],[Number],[Usage_Rate],[Commodity],[Quantity]) 
 SELECT [Id],[Number],[Usage_Rate],[Commodity],[Quantity] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cold_Storage_Inventories]
 go 

SET IDENTITY_INSERT [dbo].[Cold_Storage_Inventories] Off
 go 


--[Cold_Storages]:
INSERT INTO [dbo].[Cold_Storages] ([Number],[Address],[Scale],[Company]) 
 SELECT [Number],[Address],[Scale],[Company] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cold_Storages]
 go 

--[Customers]:
INSERT INTO [dbo].[Customers] ([Number],[CName],[Address],[Phone_Number]) 
 SELECT [Number],[CName],[Address],[Phone_Number] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Customers]
 go 

--[Orders]:
INSERT INTO [dbo].[Orders] ([Number],[Supplier_Number],[Customer_Number],[Supplier_Cold_Storage_Number],[Customer_Cold_Storage_Number],[Commodity],[Quantity],[Status],[Car_Assigned],[Deadline],[Departure_Time],[Arrival_Time]) 
 SELECT [Number],[Supplier_Number],[Customer_Number],[Supplier_Cold_Storage_Number],[Customer_Cold_Storage_Number],[Commodity],[Quantity],[Status],[Car_Assigned],[Deadline],[Departure_Time],[Arrival_Time] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Orders]
 go 

--[Suppliers]:
INSERT INTO [dbo].[Suppliers] ([Number],[SName],[Address],[Phone_Number]) 
 SELECT [Number],[SName],[Address],[Phone_Number] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Suppliers]
 go 

exec sp_configure 'Ad Hoc Distributed Queries', 0
reconfigure
exec sp_configure 'show advanced options', 0
reconfigure
go
