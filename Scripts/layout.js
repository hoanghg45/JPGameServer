$(function CheckSession() {
    $.ajax({
        url: '/Home/CheckSession',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                $('#login').text("Thông Tin").attr("href", "/thong-tin-nguoi-dung/")
                $('#register').text("Đăng Xuất").attr("href", "").attr('name', 'logout')
                $('a[name="fullname"],input[name="fullname"],h1[name="fullname"]').text(data.user.FullName).val(data.user.FullName)
                $('a[name="email"],input[name="email"]').text(data.user.Email).val(data.user.Email)
                $('a[name="phone"],input[name="phone"]').text(data.user.Phone).val(data.user.Phone)
                $('input[name="birth"]').val(formatDate(data.user.DateOfBirth))
                $('input[name="wedding"]').val(formatDate(data.user.Wedding))
                $('div[name="avatar"]').css("background-image", "url('" + data.user.Avatar + "')")
                $('img[name="avatar"]').attr("src", data.user.Avatar)
                $('#picturefile').val(data.user.Avatar)
            } else {
               $('#login').text("Đăng Nhập").attr("href","/dang-nhap/")
                $('#register').text("Đăng Kí").attr("href", "/dang-ki/").attr('name', '')
            }
        }
    })
})

$(document).on('click', 'a[name = "logout"]', function (e) {
    e.preventDefault()
    $.ajax({
        url: '/Home/LogOut',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                window.location.href = data.url
            } else {
                
            }
        }
    })
})
function formatDate(jsonDate) {
    jsonDate = jsonDate.substr(6)
    var date = jsonTodDate(jsonDate)
    return [
        date.getFullYear(),
        padTo2Digits(date.getMonth() + 1),
        padTo2Digits(date.getDate()),
    ].join('-');
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "positionClass": "toast-top-full-width",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "show",
    "hideMethod": "slideUp"
};