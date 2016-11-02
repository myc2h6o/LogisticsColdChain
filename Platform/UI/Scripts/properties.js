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
    tableNames: ['Cold_Storage', 'Cold_Storage_Inventory', 'Customer', 'Distribution', 'Refrigerator_Car', 'Supplier', 'Vehicle_Status'],
    tableDisplayNames: ['Cold Storage', 'Cold Storage Inventory', 'Customer', 'Distribution', 'Refrigerator Car', 'Supplier', 'Vehicle Status'],
    Cold_Storage: ['Cold_Storage_Number', 'Cold_Storage_Address', 'Cold_Storage_Scale', 'Cold_Storage_Company'],
    Cold_StorageDisplay: ['Number', 'Address', 'Scale', 'Company'],
    Cold_Storage_Inventory: ['Cold_Storage_Number', 'Inventory_Rate', 'Commodity', 'Quantity'],
    Cold_Storage_InventoryDisplay: ['Cold Storage Number', 'Inventory Rate', 'Commodity', 'Quantity'],
    Customer: ['Customer_Number', 'Customer1', 'Customer_Address', 'Phone_Number'],
    CustomerDisplay: ['Number', 'Customer', 'Address', 'Phone'],
    Distribution: ['Distribution_Number', 'Supplier_Number', 'Customer_Number', 'Supplier_Cold_Storage_Number', 'Customer_Cold_Storage_Number', 'Commodity', 'Quantity', 'Distribution_Status', 'Distribution_Vehicle', 'Deadline', 'Departure_Time', 'Arrival_Time'],
    DistributionDisplay: ['Number', 'Supplier Number', 'Customer Number', 'Supplier Cold Storage Number', 'Customer Cold Storage Number', 'Commodity', 'Quantity', 'Distribution Status', 'Distribution Vehicle', 'Deadline', 'Departure Time', 'Arrival Time'],
    Refrigerator_Car: ['License_Plate_Number', 'Vehicle_Type', 'Vehicle_Load', 'Fuel_Consumption', 'Driver', 'Phone_Number', 'Minimum_Temperature', 'Maximum_Temperature', 'Company', 'Car_Register'],
    Refrigerator_CarDisplay: ['License Plate', 'Type', 'Load', 'Fuel Consumption', 'Driver', 'Phone', 'Minimum Temperature', 'Maximum Temperature', 'Company', 'Car Register'],
    Supplier: ['Supplier_Number', 'Supplier1', 'Supplier_Address', 'Phone_Number'],
    SupplierDisplay: ['Number', 'Supplier', 'Address', 'Phone'],
    SupplierType: [PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER],
    Vehicle_Status: ['License_Plate_Number', 'Vehicle_Status1', 'Setting_Temperature', 'Realtime_Temperature', 'Realtime_Position'],
    Vehicle_StatusDisplay: ['License Plate', 'Status', 'Setting Temperature', 'Realtime Temperature', 'Realtime Position']
}
