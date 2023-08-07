
function jsonTodDate(jsonDate) {
    
    const backToDate = new Date(parseInt(jsonDate));
    return backToDate;
}
function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

//function formatdate(jsondate) {
//    if (jsondate == null)
//        return ""
//    jsondate = jsondate.substr(6)
//    var date = jsontoddate(jsondate)
//    return [
//        padto2digits(date.getdate()),
//        padto2digits(date.getmonth() + 1),
//        date.getfullyear(),
//    ].join('/');
//}

function formatDate(jsonDate) {
    if (jsonDate == null)
        return ""
    jsonDate = jsonDate.substr(6)
    var date = jsonTodDate(jsonDate)
    return [
        date.getFullYear(),
        padTo2Digits(date.getMonth() + 1),
        padTo2Digits(date.getDate()),
    ].join('-');
}

function timeCal(lastlogin) {
    lastlogin = lastlogin.substring(4, 24)

    moment.locale('vi')
    var cal = moment(lastlogin).fromNow();

    return cal
}
function getTime(jsonDate) {
    jsonDate = jsonDate.substr(6)
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

function dateTimeFormat(datetime) {

    datetime = parseInt(datetime.substr(6)); // Lấy giá trị milliseconds từ chuỗi

    var date = new Date(datetime); // Tạo đối tượng Date từ milliseconds

    // Định dạng ngày giờ theo định dạng "DD/MM/YYYY hh:mm A"
    var formattedDate = (date.getDate() < 10 ? '0' : '') + date.getDate() + '/' +
        ((date.getMonth() + 1) < 10 ? '0' : '') + (date.getMonth() + 1) + '/' +
        date.getFullYear() + ' ' +
        (date.getHours() < 10 ? '0' : '') + date.getHours() + ':' +
        (date.getMinutes() < 10 ? '0' : '') + date.getMinutes() + ' ' +
        (date.getHours() < 12 ? 'AM' : 'PM');

    return formattedDate

}