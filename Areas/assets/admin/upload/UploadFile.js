

"use strict";

// Class Definition
var Add = function () {




    var _handleSignInForm = function () {
        var validation;
        const form = document.getElementById('uploadForm');
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('uploadForm'),
            {
                fields: {
                    file: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng chọn file',
                            },
                            file: {
                                extension: 'xls,xlsx',
                                type: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',

                                message: 'File không hợp lệ, chỉ nhận (xls, xlsx)',
                            },
                        },
                    },


                },

                plugins: {
                    trigger: new FormValidation.plugins.Trigger(),
                    submitButton: new FormValidation.plugins.SubmitButton(),
                    //defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
                    bootstrap: new FormValidation.plugins.Bootstrap(),
                    icon: new FormValidation.plugins.Icon({
                        valid: 'fa fa-check',
                        invalid: 'fa fa-times',
                        validating: 'fa fa-refresh',
                    }),
                }
            }
        );
     


        $('#UploadFile').click(function () {
            //let data = $('#uploadForm').serialize()

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    UploadFile()
                } else {
                    toastr.error('Vui lòng điền thông tin đầy đủ và hợp lệ!')
                        .then(function () {
                            KTUtil.scrollTop();
                        });
                }
            });

        })


        const showLoading = function () {
            swal({
                title: 'Now loading',
                allowEscapeKey: false,
                allowOutsideClick: false,
                timer: 2000,
                onOpen: () => {
                    swal.showLoading();
                }
            }).then(
                () => { },
                (dismiss) => {
                    if (dismiss === 'timer') {
                        console.log('closed by timer!!!!');
                        swal({
                            title: 'Finished!',
                            type: 'success',
                            timer: 2000,
                            showConfirmButton: false
                        })
                    }
                }
            )
        };

        function UploadFile() {
            // Lấy file được chọn
            var file = $('#excelFile')[0].files[0];

            // Tạo đối tượng FormData để chứa file
            var formData = new FormData();
            formData.append('file', file);

            // Gửi yêu cầu Ajax đến server
            $.ajax({
                url: '/MemberCard/UploadExcel', 
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: function () {
                    Swal.fire({
                        title: 'Đang xử lí',
                        allowEscapeKey: false,
                        allowOutsideClick: false,
                        onOpen: () => {
                            Swal.showLoading();
                        }
                    });
                },
                success: function (response) {
                    Swal.close();
                    if (response.status) {
                        toastr.success('Thêm thành công!')
                        $('#membercardtable').find('tbody').empty()
                        ShowTable(1)
                        $('#UploadFileModal').modal('hide')
                    }
                    else
                        toastr.error(response.message, 'Lỗi!')
                },
                error: function (xhr, status, error) {
                    Swal.close();
                    // Xử lý lỗi (nếu có)
                    toastr.error(error,'Lỗi!')
                }
            });
        }

     
    }



    // Public Functions
    return {
        // public functions
        init: function () {


            _handleSignInForm();

        }
    };
}();

// Class Initialization
jQuery(document).ready(function () {
    Add.init();
    $('.select2').select2();
});
