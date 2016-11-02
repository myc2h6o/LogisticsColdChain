"use strict";

var PropertyType = {
    NUMBER: 0,
    STRING: 1
}

var Property = {
    //use (tableName+Property) for id
    //use (tableName+property + Min) and (tableName+property + Max) for number id
    DISPLAY: 'Display',
    TYPE: "Type",
    NumberMin: "Min",
    NumberMax: "Max",
    tableNames: new Array('Cold_Storage', 'Cold_Storage_Inventory', 'Customer', 'Distribution', 'Refrigerator_Car', 'Supplier', 'Vehicle_Status'),
    tableDisplayNames: new Array('Cold Storage', 'Cold Storage Inventory', 'Customer', 'Distribution', 'Refrigerator Car', 'Supplier', 'Vehicle Status'),
    Cold_Storage: new Array('Cold_Storage_Number', 'Cold_Storage_Address', 'Cold_Storage_Scale', 'Cold_Storage_Company'),
    Cold_StorageDisplay: new Array('Number', 'Address', 'Scale', 'Company'),
    Cold_Storage_Inventory: new Array('Cold_Storage_Number', 'Inventory_Rate', 'Commodity', 'Quantity'),
    Cold_Storage_InventoryDisplay: new Array('Cold Storage Number', 'Inventory Rate', 'Commodity', 'Quantity'),
    Customer: new Array('Customer_Number', 'Customer1', 'Customer_Address', 'Phone_Number'),
    CustomerDisplay: new Array('Number', 'Customer', 'Address', 'Phone'),
    Distribution: new Array('Distribution_Number', 'Supplier_Number', 'Customer_Number', 'Supplier_Cold_Storage_Number', 'Customer_Cold_Storage_Number', 'Commodity', 'Quantity', 'Distribution_Status', 'Distribution_Vehicle', 'Deadline', 'Departure_Time', 'Arrival_Time'),
    DistributionDisplay: new Array('Number', 'Supplier Number', 'Customer Number', 'Supplier Cold Storage Number', 'Customer Cold Storage Number', 'Commodity', 'Quantity', 'Distribution Status', 'Distribution Vehicle', 'Deadline', 'Departure Time', 'Arrival Time'),
    Refrigerator_Car: new Array('License_Plate_Number', 'Vehicle_Type', 'Vehicle_Load', 'Fuel_Consumption', 'Driver', 'Phone_Number', 'Minimum_Temperature', 'Maximum_Temperature', 'Company', 'Car_Register'),
    Refrigerator_CarDisplay: new Array('License Plate', 'Type', 'Load', 'Fuel Consumption', 'Driver', 'Phone', 'Minimum Temperature', 'Maximum Temperature', 'Company', 'Car Register'),
    Supplier: new Array('Supplier_Number', 'Supplier1', 'Supplier_Address', 'Phone_Number'),
    SupplierDisplay: new Array('Number', 'Supplier', 'Address', 'Phone'),
    SupplierType: new Array(PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER),
    Vehicle_Status: new Array('License_Plate_Number', 'Vehicle_Status1', 'Setting_Temperature', 'Realtime_Temperature', 'Realtime_Position'),
    Vehicle_StatusDisplay: new Array('License Plate', 'Status', 'Setting Temperature', 'Realtime Temperature', 'Realtime Position')
}
