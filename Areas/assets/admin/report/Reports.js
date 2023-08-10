var page = 1;
var isLoadingData = false;
var isFull = false;
(function DataTable() {

    ShowTable(page)
    $('.table-responsive').scroll(function () {
        /* var element = $(this)[0];*/
        var scrollTop = $(this).scrollTop() + 3;
        var scrollHeight = $(this)[0].scrollHeight;
        var windowHeight = $(this).outerHeight();

        if (scrollTop + windowHeight >= scrollHeight && !isLoadingData && !isFull) {

            // Đạt đến cuối trang và không đang lấy dữ liệu
            $('.spinner').show();
            // Gọi hàm để lấy dữ liệu tiếp theo
            ScrollData(page);
        }
    });
    InitSearch()
})()

function ShowTable(pagenumber,from, to) {

    $.ajax({
        type: "GET",
        url: "/Report/DataTable",
        data: {
            page: pagenumber,
          
            from,
            to
        },

        datatype: 'json',
        success: function (data) {
            let $table = $('#reporttable')
            let body = $table.find('tbody')
            isFull = false
            //Hiển thị dữ liệu trong bảng

            if (data.data && data.data.length > 0) {
                $.each(data.data, function (i, v) {
                    /*  <td>${v.MemberCardID}</td>*/
                    let tr = `<tr>
                            <th scope="row">${((10 * (data.pageCurrent - 1)) + (i + 1))}</th>
                        
                            <td>${formatDate(v.Date)}</td>
                            
                            <td>${Number(v.Shift1).toLocaleString()}</td>
                        
                            <td>${Number(v.Shift2).toLocaleString()}</td>
                            <td>${Number(v.Total).toLocaleString()}</td>
               
                         
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
            $('#QtyNote').text(`Hiển thị ${data.to} trên ${data.total} dữ liệu`)
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
                url: "/MemberCard/Delete",
                data: {
                    id
                },

                datatype: 'json',
                success: function (data) {
                    if (data.status == 'success') {
                        toastr.success('Đã xóa thành công')
                        $('#reporttable').find('tbody').empty()
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

    //$('#kt_datatable_search_query').keypress(function (e) {
    //    if (e.which == 13) {
                
    //        SearchData()
    //    }
    //})

    setTimeout(function () {
        $('#kt_datatable_from_query, #kt_datatable_to_query').on('change', function (e) {

            SearchData()
        });
    }, 300)

}
function SearchData() {
    let from = $('#kt_datatable_from_query').val().trim()
    let to = $('#kt_datatable_to_query').val().trim()
   
    $('#reporttable').find('tbody').empty()
    ShowTable(1, from, to,"2","2")
}
function ScrollData(page) {
    let from = $('#kt_datatable_from_query').val().trim()
    let to = $('#kt_datatable_to_query').val().trim()
   

 
    ShowTable(1, from, to)

  
}