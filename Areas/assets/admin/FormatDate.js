
function jsonTodDate(jsonDate) {
    const backToDate = new Date(parseInt(jsonDate));
    return backToDate;
}
function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function formatDate(jsonDate) {
    if (jsonDate == null)
        return ""
    jsonDate = jsonDate.substr(6)
    var date = jsonTodDate(jsonDate)
    return [
        padTo2Digits(date.getDate()),
        padTo2Digits(date.getMonth() + 1),
        date.getFullYear(),
    ].join('/');
}


function timeCal(lastlogin) {
    lastlogin = lastlogin.substring(4, 24)

    moment.locale('vi')
    var cal = moment(lastlogin).fromNow();

    return cal
}
function getTime(jsonDate) {

    let JavaScriptDate = jsonTodDate(jsonDate)
    var dateObject = new Date(JavaScriptDate);
    return dateObject.getHours() + ':' + ((dateObject.getMinutes() < 10) ? ('0' + dateObject.getMinutes()) : dateObject.getMinutes())
}
function getStringWithDate(date) {
    var dd = String(date.getDate()).padStart(2, '0');
    var mm = String(date.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = date.getFullYear();

    return dd + '/' + mm + '/' + yyyy;
}
function getDateWithString(str) {

    const [day, month, year] = str.split('/');
    const date = new Date(+year, +month - 1, +day);
    return date

}