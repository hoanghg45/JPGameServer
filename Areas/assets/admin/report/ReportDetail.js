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

function ShowTable(pagenumber, from, to, shift, cashier, type, paytype) {

    $.ajax({
        type: "GET",
        url: "/Report/DataDetailReport",
        data: {
            page: pagenumber,

            from,
            to,
            shift, cashier, type, paytype
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
                            <td>${v.RecordID}</td>
                            <td>${dateTimeFormat(v.Date)}</td>
                            
                            <td>${v.Shift}</td>
                            <td>${v.Cashier}</td>
                            <td>${Number(v.Money).toLocaleString()}</td>
                            <td>${v.Typepay}</td>
                            <td>${v.RecordType}</td>
                            <td>${v.PromotionDes}</td>
                        
               
                         
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

function InitSearch() {

    //$('#kt_datatable_search_query').keypress(function (e) {
    //    if (e.which == 13) {

    //        SearchData()
    //    }
    //})
    
    setTimeout(function () {
        $('#kt_datatable_from_query, #kt_datatable_to_query, #kt_datatable_shift_query, #kt_datatable_cashier_query, #kt_datatable_type_query, #kt_datatable_paytype_query').on('change', function (e) {

            SearchData()
        });
    }, 300)

}
function SearchData() {
    let from = $('#kt_datatable_from_query').val().trim()
    let to = $('#kt_datatable_to_query').val().trim()
    let shift = $('#kt_datatable_shift_query').val().trim()
    let cashier = $('#kt_datatable_cashier_query').val().trim()
    let type = $('#kt_datatable_type_query').val().trim()
    let paytype = $('#kt_datatable_paytype_query').val().trim()

    $('#reporttable').find('tbody').empty()
    ShowTable(1, from, to, shift, cashier, type, paytype)
}
function ScrollData(page) {
    let from = $('#kt_datatable_from_query').val().trim()
    let to = $('#kt_datatable_to_query').val().trim()
    let shift = $('#kt_datatable_shift_query').val().trim()
    let cashier = $('#kt_datatable_cashier_query').val().trim()
    let type = $('#kt_datatable_type_query').val().trim()
    let paytype = $('#kt_datatable_paytype_query').val().trim()


    ShowTable(page, from, to, shift, cashier, type, paytype)


}
function excel() {
   
    let from = $('#kt_datatable_from_query').val().trim()
    let to = $('#kt_datatable_to_query').val().trim()
    let shift = $('#kt_datatable_shift_query').val().trim()
    let cashier = $('#kt_datatable_cashier_query').val().trim()
    let type = $('#kt_datatable_type_query').val().trim()
    let paytype = $('#kt_datatable_paytype_query').val().trim()

    $.ajax({
        type: "GET",
        url: "/Admin/Report/ExportDetailReport",
        data: {
            from,
            to,
            shift,
            cashier,
            type,
            paytype
        },

        datatype: 'json',
        success: function (data) {
            window.location.href = `/Admin/Report/ExportDetailReport?from=${from}&to=${to}&shift=${shift}&cashier=${cashier}&type=${type}&paytype=${paytype}`
        },
        error: function () {
            // Xử lý lỗi (nếu cần thiết)
            // Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
           
        }
    })
}