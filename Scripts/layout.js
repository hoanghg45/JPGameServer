$(function CheckSession() {
    $.ajax({
        url: '/Home/CheckSession',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                $('#login').text("Thông Tin Của " + data.user.FullName).attr("href", "/thong-tin-nguoi-dung/")
                $('#register').text("Đăng Xuất").attr("href", "").attr('name', 'logout')
                $('a[name="name_user"],input[name = "name_user"]').text(data.user.FullName).val(data.user.FullName)
                $('a[name="email"],input[name="email"]').text(data.user.Email).val(data.user.Email)
                $('a[name="phone"],input[name="phone"]').text(data.user.Phone).val(data.user.Phone)
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