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

$(function Module() {
    $.ajax({
        url: '/Home/Module',
        type: 'get',
        success: function (data) {
            if (data.code == 200) {
                $('link[name="logo"]').attr("href", data.logo)
                $('img[name="logo"]').attr("src", data.logo)
            } else {
            }
        }
    })
})
$(function Game() {
    $.ajax({
        type: 'get',
        url: '/Home/Game',
        success: function (data) {
            var div = ``
            $.each(data.dataHot, function (k, v) {
                div += `<div class="tc-item">
                                <div class="tc-thumb set-bg" data-setbg="${v.Image}"style="background-image:url('${v.Image}')"></div>
                                <div class="tc-content">
                                    <p><a href="#">${v.Name}</a>  ${v.Title}</p>
                                    <div class="tc-date">${formatDate(v.ModifyDate)}</div>
                                </div>
                            </div>`
            })
            $('div[name="gamelayout"]').append(div)
        }
    })
})
$(function Blog() {
    $.ajax({
        type: 'get',
        url: '/Home/Blog',
        success: function (data) {
            var div = ``
            $.each(data.dataHot, function (k, v) {
                div += `<div class="lb-item">
                                <div class="lb-thumb set-bg" data-setbg="${v.Image}"style="background-image:url('${v.Image}')"></div>
                                <div class="lb-content">
                                    <div class="lb-date">${v.Name}</div>
                                    <p><a href="#">xem thêm</a></p>
                                    <a href="#" class="lb-author">${v.ModifyBy}</a>
                                </div>
                            </div>`
            })
            $('div[name="blogLayout"]').append(div)
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
