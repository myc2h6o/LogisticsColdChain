exec sp_configure 'show advanced options', 1
reconfigure
exec sp_configure 'Ad Hoc Distributed Queries', 1
reconfigure
go

--[Cold_Storage]:

CREATE TABLE [dbo].[Cold_Storage] (
   [Cold_Storage_Number] nvarchar(255) not null ,
   [Cold_Storage_Address] nvarchar(255) null ,
   [Cold_Storage_Scale] nvarchar(255) null ,
   [Cold_Storage_Company] nvarchar(255) null 
  ) ON [Primary]
 go

--[Cold_Storage_Inventory]:

CREATE TABLE [dbo].[Cold_Storage_Inventory] (
   [Cold_Storage_Number] nvarchar(255) not null ,
   [Inventory_Rate] float null ,
   [Commodity] nvarchar(255) null ,
   [Quantity] integer null 
  ) ON [Primary]
 go

--[Customer]:

CREATE TABLE [dbo].[Customer] (
   [Customer_Number] nvarchar(255) not null ,
   [Customer] nvarchar(255) null ,
   [Customer_Address] nvarchar(255) null ,
   [Phone_Number] float null 
  ) ON [Primary]
 go

--[Distribution]:

CREATE TABLE [dbo].[Distribution] (
   [Distribution_Number] nvarchar(255) not null ,
   [Supplier_Number] nvarchar(255) null ,
   [Customer_Number] nvarchar(255) null ,
   [Supplier_Cold_Storage_Number] nvarchar(255) null ,
   [Customer_Cold_Storage_Number] nvarchar(255) null ,
   [Commodity] nvarchar(255) null ,
   [Quantity] nvarchar(255) null ,
   [Distribution_Status] nvarchar(255) null ,
   [Distribution_Vehicle] nvarchar(255) null ,
   [Deadline] nvarchar(255) null ,
   [Departure_Time] nvarchar(255) null ,
   [Arrival_Time] nvarchar(255) null 
  ) ON [Primary]
 go

--[Refrigerator_Car]:

CREATE TABLE [dbo].[Refrigerator_Car] (
   [License_Plate_Number] nvarchar(255) not null ,
   [Vehicle_Type] nvarchar(255) null ,
   [Vehicle_Load] float null ,
   [Fuel_Consumption] integer null ,
   [Driver] nvarchar(255) null ,
   [Phone_Number] float null ,
   [Minimum_Temperature] nvarchar(255) null ,
   [Maximum_Temperature] integer null ,
   [Company] nvarchar(255) null ,
   [Car_Register] nvarchar(255) null 
  ) ON [Primary]
 go

--[Supplier]:

CREATE TABLE [dbo].[Supplier] (
   [Supplier_Number] nvarchar(255) not null ,
   [Supplier] nvarchar(255) null ,
   [Supplier_Address] nvarchar(255) null ,
   [Phone_Number] float null 
  ) ON [Primary]
 go

--[Vehicle_Status]:

CREATE TABLE [dbo].[Vehicle_Status] (
   [License_Plate_Number] nvarchar(255) not null ,
   [Vehicle_Status] nvarchar(255) null ,
   [Setting_Temperature] nvarchar(255) null ,
   [Real-time_Temperature] nvarchar(255) null ,
   [Real-time_Position] nvarchar(255) null 
  ) ON [Primary]
 go


--[Cold_Storage]:
INSERT INTO [dbo].[Cold_Storage] ([Cold_Storage_Number],[Cold_Storage_Address],[Cold_Storage_Scale],[Cold_Storage_Company]) 
 SELECT [Cold_Storage_Number],[Cold_Storage_Address],[Cold_Storage_Scale],[Cold_Storage_Company] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cold_Storage]
 go 

--[Cold_Storage_Inventory]:
INSERT INTO [dbo].[Cold_Storage_Inventory] ([Cold_Storage_Number],[Inventory_Rate],[Commodity],[Quantity]) 
 SELECT [Cold_Storage_Number],[Inventory_Rate],[Commodity],[Quantity] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Cold_Storage_Inventory]
 go 

--[Customer]:
INSERT INTO [dbo].[Customer] ([Customer_Number],[Customer],[Customer_Address],[Phone_Number]) 
 SELECT [Customer_Number],[Customer],[Customer_Address],[Phone_Number] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Customer]
 go 

--[Distribution]:
INSERT INTO [dbo].[Distribution] ([Distribution_Number],[Supplier_Number],[Customer_Number],[Supplier_Cold_Storage_Number],[Customer_Cold_Storage_Number],[Commodity],[Quantity],[Distribution_Status],[Distribution_Vehicle],[Deadline],[Departure_Time],[Arrival_Time]) 
 SELECT [Distribution_Number],[Supplier_Number],[Customer_Number],[Supplier_Cold_Storage_Number],[Customer_Cold_Storage_Number],[Commodity],[Quantity],[Distribution_Status],[Distribution_Vehicle],[Deadline],[Departure_Time],[Arrival_Time] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Distribution]
 go 

--[Refrigerator_Car]:
INSERT INTO [dbo].[Refrigerator_Car] ([License_Plate_Number],[Vehicle_Type],[Vehicle_Load],[Fuel_Consumption],[Driver],[Phone_Number],[Minimum_Temperature],[Maximum_Temperature],[Company],[Car_Register]) 
 SELECT [License_Plate_Number],[Vehicle_Type],[Vehicle_Load],[Fuel_Consumption],[Driver],[Phone_Number],[Minimum_Temperature],[Maximum_Temperature],[Company],[Car_Register] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Refrigerator_Car]
 go 

--[Supplier]:
INSERT INTO [dbo].[Supplier] ([Supplier_Number],[Supplier],[Supplier_Address],[Phone_Number]) 
 SELECT [Supplier_Number],[Supplier],[Supplier_Address],[Phone_Number] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Supplier]
 go 

--[Vehicle_Status]:
INSERT INTO [dbo].[Vehicle_Status] ([License_Plate_Number],[Vehicle_Status],[Setting_Temperature],[Real-time_Temperature],[Real-time_Position]) 
 SELECT [License_Plate_Number],[Vehicle_Status],[Setting_Temperature],[Real-time_Temperature],[Real-time_Position] 
 FROM OPENDATASOURCE('Microsoft.ACE.OLEDB.12.0','Data Source="[FOLDER_PATH]\Logistics.mdb"')...[Vehicle_Status]
 go 

 
exec sp_configure 'Ad Hoc Distributed Queries', 0
reconfigure
exec sp_configure 'show advanced options', 0
reconfigure
go
