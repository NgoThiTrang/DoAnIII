function statusFilter() {
    return function (input) {
        if (input == true)
            return 'Kích hoạt';
        else
            return 'Khóa';
    }
};
function lockOutUser() {
    return function (input) {
        if (input == true)
            return 'Khóa';
        else
            return 'Kích hoạt';
    }
};
function genderFilter() {
    return function (input) {
        if (input == true)
            return 'Nam';
        else
            return 'Nữ';
    }
};
function convertDatePicker() {
    return function (input) {
        if (input == null) {
            return '';
        }
        if (input.indexOf('(') < 0)
            return input;
        var dt = input.split("(")[1];
        dt = dt.split(")")[0];
        var d = new Date(parseInt(dt));
        //  return (d.getDate() > 10 ?  + "/" + d.getMonth() + 1) + "/" + d.getFullYear();
        return moment(d).format("MM/DD/YYYY");
    }
}
function convertDatetimePicker() {
    return function (input) {
        if (input == null) {
            return '';
        }
        var d = new Date(input);
        //  return (d.getDate() > 10 ?  + "/" + d.getMonth() + 1) + "/" + d.getFullYear();
        return moment(d).format("DD/MM/YYYY hh:mm");
    }
}
function filterImage() {
    return function (input) {
        if (input)
            return input.replace('~/', '/');
        else
            return '';
    }
};
angular.module('app')
    .filter('statusFilter', statusFilter)
    .filter('lockOutUser', lockOutUser)
    .filter('genderFilter', genderFilter)
    .filter('convertDatePicker', convertDatePicker)
    .filter('convertDatetimePicker', convertDatetimePicker)
    .filter('filterImage', filterImage);
