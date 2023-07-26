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
    InitSearch()
})()

function ShowTable(pagenumber, search) {

    $.ajax({
        type: "GET",
        url: "/Administrator/DataTable",
        data: {
            page: pagenumber,
            search
        },

        datatype: 'json',
        success: function (data) {
            let $table = $('#accounttable')
            let body = $table.find('tbody')

            //Hiển thị dữ liệu trong bảng

            if (data.data && data.data.length > 0) {
                $.each(data.data, function (i, v) {
                    let tr = `<tr>
                            <th scope="row">${((10 * (data.pageCurrent - 1)) + (i + 1))}</th>
                            <td>${v.UserName}</td>
                            <td>${v.Name}</td>
                            <td>${v.Status}</td>
               
                            <td nowrap="nowrap">
				            <a href="/Admin/Administrator/Edit/${v.ID}" class="btn btn-sm btn-clean btn-icon" title="Sửa">
                            <i class="la la-edit"></i>
                            </a>
                                <a href="javascript:Remove('${v.ID}')" class="btn btn-sm btn-clean btn-icon" title="Xóa">
                                    <i class="la la-trash"></i>
                                    </a>
                            </td>
                            </tr>`


                    body.append(tr)
                })
                pagenumber++
                page = pagenumber



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
function Remove(id) {

    Swal.fire({
        title: 'Bạn có muốn xóa dữ liệu này?',
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: 'Có, xóa!',

        customClass: {
            actions: 'my-actions',
            cancelButton: 'order-1 right-gap',
            confirmButton: 'order-2',
            denyButton: 'order-3',
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "Post",
                url: "/Administrator/Delete",
                data: {
                    id
                },

                datatype: 'json',
                success: function (data) {
                    if (data.status == 'success') {
                        toastr.success('Đã xóa thành công')
                        ShowTable(1)
                    }
                
                    else
                        toastr.error(data.message, 'Lỗi')
                },
                error: function () {

                }
            })
        }
    })
 
}
function InitSearch() {
    $('#kt_datatable_search_query').keypress(function (e) {
        if (e.which == 13) {
            $('#accounttable').find('tbody').empty()
            search = $('#kt_datatable_search_query').val().trim()
            ShowTable(1, search)
        }
    })
}