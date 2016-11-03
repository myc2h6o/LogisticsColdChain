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
    tableNames: ['Cars', 'Cars_Status', 'Cold_Storage_Inventories', 'Cold_Storages', 'Customers', 'Orders', 'Suppliers'],
    tableDisplayNames: ['Cars', 'Cars_Status', 'Cold_Storage_Inventories', 'Cold_Storages', 'Customers', 'Orders', 'Suppliers'],

    Cars: ['License_Plate_Number', 'Tonnage', 'Load_Bearing', 'Fuel_Consumption', 'Driver', 'Phone_Number', 'Minimum_Temperature', 'Maximum_Temperature', 'Company', 'Register_Place'],
    CarsDisplay: ['License Plate Number', 'Tonnage', 'Load Bearing', 'Fuel Consumption', 'Driver', 'Phone Number', 'Minimum Temperature', 'Maximum Temperature', 'Company', 'Register Place'],
    CarsType: [PropertyType.STRING, PropertyType.NUMBER, PropertyType.NUMBER, PropertyType.NUMBER, PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER, PropertyType.NUMBER, PropertyType.STRING, PropertyType.STRING],

    Cars_Status: ['License_Plate_Number', 'Status', 'Setting_Temperature', 'Realtime_Temperature', 'Realtime_Position'],
    Cars_StatusDisplay: ['License Plate Number', 'Status', 'Setting Temperature', 'Realtime Temperature', 'Realtime Position'],
    Cars_StatusType: [PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER, PropertyType.NUMBER, PropertyType.STRING],

    Cold_Storage_Inventories: ['Number', 'Usage_Rate', 'Commodity', 'Quantity'],
    Cold_Storage_InventoriesDisplay: ['Number', 'Usage Rate', 'Commodity', 'Quantity'],
    Cold_Storage_InventoriesType: [PropertyType.STRING, PropertyType.NUMBER, PropertyType.STRING, PropertyType.NUMBER],

    Cold_Storages: ['Number', 'Address', 'Scale', 'Company'],
    Cold_StoragesDisplay: ['Number', 'Address', 'Scale', 'Company'],
    Cold_StoragesType: [PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER, PropertyType.STRING],

    Customers: ['Number', 'CName', 'Address', 'Phone_Number'],
    CustomersDisplay: ['Number', 'CName', 'Address', 'Phone Number'],
    CustomersType: [PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING],

    Orders: ['Number', 'Supplier_Number', 'Customer_Number', 'Supplier_Cold_Storage_Number', 'Customer_Cold_Storage_Number', 'Commodity', 'Quantity', 'Status', 'Car_Assigned', 'Deadline', 'Departure_Time', 'Arrival_Time'],
    OrdersDisplay: ['Number', 'Supplier Number', 'Customer Number', 'Supplier Cold Storage Number', 'Customer Cold Storage Number', 'Commodity', 'Quantity', 'Status', 'Car Assigned', 'Deadline', 'Departure Time', 'Arrival Time'],
    OrdersType: [PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.NUMBER, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING],

    Suppliers: ['Number', 'SName', 'Address', 'Phone_Number'],
    SuppliersDisplay: ['Number', 'SName', 'Address', 'Phone Number'],
    SuppliersType: [PropertyType.STRING, PropertyType.STRING, PropertyType.STRING, PropertyType.STRING]
}
