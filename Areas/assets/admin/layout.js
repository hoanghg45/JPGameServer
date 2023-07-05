(function SetLayOut() {
    SetAdminUser()

})()
function SetAdminUser() {
    $.ajax({
        type: "GET",
        url: '/LoginAdmin/GetUserInfor',
        datatype: "json",
        success: function (data) {
            if (data.status == "success") {
                $('.Name').text(data.user.Name)
            } else{
                window.location.href = "/Admin/LoginAdmin"
            }
        },
        error: function () {

            window.location.href = "/Admin/LoginAdmin"
        }
    })
}
