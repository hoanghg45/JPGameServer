(function DataTable() {
    

})()
function ShowTable() {
    $.ajax({
        type: "GET",
        url: "/AccountsAdmin/DataTable",
        data: {

        },
        datatype: 'json',
        success: function (data) {
            let $table = $('#accounttable')
            let body = $table.find('body')


        }
    })
}