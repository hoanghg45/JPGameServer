////(function Module() {


////})()

"use strict";

// Class Definition
var Add = function () {




    var _handleSignInForm = function () {
        var validation;
        const form = document.getElementById('UserForm');
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('UserForm'),
            {
                fields: {
                    UserName: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập tài khoản'
                            },
                            stringCase: {
                                message: 'Tài khoản phải viết thường',
                                case: 'lower',
                            },
                        }
                    },

                    Name: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập tên',

                            },

                        }
                    },
                    Password: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập mật khẩu',
                            },
                        },
                    },
                    RePassword: {
                        validators: {
                            identical: {
                                compare: function () {
                                    return form.querySelector('[name="Password"]').value;
                                },
                                message: 'Xác nhận mật khẩu chưa đúng',
                            },
                        },

                    }
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
        form.querySelector('[name="Password"]').addEventListener('input', function () {
            fv.revalidateField('RePassword');
        });

        $('.btnCreate').click(function () {
            let data = $('#UserForm').serialize()

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    AddUser(data)
                } else {
                    toastr.error('Vui lòng điền thông tin đầy đủ và hợp lệ!')
                        .then(function () {
                            KTUtil.scrollTop();
                        });
                }
            });

        })




        function AddUser(data) {
            $.ajax({
                type: "POST",
                url: "/Administrator/Create",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "success") {
                        toastr.success('Thành công!')
                        window.location.href = "/Admin/Administrator"
                    } else {

                        toastr.error('Đã có lỗi xảy ra, vui lòng thử lại!', result.message)
                    }
                },
                error: function () {
                    toastr.error('Đã có lỗi xảy ra, vui lòng thử lại!')
                }
            })
        }

        $('.btnUpdate').click(function () {
            let data = $('#UserForm').serialize()

            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    UpdatePromotion(data)
                } else {
                    toastr.error('Vui lòng điền thông tin đầy đủ và hợp lệ!')
                        .then(function () {
                            KTUtil.scrollTop();
                        });
                }
            });

        })

        function UpdatePromotion(data) {
            $.ajax({
                type: "POST",
                url: "/PromotionAdmin/EditPromotion",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "success") {
                        toastr.success('Thành công!')
                        window.location.href = "/Admin/PromotionAdmin"
                    } else {

                        toastr.error('Đã có lỗi xảy ra, vui lòng thử lại!', result.message)
                    }
                },
                error: function () {
                    toastr.error('Đã có lỗi xảy ra, vui lòng thử lại!')
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
