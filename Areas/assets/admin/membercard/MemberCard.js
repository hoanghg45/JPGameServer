"use strict";

// Class definition
var KTWizard1 = function () {
	// Base elements
	var _wizardEl;
	var _formEl;
	var _wizardObj;
	var _validations = [];

	// Private functions
	var _initValidation = function () {
		// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
		// Step 1
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					MoneyPay: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu số tiền nạp'
							}
						}
					},
					LevelName: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu cấp độ thẻ'
							},

						}
					},
					GiftLevelName: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu cấp độ quà'
							},

						}
					},
					RewardRate: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu tỉ lệ tiền'
							},

						}
					},
					PointPlus: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu sô tiền tặng'    
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
					})
				}
			}
		));

		// Step 2
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					AccountName: {
						validators: {
							notEmpty: {
								message: 'Vui lòng nhập tên tài khoản'
							}
						}
					},
					FullName: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu họ và tên'
							}
						}
					},
					DateOfBirth: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu ngày sinh'
							}
						}
					},
					Email: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu nhập địa chỉ Email'
							}
						}
					},
					Phone: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu nhập số điện thoại'
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
					})
				}
			}
		));

		

		// Step 3
		_validations.push(FormValidation.formValidation(
			_formEl,
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
		_validations.push(FormValidation.formValidation(
			_formEl,
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

	
	}

	var _initWizard = function () {
		// Initialize form wizard
		_wizardObj = new KTWizard(_wizardEl, {
			startStep: 1, // initial active step number
			clickableSteps: false  // allow step clicking
		});

		// Validation before going to next page
		_wizardObj.on('change', function (wizard) {
			
			if (wizard.getStep() > wizard.getNewStep()) {
				return; // Skip if stepped back
			}

			// Validate form before change wizard step
			var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

			if (validator) {
				validator.validate().then(function (status) {
					if (status == 'Valid') {
						let nextStep = wizard.getNewStep()
						if (wizard.getNewStep() == 2) {
							nextStep = SkipInfoStep(nextStep)
                        }
						if (wizard.getNewStep() == 3) {
							SetReviewStep()
                        }
						///
						wizard.goTo(nextStep);
						
						KTUtil.scrollTop();
					} else {
						Swal.fire({
							text: "Đã có lỗi trong quá trình, vui lòng thử lại",
							icon: "error",
							buttonsStyling: false,
							confirmButtonText: "Ok, got it!",
							customClass: {
								confirmButton: "btn font-weight-bold btn-light"
							}
						}).then(function () {
							KTUtil.scrollTop();
						});
					}
				});
			}
			///
			function SetReviewStep() {
				$('input[name = "FullNameReview"]').val($('input[name = "FullName"]').val())
				$('input[name = "LevelNameReview"]').val($('input[name = "LevelName"]').val())
				let rate = Number($('input[name = "RewardRate"]').val())
				let money = Number($('input[name = "MoneyPay"]').val().replaceAll(",", ""))
				let moneyReward = rate != 0? money * rate / 100 : money 
				$('input[name = "Money"]').val(moneyReward.toLocaleString('en-US'))
				$('input[name = "Point"]').val($('input[name = "PointPlus"]').val())
			}
			function SkipInfoStep(nextStep) {
				return $('input[name = "LevelName"]').val() == "Welcome" ? 4 : nextStep			
			}
			return false;  // Do not change wizard step, further action will be handled by he validator
		});

		// Change event
		_wizardObj.on('changed', function (wizard) {
			KTUtil.scrollTop();
		});

		// Submit event
		_wizardObj.on('submit', function (wizard) {
			Swal.fire({
				text: "All is good! Please confirm the form submission.",
				icon: "success",
				showCancelButton: true,
				buttonsStyling: false,
				confirmButtonText: "Yes, submit!",
				cancelButtonText: "No, cancel",
				customClass: {
					confirmButton: "btn font-weight-bold btn-primary",
					cancelButton: "btn font-weight-bold btn-default"
				}
			}).then(function (result) {
				if (result.value) {
					_formEl.submit(); // Submit form
				} else if (result.dismiss === 'cancel') {
					Swal.fire({
						text: "Your form has not been submitted!.",
						icon: "error",
						buttonsStyling: false,
						confirmButtonText: "Ok, got it!",
						customClass: {
							confirmButton: "btn font-weight-bold btn-primary",
						}
					});
				}
			});
		});

		
	}

	return {
		// public functions
		init: function () {
			_wizardEl = KTUtil.getById('kt_wizard');
			_formEl = KTUtil.getById('kt_form');

			_initValidation();
			_initWizard();
		}
	};
}();

jQuery(document).ready(function () {
	KTWizard1.init();
	$('input[name="AccountName"').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn phím Enter
			
			$('input[name="FullName"]').val("")
			$('input[name="Email"]').val("")
			$('input[name="Phone"]').val("")
			$('input[name="DateOfBirth"]').val("")
			GetUser($(this).val())
		}
	});
	$('input[name="MoneyPay"').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn phím Enter
			let money = $(this).val().replaceAll(',','')
			
			GetMemberCardLevel(money)
		}
	});
	initMasks()
});


function GetUser(UserName) {
	$.ajax({
		type: "GET",
		url: "/Login/GetAccountInfo",
		data: {
			UserName
		},

		datatype: 'json',
		success: function (data) {
			if (data.code == 200) {
				
				$('input[name="FullName"]').val(data.data.FullName)
				$('input[name="Email"]').val(data.data.Email)
				$('input[name="Phone"]').val(data.data.Phone)
				$('input[name="DateOfBirth"]').val(formatDate(data.data.DateOfBirth))
			} else {
				toastr.error(data.msg,"Lỗi!")
            }


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
			
		}
	})
}

function GetMemberCardLevel(LevelFee) {
	$.ajax({
		type: "GET",
		url: "/MemberCardLevel/GetMemberCardLevel",
		data: {
			LevelFee
		},

		datatype: 'json',
		success: function (data) {
			if (data.status == "Success") {
				
				$('input[name="LevelName"]').val(data.data.LevelName)
				$('input[name="GiftLevelName"]').val(data.data.GiftLevelName)
				$('input[name="RewardRate"]').val(data.data.RewardRate)
				$('input[name="PointPlus"]').val(data.data.PointPlus)
				$('#AvailableTemplates').prop("checked", data.data.AvailableTemplates);
				$('#CustomizeAvailableTemplate').prop("checked", data.data.CustomizeAvailableTemplate);
				$('#Holiday').prop("checked", data.data.Holiday);
				$('#Special').prop("checked", data.data.Special);
				$('#Personal').prop("checked", data.data.Personal);
				$('#VIP').prop("checked", data.data.VIP);
				$('#Mocktail').prop("checked", data.data.Mocktail);
				$('#VipRoom').prop("checked", data.data.VipRoom);
			} else {
				toastr.error(data.message,"Lỗi!")
            }


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
			
		}
	})
}
function initMasks() {
	$('.money').mask('000,000,000,000,000,000,000,000,000', {
		reverse: true
	});

}