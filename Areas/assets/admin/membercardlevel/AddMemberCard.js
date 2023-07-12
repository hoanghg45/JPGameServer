

"use strict";

// Class Definition
var Add = function () {




    var _handleSignInForm = function () {
        var validation;
        const form = document.getElementById('MemberCardForm');
        // Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
        validation = FormValidation.formValidation(
            KTUtil.getById('MemberCardForm'),
            {
                fields: {
                    LevelFee: {
                        validators: {
                            notEmpty: {
                                message: 'Vui lòng chọn cấp độ thẻ'
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
            let data = $('#MemberCardForm').serializeArray()
            
            validation.validate().then(function (status) {
                if (status == 'Valid') {
                    AddMemberCard(data)
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




        function AddMemberCard(data) {
            $.ajax({
                type: "POST",
                url: "/MemberCardLevel/AddMemberCard",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "Success") {

                        $('#createModal').modal('hide')
                        toastr.success('Thành công!')
                    } else {
                       
                        toastr.error('Lỗi')
                    }
                },
                error: function () {
                    toastr.error('Lỗi')
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
