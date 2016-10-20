"use strict";

function initSelectorAndFilterLayout() {
    var selector = document.getElementById('selector');
    selector.innerHTML = getInnerHtml();
    updateFilterLayout(Property.tableNames[0]);

    function getInnerHtml(){
        var result = '';
        $.each(Property.tableNames, function (i, tableName) {
            result += getOptionItem(tableName, Property.tableDisplayNames[i]);
        });
        return result;
    }

    function getOptionItem(value, displayValue) {
        return '<option value="' + value + '">' + displayValue + '</option>';
    }
}

function updateFilterLayout(tableName) {
    var filter = document.getElementById('filter');
    filter.innerHTML = getInnerHtml(tableName);

    function getInnerHtml(tableName) {
        var properties = Property[tableName];
        var propertiesDisplay = Property[tableName + Property.DISPLAY];
        if (!properties || !propertiesDisplay) {
            return '';
        }

        var result = '';
        $.each(properties, function (i, property) {
            result += getInputHtml(tableName + property, propertiesDisplay[i]);
        });
        return result;
    }

    function getInputHtml(id, displayText) {
        return '<p>' + displayText + ':<input id="' + id + '" type="text" onkeyup="filterResult()"></input><p/>';
    }
}