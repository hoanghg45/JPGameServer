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
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					CardID: {
						validators: {
							notEmpty: {
								message: 'Vui lòng quét lại'
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


	}

	var _initWizard = function () {
		// Initialize form wizard
		_wizardObj = new KTWizard(_wizardEl, {
			startStep: 1, // initial active step number
			clickableSteps: false  // allow step clicking
		});

		// Validation before going to next page
		var isWelcome = false
		_wizardObj.on('change', function (wizard) {

			if (wizard.getStep() > wizard.getNewStep()) {
				/// Nếu là thẻ welcome thì bỏ bước thông tin
				if (wizard.getStep() == 3 && isWelcome)
					wizard.goTo(wizard.getStep() - 2);
				return; // Skip if stepped back
			}

			// Validate form before change wizard step
			var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

			if (validator) {
				validator.validate().then(function (status) {
					if (status == 'Valid') {
						let nextStep = wizard.getNewStep()
						if (nextStep == 2) {
							/// Nếu là thẻ welcome thì bỏ bước thông tin
							nextStep = SkipInfoStep(nextStep)

						}
						if (nextStep == 3) {
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
							heightAuto: false,
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
				if ($('input[name = "LevelName"]').val() == "Welcome") {
					$('#NameContain').hide()
				} else {
					$('input[name = "FullNameReview"]').val($('input[name = "FullName"]').val())
					$('#NameContain').show()
				}
				$('input[name = "LevelNameReview"]').val($('input[name = "LevelName"]').val())
				let rate = Number($('input[name = "RewardRate"]').val())
				let money = Number($('input[name = "MoneyPay"]').val().replaceAll(",", ""))
				let moneyReward = rate != 0 ? money * rate / 100 : money
				$('input[name = "Money"]').val(moneyReward.toLocaleString('en-US'))
				$('input[name = "Point"]').val($('input[name = "PointPlus"]').val())
			}
			function SkipInfoStep(nextStep) {
				isWelcome = $('input[name = "LevelName"]').val() == "Welcome"
				return isWelcome ? 3 : nextStep
			}
			return false;  // Do not change wizard step, further action will be handled by he validator
		});

		// Change event
		_wizardObj.on('changed', function (wizard) {
			KTUtil.scrollTop();
		});

		// Submit event
		_wizardObj.on('submit', function (wizard) {
			// Validate form before change wizard step
			var validator = _validations[wizard.getStep() - 1];
			if (validator) {
				validator.validate().then(function (status) {
					if (status == 'Valid') {
						Swal.fire({
							text: "All is good! Please confirm the form submission.",
							icon: "success",
							showCancelButton: true,
							buttonsStyling: false,
							confirmButtonText: "Yes, submit!",
							cancelButtonText: "No, cancel",
							heightAuto: false,
							customClass: {
								confirmButton: "btn font-weight-bold btn-primary",
								cancelButton: "btn font-weight-bold btn-default"
							}
						}).then(function (result) {
							if (result.value) {
								AddMemberCard()
							} else if (result.dismiss === 'cancel') {
								Swal.fire({
									text: "Your form has not been submitted!.",
									icon: "error",
									buttonsStyling: false,
									heightAuto: false,
									confirmButtonText: "Ok, got it!",
									customClass: {
										confirmButton: "btn font-weight-bold btn-primary",
									}
								});
							}
						});
					} else {
						Swal.fire({
							text: "Đã có lỗi trong quá trình, vui lòng thử lại",
							icon: "error",
							buttonsStyling: false,
							confirmButtonText: "Ok, got it!",
							heightAuto: false,
							customClass: {
								confirmButton: "btn font-weight-bold btn-light"
							}
						}).then(function () {
							KTUtil.scrollTop();
						});
					}
				});
			}

		});

		function AddMemberCard() {
			let data = $('#createMemberCardForm').serializeArray()
			$.ajax({
				type: "POST",
				url: "/MemberCard/Create",
				data: data,
				datatype: "json",
				success: function (result) {
					if (result.status == "Success") {


						toastr.success('Thành công!')
						location.reload()
					} else {

						toastr.error(data.message, 'Lỗi')
					}
				},
				error: function () {
					toastr.error('Lỗi')
				}
			})

		}
	}

	return {
		// public functions
		init: function () {
			_wizardEl = KTUtil.getById('kt_wizard');
			_formEl = KTUtil.getById('createMemberCardForm');

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
			let money = $(this).val().replaceAll(',', '')

			GetMemberCardLevel(money)
		}
	});
	initMasks()
	InitLoadingButton()
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
				toastr.error(data.msg, "Lỗi!")
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
				toastr.error(data.message, "Lỗi!")
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
function InitLoadingButton() {
	$('#loadingBtn').hide()
	$('#iconStatus').hide()
	$('#CheckCardBtn').on('click', function () {
		let $this = $(this)
		$this.hide();
		$('#loadingBtn').show()
		$("#iconStatus").removeClass()
		GetCurrCard($this, $('input[name="LevelName"]').val())
		$('input[name="CardID"]').val("")
		$('#iconStatus').show()
	});

}
function GetCurrCard($this) {
	$.ajax({
		type: "GET",
		url: "/MemberCard/GetCurrentCardForCharge",
		

		datatype: 'json',
		complete: function () {
			$this.show();
			$('#loadingBtn').hide()

		},
		success: function (data) {
			if (data.status == "Success") {
				$('#iconStatus').addClass("flaticon2-check-mark text-success");
				$('#textNoti').text("Thẻ hợp lệ!");
				$('input[name="CardID"]').val(data.card.MemberCardID)
				$('input[name="LevelName"]').val(data.card.LevelName)
				$('input[name="GiftLevelName"]').val(data.card.GiftLevelName)
				$('input[name="RewardRate"]').val(data.card.RewardRate)
				$('input[name="PointPlus"]').val(data.card.PointPlus)
				$('#AvailableTemplates').prop("checked", data.card.AvailableTemplates);
				$('#CustomizeAvailableTemplate').prop("checked", data.card.CustomizeAvailableTemplate);
				$('#Holiday').prop("checked", data.card.Holiday);
				$('#Special').prop("checked", data.card.Special);
				$('#Personal').prop("checked", data.card.Personal);
				$('#VIP').prop("checked", data.card.VIP);
				$('#Mocktail').prop("checked", data.card.Mocktail);
				$('#VipRoom').prop("checked", data.card.VipRoom);
			} else {
				toastr.error("Lỗi!")

				$('#textNoti').text(data.message);
				$('#iconStatus').addClass("flaticon2-delete text-danger");

			}


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo

		}
	})
}