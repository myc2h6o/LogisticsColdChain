function initSelectorAndFilterLayout() {
    var selector = document.getElementById('selector');
    selector.innerHTML = getInnerHtml();
    updateFilterLayout('Cold_Storage');

    function getInnerHtml(){
        var result = '';
        result += getOptionItem('Cold_Storage');
        result += getOptionItem('Cold_Storage_Inventory');
        result += getOptionItem('Customer');
        result += getOptionItem('Distribution');
        result += getOptionItem('Refrigerator_Car');
        result += getOptionItem('Supplier');
        result += getOptionItem('Vehicle_Status');
        return result;
    }

    function getOptionItem(value) {
        return '<option value="' + value + '">' + value + '</option>';
    }
}

function updateFilterLayout(selector) {
    var filter = document.getElementById('filter');
    filter.innerHTML = getInnerHtml(selector);

    function getInnerHtml(selector) {
        var values;
        switch (selector) {
            case 'Cold_Storage':
                values = new Array('Number', 'Address', 'Scale', 'Company');
                break;
            case 'Cold_Storage_Inventory':
                values = new Array();
                break;
            case 'Customer':
                values = new Array();
                break;
            case 'Distribution':
                values = new Array();
                break;
            case 'Refigerator_Car':
                values = new Array();
                break;
            case 'Supplier':
                values = new Array();
                break;
            case 'Vehicle_Status':
                values = new Array();
                break;
            default:
                values = new Array();
                break;
        }
        return getFilterHtml(selector, values)
    }

    function getFilterHtml(propertyName, propertyValues) {
        var result = '';
        $.each(propertyValues, function (_, value) {
            result += getInputHtml(propertyName + value, value);
        });
        return result;
    }

    function getInputHtml(id, displayText) {
        return '<p>' + displayText + ':<input id="' + id + '" type="text" onkeyup="filterResult()"></input><p/>';
    }
}