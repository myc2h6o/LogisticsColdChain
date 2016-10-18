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
        var selector = document.getElementById('selector').value
        var result = '';
        $.ajax({
            method: 'GET',
            url: Filter.endpoint + selector,
            async: false,
            success: function (data) {
                result = getTable(data);
            }
        });
        return result;
    }

    function getTable(data) {
        var result = '<table>';
        result += getTableHeader();
        result += getTableBody();
        result += '</table>';
        return result;
    }

    function getTableHeader() {
        var result = '<tr>';
        result += getTh('Number');
        result += getTh('Address');
        result += getTh('Scale');
        result += getTh('Company');
        result += '</tr>';
        return result;
    }

    function getTableBody() {
    }

    function getTh(name) {
        return '<th>' + name + '</th>';
    }
}