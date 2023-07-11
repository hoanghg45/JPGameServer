var page = 1;
var isLoadingData = false;
var isFull = false;
(function DataTable() {

    ShowTable(page)
    //Cuộn table
    $('.table-responsive').scroll(function () {
        /* var element = $(this)[0];*/
        var scrollTop = $(this).scrollTop() + 3;
        var scrollHeight = $(this)[0].scrollHeight;
        var windowHeight = $(this).outerHeight();

        if (scrollTop + windowHeight >= scrollHeight && !isLoadingData && !isFull) {
            // Đạt đến cuối trang và không đang lấy dữ liệu
            $('.spinner').show();
            // Gọi hàm để lấy dữ liệu tiếp theo
            ShowTable(page);
        }
    });
    //Khởi tạo select
    initSelect()
    
})()

function ShowTable(pagenumber) {

    $.ajax({
        type: "GET",
        url: "/MemberCardLevel/DataTable",
        data: {
            page: pagenumber
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
                <td>${v.CardLevel}</td>
                <td>${v.LevelFee}</td>
                <td>${v.GiftLevelName}</td>
                <td>${v.Vipzone}</td>
               

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
function CreateModal() {
    $('#createModal').modal()
}
function initSelect() {
    $('.select2').select2();
    
    GetLevelFee($('#SelectLevel').val())
    $('#SelectLevel').change(function () {
        GetLevelFee($('#SelectLevel').val())
    })

    GetGiftInfomation($('#SelectGift').val())
    $('#SelectGift').change(function () {
        GetGiftInfomation($('#SelectGift').val())
    })
}
function GetLevelFee(LevelID) {
    $.ajax({
        type: "GET",
        url: "/MemberCardLevel/GetLevelFee",
        data: {
            LevelID
        },

        datatype: 'json',
        success: function (data) {
            if (data.status == "Success") {
                $('input[name="LevelFee"]').val(data.cardlevel.LevelFee.toLocaleString())
            }
            
        },
        error: function () {
            // Xử lý lỗi (nếu cần thiết)
            // Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
            $('.spinner').hide();
            isLoadingData = false;
        }
    })
}
function GetGiftInfomation(GiftID) {
    $.ajax({
        type: "GET",
        url: "/MemberCardLevel/GetGiftInformation",
        data: {
            GiftID
        },

        datatype: 'json',
        success: function (data) {
            if (data.status == "Success") {
                $('input[name="PointPlus"]').val(data.gift.PointPlus)
                $('input[name="RewardRate"]').val(data.gift.RewardRate)
                $('#Personal').prop("checked", data.gift.Personal);
                $('#Holiday').prop("checked", data.gift.Holiday);
                $('#Special').prop("checked", data.gift.Special);
                $('#AvailableTemplates').prop("checked", data.gift.AvailableTemplates);
                $('#CustomizeAvailableTemplate').prop("checked", data.gift.CustomizeAvailableTemplate);
            }
            
        },
        error: function () {
            // Xử lý lỗi (nếu cần thiết)
            // Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
            $('.spinner').hide();
            isLoadingData = false;
        }
    })
}