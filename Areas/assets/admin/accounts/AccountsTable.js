var page = 1;
var isLoadingData = false;
var isFull = false;
(function DataTable() {
   
    ShowTable(page)
    $('.table-responsive').scroll(function () {
       /* var element = $(this)[0];*/
        var scrollTop = $(this).scrollTop()+3;
        var scrollHeight = $(this)[0].scrollHeight;
        var windowHeight = $(this).outerHeight();

        if (scrollTop + windowHeight >= scrollHeight && !isLoadingData && !isFull) {
            // Đạt đến cuối trang và không đang lấy dữ liệu
            $('.spinner').show();
            // Gọi hàm để lấy dữ liệu tiếp theo
            ShowTable(page);
        }
    });
})()

function ShowTable(pagenumber) {
    
    $.ajax({
        type: "GET",
        url: "/AccountsAdmin/DataTable",
        data: {
            page: pagenumber
        },

        datatype: 'json',
        success: function (data) {
            let $table = $('#accounttable')
            let body = $table.find('tbody')
            isFull = false
            //Hiển thị dữ liệu trong bảng

            if (data.data && data.data.length > 0) {
                $.each(data.data, function (i, v) {
                    let tr = `<tr>
                <th scope="row">${((10 * (data.pageCurrent - 1)) + (i + 1))}</th>
                <td>${v.AccountName}</td>
                <td>${v.FullName}</td>
                <td>${formatDate(v.DateOfBirth)}</td>
                <td>${v.Email}</td>
                <td>${v.PhoneNumber}</td>
                <td></td>
                <td></td>

                </tr>`
                    body.append(tr)
                })
                pagenumber++
                page = pagenumber
                console.log(page)


            } else {
                isFull = true
            }
            $('.spinner').hide();
            isLoadingData = false;
            $('#QtyNote').text(`Displaying ${data.to} of ${data.total} records`)
        },
        error: function () {
            // Xử lý lỗi (nếu cần thiết)
            // Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
            $('.spinner').hide();
            isLoadingData = false;
        }
    })
}

