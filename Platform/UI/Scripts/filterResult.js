"use strict";

var Filter = {
    endpoint: ''
}

function initEndpoint(endpoint) {
    Filter.endpoint = endpoint;
}

function filterResult() {
    var result = document.getElementById('result');
    result.innerHTML = getInnerHtml();

    function getInnerHtml() {
        var tableName = document.getElementById('selector').value
        var result = '';
        $.ajax({
            method: 'GET',
            url: Filter.endpoint + tableName,
            async: false,
            success: function (data) {
                result = getTable(tableName, data.value);
            }
        });
        return result;
    }

    function getTable(tableName, entities) {
        var result = '<table>';
        result += getTableHeader(tableName);
        result += getTableBody(tableName, entities);
        result += '</table>';
        return result;
    }

    function getTableHeader(tableName) {
        var result = '<tr>';
        $.each(Property[tableName + Property.DISPLAY], function (_, propertyDisplay) {
            result += getTh(propertyDisplay);
        });
        result += '</tr>';
        return result;
    }

    function getTableBody(tableName, entities) {
        var result = '';
        $.each(entities, function (_, entity) {
            result += '<tr>';
            $.each(Property[tableName], function (_, property) {
                result += getTd(entity[property]);
            });
            result += '</tr>';
        });
        return result;
    }

    function getTh(name) {
        return '<th>' + name + '</th>';
    }

    function getTd(name) {
        return '<td>' + name + '</td>';
    }
}