////(function Module() {


////})()

"use strict";

// Class Definition
var Add = function () {




    var _handleSignInForm = function () {
        var validation;
        const form = document.getElementById('CreateAccForm');
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('CreateAccForm'),
            {
                fields: {
                    AccountName: {
                        validators: {
                            notEmpty: {
                                message: 'Yêu Cầu Nhập Tài Khoản'
                            },
                            regexp: {
                                regexp: /^[a-zA-Z0-9]+$/i,
                                message: 'Không Chứa Dấu Và Các Kí Tự Đặc Biệt',
                            },
                            stringLength: {
                                min: 10,
                                max: 12,
                                message: 'Tài khoản Từ 10-12 Kí Tự',
                            },
                        }
                    },
                    FullName: {
                        validators: {
                            notEmpty: {
                                message: 'Yêu Cầu Nhập Tên'
                            }
                        }
                    },
                    Phone: {
                        validators: {
                            notEmpty: {
                                message: 'Yêu Cầu Nhập Số Điện Thoại'
                            },
                            stringLength: {
                                min: 9,
                                max: 11,
                                message: 'Nhập Đúng Số Điện Thoại',
                            },
                        }
                    },
                    Email: {
                        validators: {
                            notEmpty: {
                                message: 'Yêu Cầu Nhập Địa Chỉ Email'
                            },
                            emailAddress: {
                                message: 'Yêu Cầu Nhập Đúng Email',
                            },
                        },
                    },
                    DateOfBirth: {
                        validators: {
                            notEmpty: {
                                message: 'Yêu Cầu Nhập Ngày Sinh'
                            },
                        }
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


        $('.btnCreate').click(function () {
            let data = $('#CreateAccForm').serialize()
            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    CreateAccount(data)
                } else {
                    swal.fire({
                        text: "Vui lòng điền đầy đủ thông tin.",
                        icon: "error",
                        buttonsStyling: false,
                        heightAuto: false,
                        confirmButtonText: "Ok, got it!",
                        customClass: {
                            confirmButton: "btn font-weight-bold btn-light-primary"
                        }
                    }).then(function () {
                        KTUtil.scrollTop();
                    });
                }
            });

        })




        function CreateAccount(data) {
            $.ajax({
                type: "POST",
                url: "/AccountsAdmin/Create",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "success") {
                        Swal.fire({
                            icon: 'success',
                            title: 'Tạo thành công!',
                            showConfirmButton: false,
                            timer: 1500
                        }).then(function () {
                            window.location.href = "/Admin/AccountsAdmin/Index"
                        })
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Đã có lỗi xảy ra, vui lòng thử lại!',
                            text: result.message,
                            showConfirmButton: false,
                            timer: 1500
                        })
                    }
                },
                error: function () {
                    Swal.fire({

                        icon: 'error',
                        title: 'Something wrong!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })
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
});
