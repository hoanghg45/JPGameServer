﻿////(function Module() {

    
////})()

"use strict";

// Class Definition
var Add = function () {




    var _handleSignInForm = function () {
        var validation;
        const form = document.getElementById('ModuleForm');
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('ModuleForm'),
            {
                fields: {
                    Logo: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng chọn Logo'
                            },

                        }
                    },
                    Address: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập địa chỉ'
                            },
                            
                        }
                    }, 
                    
                    Email: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập Email',

                            },
                            emailAddress: {
                                message: 'Vui lòng nhập đúng định dạng Email',
                            },
                        }
                    },
                    Hotline: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng nhập hotline'
                            }
                        }
                    },
                    AboutMe: {
                        validators: {
                            callback: {
                                message: 'Vui lòng nhập nội dung',
                                callback: function (input) {
                                    const code = $('[name="AboutMe"]').summernote('isEmpty');
                                    // <p><br> is code generated by Summernote for empty content
                                    return !code;
                                },
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
        

        $('.btnCreate').click(function () {
            let data = $('#ModuleForm').serialize()
            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    AddModule(data)
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




        function AddModule(data) {
            $.ajax({
                type: "POST",
                url: "/ModulesAdmin/CreateModule",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "success") {
                        Swal.fire({
                            icon: 'success',
                            title: 'Thêm module thành công!',
                            showConfirmButton: false,
                            timer: 1500
                        }).then(function () {
                            window.location.href = "/Admin/ModulesAdmin/Index"
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
