"use strict";

// Class Definition
var KTLogin = function() {
	var _buttonSpinnerClasses = 'spinner spinner-right spinner-white pr-15';

	var _handleFormSignin = function() {
		var form = KTUtil.getById('kt_login_singin_form');
		var formSubmitUrl = KTUtil.attr(form, 'action');
		var formSubmitButton = KTUtil.getById('kt_login_singin_form_submit_button');

		if (!form) {
			return;
		}

		FormValidation
		    .formValidation(
		        form,
		        {
		            fields: {
						account: {
							validators: {
								notEmpty: {
									message: 'Chưa Nhập Tài Khoản'
								}
							}
						},
						pass: {
							validators: {
								notEmpty: {
									message: 'Chưa Nhập Mật Khẩu'
								}
							}
						}
		            },
		            plugins: {
						trigger: new FormValidation.plugins.Trigger(),
						submitButton: new FormValidation.plugins.SubmitButton(),
	            		//defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
						bootstrap: new FormValidation.plugins.Bootstrap({
						//	eleInvalidClass: '', // Repace with uncomment to hide bootstrap validation icons
						//	eleValidClass: '',   // Repace with uncomment to hide bootstrap validation icons
						})
		            }
		        }
		    )
		    .on('core.form.valid', function() {
				// Show loading state on button
				KTUtil.btnWait(formSubmitButton, _buttonSpinnerClasses, "Kiểm Tra Thông Tin");

				// Simulate Ajax request
				setTimeout(function() {
					var formData = {
						AccountName: $('input[name="account"]').val().trim(),
						Password: $('input[name="pass"]').val().trim(),
					}
					$.ajax({
						url: '/Login/Logins',
						type: 'post',
						data: JSON.stringify(formData),
						contentType: 'application/json',
						success: function (data) {
							if (data.code == 200) {
								window.location.href = "/trang-chu/"
							} else {
								Swal.fire({
									text: data.msg,
									icon: "error",
									buttonsStyling: false,
									confirmButtonText: "Tiếp Tục!",
									customClass: {
										confirmButton: "btn font-weight-bold btn-light-primary"
									}
								}).then(function () {
									KTUtil.btnRelease(formSubmitButton);
									$('input[name="pass"]').val('')
								});
							}
						}
					})
				}, 1000);
		    })
			.on('core.form.invalid', function() {
				Swal.fire({
					text: "Xin Lỗi, Nhập Đủ Và Đúng, Vui Lòng Thử Lại.",
					icon: "error",
					buttonsStyling: false,
					confirmButtonText: "Tiếp Tục!",
					customClass: {
						confirmButton: "btn font-weight-bold btn-light-primary"
					}
				}).then(function() {
					KTUtil.scrollTop();
				});
		    });
    }

	var _handleFormForgot = function() {
		var form = KTUtil.getById('kt_login_forgot_form');
		var formSubmitUrl = KTUtil.attr(form, 'action');
		var formSubmitButton = KTUtil.getById('kt_login_forgot_form_submit_button');

		if (!form) {
			return;
		}

		FormValidation
		    .formValidation(
		        form,
		        {
		            fields: {
						email: {
							validators: {
								notEmpty: {
									message: 'Email is required'
								},
								emailAddress: {
									message: 'The value is not a valid email address'
								}
							}
						}
		            },
		            plugins: {
						trigger: new FormValidation.plugins.Trigger(),
						submitButton: new FormValidation.plugins.SubmitButton(),
	            		//defaultSubmit: new FormValidation.plugins.DefaultSubmit(), // Uncomment this line to enable normal button submit after form validation
						bootstrap: new FormValidation.plugins.Bootstrap({
						//	eleInvalidClass: '', // Repace with uncomment to hide bootstrap validation icons
						//	eleValidClass: '',   // Repace with uncomment to hide bootstrap validation icons
						})
		            }
		        }
		    )
		    .on('core.form.valid', function() {
				// Show loading state on button
				KTUtil.btnWait(formSubmitButton, _buttonSpinnerClasses, "Please wait");

				// Simulate Ajax request
				setTimeout(function() {
					KTUtil.btnRelease(formSubmitButton);
				}, 2000);
		    })
			.on('core.form.invalid', function() {
				Swal.fire({
					text: "Xin Lỗi, Nhập Đủ Và Đúng, Vui Lòng Thử Lại.",
					icon: "error",
					buttonsStyling: false,
					confirmButtonText: "Tiếp Tục",
					customClass: {
						confirmButton: "btn font-weight-bold btn-light-primary"
					}
				}).then(function() {
					KTUtil.scrollTop();
				});
		    });
    }

	var _handleFormSignup = function() {
		// Base elements
		var wizardEl = KTUtil.getById('kt_login');
		var form = KTUtil.getById('kt_login_signup_form');
		var wizardObj;
		var validations = [];

		if (!form) {
			return;
		}

		// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
		// Step 1
		validations.push(FormValidation.formValidation(
			form,
			{
				fields: {
					account: {
						validators: {
							notEmpty: {
								message: 'Yêu Cầu Nhập Tài Khoản'
							},
							regexp: {
								regexp: /^[a-zA-Z0-9]+$/i,
								message: 'Không Chứa Dấu Và Các Kí Tự Đặc Biệt',
							},
							stringLength: {
								min:10,
								max: 12,
								message: 'Tài khoản Từ 10-12 Kí Tự',
							},
						}
					},
					pass: {
						validators: {
							notEmpty: {
								message: 'Yêu Cầu Nhập Mật Khẩu'
							},
							stringLength: {
								min: 6,
								message: 'Mật Khẩu Lớn Hơn 6 Kí Tự',
							},
						}
					},
					pass_again: {
						validators: {
							notEmpty: {
								message: 'Nhập Lại Mật khẩu'
							},
							identical: {
								compare: function () {
									return form.querySelector('[name="pass"]').value;
								},
								message: 'Mật Khẩu Không Khớp'
                            }
						}
					},
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap({
						//eleInvalidClass: '',
						eleValidClass: '',
					}),
					icon: new FormValidation.plugins.Icon({
						valid: 'fa fa-check',
						invalid: 'fa fa-times',
						validating: 'fa fa-refresh',
					}),
				}
			}
		));

		// Step 2
		validations.push(FormValidation.formValidation(
			form,
			{
				fields: {
					fullname: {
						validators: {
							notEmpty: {
								message: 'Yêu Cầu Nhập Tên'
							}
						}
					},
					phone: {
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
					email: {
						validators: {
							notEmpty: {
								message: 'Yêu Cầu Nhập Địa Chỉ Email'
							},
							emailAddress: {
								message: 'Yêu Cầu Nhập Đúng Email',
							},
						},
					},
					birth: {
						validators: {
							notEmpty: {
								message: 'Yêu Cầu Nhập Ngày Sinh'
							},
						}
					},
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap({
						//eleInvalidClass: '',
						eleValidClass: '',
					}),
				}
			}
		));

		// Step 3
		validations.push(FormValidation.formValidation(
			form,
			{
				fields: {
					delivery: {
						validators: {
							notEmpty: {
								message: 'Delivery type is required'
							}
						}
					},
					packaging: {
						validators: {
							notEmpty: {
								message: 'Packaging type is required'
							}
						}
					},
					preferreddelivery: {
						validators: {
							notEmpty: {
								message: 'Preferred delivery window is required'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap({
						//eleInvalidClass: '',
						eleValidClass: '',
					})
				}
			}
		));

		// Step 4
		validations.push(FormValidation.formValidation(
			form,
			{
				fields: {
					ccname: {
						validators: {
							notEmpty: {
								message: 'Credit card name is required'
							}
						}
					},
					ccnumber: {
						validators: {
							notEmpty: {
								message: 'Credit card number is required'
							},
							creditCard: {
								message: 'The credit card number is not valid'
							}
						}
					},
					ccmonth: {
						validators: {
							notEmpty: {
								message: 'Credit card month is required'
							}
						}
					},
					ccyear: {
						validators: {
							notEmpty: {
								message: 'Credit card year is required'
							}
						}
					},
					cccvv: {
						validators: {
							notEmpty: {
								message: 'Credit card CVV is required'
							},
							digits: {
								message: 'The CVV value is not valid. Only numbers is allowed'
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger(),
					// Bootstrap Framework Integration
					bootstrap: new FormValidation.plugins.Bootstrap({
						//eleInvalidClass: '',
						eleValidClass: '',
					})
				}
			}
		));

		// Initialize form wizard
		wizardObj = new KTWizard(wizardEl, {
			startStep: 1, // initial active step number
			clickableSteps: false  // allow step clicking
		});

		// Validation before going to next page
		wizardObj.on('change', function (wizard) {
			//if ($('input[name="pass_again"]').val().trim() != $('input[name="pass"]').val().trim()) {
			//	return;
   //         }
			if (wizard.getStep() > wizard.getNewStep()) {
				return; // Skip if stepped back
			}

			// Validate form before change wizard step
			var validator = validations[wizard.getStep() - 1]; // get validator for currnt step

			if (validator) {
				validator.validate().then(function (status) {
					if (status == 'Valid') {
						wizard.goTo(wizard.getNewStep());

						KTUtil.scrollTop();
					} else {
						Swal.fire({
							text: "Xin Lỗi, Nhập Đủ Và Đúng, Vui Lòng Thử Lại.",
							icon: "error",
							buttonsStyling: false,
							confirmButtonText: "Tiếp Tục!",
							customClass: {
								confirmButton: "btn font-weight-bold btn-light"
							}
						}).then(function () {
							KTUtil.scrollTop();
						});
					}
				});
			}

			return false;  // Do not change wizard step, further action will be handled by he validator
		});

		// Change event
		wizardObj.on('changed', function (wizard) {
			KTUtil.scrollTop();
		});

		// Submit event
		wizardObj.on('submit', function (wizard) {
			var formData = {
				AccountName: $('input[name="account"]').val().trim(),
				Password: $('input[name="pass"]').val().trim(),
				FullName: $('input[name="fullname"]').val().trim(),
				Phone: $('input[name="phone"]').val().trim(),
				Email: $('input[name="email"]').val().trim(),
				DateOfBirth: $('input[name="birth"]').val().trim(),
			}
			$.ajax({
				url: '/Login/Adds',
				type: 'post',
				data: JSON.stringify(formData),
				contentType: 'application/json',
				success: function (data) {
					if (data.code == 200) {
						Swal.fire({
							text: "Đăng Kí Thành Công, Mời Bạn Đăng Nhập",
							icon: "success",
							showCancelButton: true,
							buttonsStyling: false,
							confirmButtonText: "Đăng Nhập!",
							customClass: {
								confirmButton: "btn font-weight-bold btn-primary",
								cancelButton: "btn font-weight-bold btn-default"
							}
						}).then(function (result) {
							if (result.value) {
								window.location.href = "/dang-nhap/"
							}
						});
					} else {
						Swal.fire(
							data.msg,
							'',
							'erorr'
						)
					}
				}
			})
	
		});
    }

    // Public Functions
    return {
        init: function() {
            _handleFormSignin();
			_handleFormForgot();
			_handleFormSignup();
        }
    };
}();

// Class Initialization
jQuery(document).ready(function() {
    KTLogin.init();
});
