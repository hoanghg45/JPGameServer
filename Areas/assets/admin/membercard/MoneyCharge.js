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
					CurrCardID: {
						validators: {
							notEmpty: {
								message: 'Vui lòng quét thẻ'
							}
						}
					},
					CurrBalance: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu số tiền'
							}
						}
					},
					CurrTotal: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu số tiền tổng'
							}
						}
					},
					CurrLevelName: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu cấp độ thẻ'
							},

						}
					},
					CurrGiftLevelName: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu cấp độ quà'
							},

						}
					},
					CurrRewardRate: {
						validators: {
							notEmpty: {
								message: 'Yêu cầu tỉ lệ tiền'
							},

						}
					},
					CurrPointPlus: {
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
					MoneyPay: {
						validators: {
							notEmpty: {
								message: 'Vui lòng nhập số tiền nạp'
							},
							between: {
								min: 1,
								max: 999999999999,
								message: 'Số tiền nạp phải lớn hơn 0',
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



		// Step 3
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
		// Step 4
		_validations.push(FormValidation.formValidation(
			_formEl,
			{
				fields: {
					NewCardID: {
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
		var isEnterInfor = false
		_wizardObj.on('change', function (wizard) {

			if (wizard.getStep() > wizard.getNewStep()) {
				/// Nếu là thẻ welcome thì bỏ bước thông tin
				if (wizard.getStep() == 4 && !isEnterInfor)
					wizard.goTo(wizard.getStep() - 1);
				return; // Skip if stepped back
			}

			// Validate form before change wizard step
			var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step

			if (validator) {
				let level = $('input[name="CurrCardLevelID"]').val().trim()
				let nextlevel = $('input[name="LevelID"]').val().trim()
				let step = wizard.getNewStep()
				if (level == "level1" && nextlevel == level  && step == 4) {
					SetReviewStep()
					wizard.goTo(4);
				} else {
				validator.validate().then(function (status) {
					if (status == 'Valid') {
						let nextStep = wizard.getNewStep()
						if (nextStep == 3) {
							if ($("input[name='radiospay']:checked").val() == 2 && $("input[name='Paycode']").val() == "") {
								$('#PaycodeNoti').show()
								toastr.error($('#PaycodeNoti').text(), "Lỗi!")
								KTUtil.scrollTop();
								return
							}
							if ($("input[name='radiospay']:checked").val() == 1 && $("input[name='CusMoney']").val() == "") {
								toastr.error($('#CusmoneyNoti').text(), "Lỗi!")
								$('#CusmoneyNoti').show()
								KTUtil.scrollTop();
								return
							} 
                        }
						if (nextStep == 3) {
							/// Nếu là thẻ welcome thì bỏ bước thông tin
							nextStep = SkipInfoStep(nextStep)

						}
						if (nextStep == 4) {
							SetReviewStep()
						}
						
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
			}
			///
			function SetReviewStep() {
				if ($('input[name = "IsHaveOwner"]').val() == 'true' || $('input[name = "AccountName"]').val() =='' ) {
					$('#NameContain').hide()
				} else {
					
					$('#NameContain').show()
				}
				/// Nếu nâng cấp thẻ
				if ($('input[name = "LevelName"]').val() != $('input[name = "CurrLevelName"]').val()) {
					$('#ScanNewCardID').show()
				

				} else {
					$('#ScanNewCardID').hide()
					$('input[name = "NewCardID"]').val($('input[name = "CurrCardID"]').val())
					

                }


				$('input[name = "LevelNameReview"]').val($('input[name = "LevelName"]').val())

				let point = Number($('input[name = "PointPlus"]').val()) + Number($('input[name = "CurrPoints"]').val())
				$('input[name = "Money"]').val($('input[name = "CardMoneyPay"]').val())
				$('input[name = "Point"]').val(point)
				
			}
			function SkipInfoStep(nextStep) {
				var oldTotal = Number($('input[name="CurrTotal"]').val().replaceAll(',',''));
				var newTotal = Number($('input[name="TotalMoneyPay"]').val().replaceAll(',',''));
				


				isEnterInfor = oldTotal < 3000000 && newTotal >= 3000000 || $('input[name = "IsHaveOwner"]').val() == 'false'
				return isEnterInfor ? nextStep : nextStep+1
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
						ChargeMemberCard()
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

		function ChargeMemberCard() {
			let data = $('#chargeMemberCardForm').serializeArray()
			$.ajax({
				type: "POST",
				url: "/MemberCard/MoneyCharge",
				data: data,
				datatype: "json",
				success: function (result) {
					if (result.status == "Success") {


						toastr.success('Thành công!')
						///
						let paytype = $('input[name="radiospay"]:checked').data('type');
						let cusMoney = 0;
						let changeMoney = 0
						let promotiondes = ""
						if (paytype == 'Tien mat') {
							cusMoney = $('input[name="CusMoney"]').val()
							changeMoney = $('input[name="ChangeMoney"]').val()
						}
						if (promotionInfo != null) {
							promotiondes = promotionInfo.Des
						}
						let cardcode = $('input[name="CurrCardID"]').val() == $('input[name="NewCardID"]').val() ? $('input[name="CurrCardID"]').val() : $('input[name="NewCardID"]').val()
						Print(data, result.userID, result.sp, result.userName, "PHIEU NAP TIEN", result.cashier, paytype, result.member, cusMoney, changeMoney, promotiondes, cardcode);
						Print(data, result.userID, result.sp, result.userName, "PHIEU NAP TIEN", result.cashier, paytype, result.member, cusMoney, changeMoney, promotiondes, cardcode);
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

	function SaveReportRecharge(data, userID, userName) {
		var idCard = "";
		let paytypeid = $('input[name="radiospay"]:checked').val();
		let money = $('input[name = "MoneyPay"]').val().replaceAll(',', '')
		$.each(data, function (k, v) {
			if (v.name == "NewCardID") {
				idCard = v.value
			}
		})

		$.ajax({
			type: "POST",
			url: "/MemberCard/SaveReportRecharge",
			data: {
				userID: userID, idCard: idCard, paytype: paytypeid, money
			},
			success: function (result) {
				if (result.status == "Success") {
					
				
				} else {
					toastr.error(data.message, 'Lỗi')
				}
			},
			error: function () {
				toastr.error('Lỗi')
			}
		})
	}
	return {
		// public functions
		init: function () {
			_wizardEl = KTUtil.getById('kt_wizard');
			_formEl = KTUtil.getById('chargeMemberCardForm');

			_initValidation();
			_initWizard();
		}
	};
}();

jQuery(document).ready(function () {
	KTWizard1.init();

	var membercardHub = $.connection.membercardHub;
	console.log(membercardHub)
	membercardHub.client.notify = function (message) {
		

		if (message && message.toLowerCase() == "cardscanned" && ReaderID != null) {
			ScanTag()
		}
	}
	$.connection.hub.start().done(function () {
		console.log('Hub started');
	});
	/*signalr method for push server message to client*/

	InitInputEvent()
	initMasks()
	InitLoadingButton()
	$('#PaycodeNoti').hide()
	$('#CusmoneyNoti').hide()
	
});

function ScanTag() {
	///Lấy thông tin mỗi khi quét thẻ khi đang ở trang quét thẻ
	let isInOldCardStep = $('[data-wizard-state="current"]').first().data('step') == 1
	let isInNewCardStep = $('[data-wizard-state="current"]').first().data('step') == 4
	if (isInOldCardStep) {
		$('#CheckCurrCardBtn').trigger("click");
	}
	if (isInNewCardStep) {
		$('#CheckChargeCurrCardBtn').trigger("click");
	}



}
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
				$('input[name="FullNameReview"]').val(data.data.FullName)
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
		url: "/MemberCardLevel/GetMemberCardLevelForCharge",
		data: {
			LevelFee
		},

		datatype: 'json',
		success: function (data) {
			if (data.status == "Success") {
				let rate = data.data.RewardRate
				//Chỉ được nhân tiền với tỉ lệ 1 lần

				
				$('input[name="LevelName"]').val(data.data.LevelName)
				$('input[name="GiftLevelName"]').val(data.data.GiftLevelName)
				$('input[name="LevelID"]').val(data.data.CardLevelID.trim())
				$('input[name="PointPlus"]').val(data.data.PointPlus)			
				
				$('#AvailableTemplates').prop("checked", data.data.AvailableTemplates);
				$('#CustomizeAvailableTemplate').prop("checked", data.data.CustomizeAvailableTemplate);
				$('#Holiday').prop("checked", data.data.Holiday);
				$('#Special').prop("checked", data.data.Special);
				$('#Personal').prop("checked", data.data.Personal);
				$('#VIP').prop("checked", data.data.VIP);
				$('#Mocktail').prop("checked", data.data.Mocktail);
				$('#VipRoom').prop("checked", data.data.VipRoom);
				if (data.data.CardLevelID.trim() == $('input[name = "CurrCardLevelID"]').val()) {
					rate = 0
				}
				$('input[name="RewardRate"]').val(rate)
				SetMoney(rate, data.data.LevelFee)
			} else {
				toastr.error(data.message, "Lỗi!")
				$('input[name="MoneyPay"],input[name="TotalMoneyPay"],input[name="CardMoneyPay"]').val('')
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
	$('#CheckCurrCardBtn').on('click', function () {
		let $this = $(this)
		$this.hide();
		$('#loadingBtn').show()
		$("#iconStatus").removeClass()
		GetCurrCard($this)
		$('input[name="CheckCurrCardBtn"]').val("")
		$('#iconStatus').show()
	});
	InitLoadingButtonForCharge()
}
function InitLoadingButtonForCharge() {
	$('#loadingChargeBtn').hide()
	$('#iconChargeStatus').hide()
	$('#CheckChargeCurrCardBtn').on('click', function () {
		let $this = $(this)
		$this.hide();
		$('#loadingChargeBtn').show()
		$("#iconChargeStatus").removeClass()
		GetNewCard($this, $('input[name="LevelName"]').val())
		$('input[name="CheckChargeCurrCardBtn"]').val("")
		$('#iconChargeStatus').show()
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
				$('input[name="CurrCardID"]').val(data.card.MemberCardID)
				$('input[name="CurrCardLevelID"]').val(data.card.CardLevelID.trim())
				$('input[name="IsHaveOwner"]').val(data.card.Owner == null ? false : true)
				$('input[name="CurrLevelName"]').val(data.card.LevelName)
				$('input[name="CurrGiftLevelName"]').val(data.card.GiftLevelName)
				$('input[name="CurrRewardRate"]').val(data.card.RewardRate)
				$('input[name="CurrPoints"]').val(data.card.Points)
				
				$('input[name="CurrBalance"]').val(data.card.Balance.toLocaleString('en-US'))
				$('input[name="FullNameReview"]').val(data.card.Owner == null ? "" : data.card.Owner)
				let total = data.card.Total == null ? 0 : data.card.Total.toLocaleString('en-US')
				$('input[name="CurrTotal"]').val(total)
				$('#CurrAvailableTemplates').prop("checked", data.card.AvailableTemplates);
				$('#CurrCustomizeAvailableTemplate').prop("checked", data.card.CustomizeAvailableTemplate);
				$('#CurrHoliday').prop("checked", data.card.Holiday);
				$('#CurrSpecial').prop("checked", data.card.Special);
				$('#CurrPersonal').prop("checked", data.card.Personal);
				$('#CurrVIP').prop("checked", data.card.VIP);
				$('#CurrMocktail').prop("checked", data.card.Mocktail);
				$('#CurrVipRoom').prop("checked", data.card.VipRoom);

				
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

function GetNewCard($this,level) {
	$.ajax({
		type: "GET",
		url: "/MemberCard/GetCurrentCardForCreate",
		data: { level },

		datatype: 'json',
		complete: function () {
			$this.show();
			$('#loadingChargeBtn').hide()

		},
		success: function (data) {
			if (data.status == "Success") {
				$('#iconStatusCharge').addClass("flaticon2-check-mark text-success");
				$('#textChargeNoti').text("Thẻ hợp lệ!");
				$('input[name="NewCardID"]').val(data.card)
			} else {
				toastr.error("Lỗi!")

				$('#textChargeNoti').text(data.message);
				$('#iconStatusCharge').addClass("flaticon2-delete text-danger");

			}


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo

		}
	})
}

function InitInputEvent() {

	///AccoutName
	$('input[name="AccountName"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn ngoài input

			$('input[name="FullName"]').val("")
			$('input[name="Email"]').val("")
			$('input[name="Phone"]').val("")
			$('input[name="DateOfBirth"]').val("")
			GetUser($(this).val())
		}
	});
	$('input[name="AccountName"]').on('blur', function (event) {
		event.preventDefault();
		// Xử lý logic khi nhấn ngoài input

		$('input[name="FullName"]').val("")
		$('input[name="Email"]').val("")
		$('input[name="Phone"]').val("")
		$('input[name="DateOfBirth"]').val("")
		GetUser($(this).val())
	});


	//Money
	$('input[name="MoneyPay"]').on('blur', function (event) {
		$('input[name="PromotionDes"],input[name="Promotion"]').val('')
		
		let money = Number($(this).val().replaceAll(',', ''))
		let total = Number($('input[name="CurrTotal"]').val().replaceAll(',', ''))
		
		let payTotal = money + total
		$('input[name="TotalMoneyPay"]').val(payTotal.toLocaleString('en-US'))
		
		GetMemberCardLevel(payTotal)
	});
	$('input[name="MoneyPay"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn phím Enter
			$('input[name="PromotionDes"],input[name="Promotion"]').val('')


			let money = Number($(this).val().replaceAll(',', ''))
			let total = Number($('input[name="CurrTotal"]').val().replaceAll(',', ''))

			let payTotal = money + total
			$('input[name="TotalMoneyPay"]').val(payTotal.toLocaleString('en-US'))

			GetMemberCardLevel(payTotal)
		}
	});
	/// tiền thừa
	$('input[name="CusMoney"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn ngoài input
			let money = Number($('input[name="MoneyPay"]').val().replaceAll(',', ''))
			let cusMoney = Number($(this).val().replaceAll(',', ''))
			let changeMoney = 0
			if ($(this).val() == '')
				return

			if (money == '') {
				toastr.error("Vui lòng nhập số tiền cần nạp", "Lỗi!")
				$(this).val('')
			} else if (money > cusMoney) {

				toastr.error("Số tiền cần nạp phải nhỏ hơn số tiền khách đưa", "Lỗi!")
				$(this).val('')
			}
			else if (money <= cusMoney) {

				let changeMoney = cusMoney - money
				$('input[name="ChangeMoney"]').val(changeMoney.toLocaleString('en-US'))
			}


		}
	});
	//$('input[name="CusMoney"]').on('blur', function (event) {
	//	event.preventDefault();
	//	// Xử lý logic khi nhấn ngoài input
	//	let money = Number($('input[name="MoneyPay"]').val().replaceAll(',', ''))
	//	let cusMoney = Number($(this).val().replaceAll(',', ''))
	//	let changeMoney = 0
	//	if ($(this).val() == '')
	//		return

	//	if (money == '') {
	//		toastr.error("Vui lòng nhập số tiền cần nạp", "Lỗi!")
	//		$(this).val('')
	//	} else if (money > cusMoney) {

	//		toastr.error("Số tiền cần nạp phải nhỏ hơn số tiền khách đưa", "Lỗi!")
	//		$(this).val('')
	//	}
	//	else if (money <= cusMoney) {

	//		let changeMoney = cusMoney - money
	//		$('input[name="ChangeMoney"]').val(changeMoney.toLocaleString('en-US'))
	//	}


	//});
	// Mã khuyến mãi
	//$('input[name="Promotion"]').on('blur', function (event) {
	//	/// xử lý khi ấn ra ngoài
	//	$('input[name="PromotionDes"]').val('')

	//	$('input[name="ChangeMoney"],input[name="CusMoney"]').val("")
	//	promotionInfo = null
	//	let IdPromotion = $(this).val()
	//	if (IdPromotion == '')
	//		return
	//	GetPromotion(IdPromotion)


	//});
	$('input[name="Promotion"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn phím Enter
			promotionInfo = null
			$('input[name="ChangeMoney"],input[name="CusMoney"]').val("")
			$('input[name="PromotionDes"]').val('')

			let IdPromotion = $(this).val()
			if (IdPromotion == '')
				return
			GetPromotion(IdPromotion)

		}
	});

}
function GetPromotion(IdPromotion) {

	$.ajax({
		type: "GET",
		url: "/dataapi/Promotion",
		data: { IdPromotion },

		datatype: 'json',
		complete: function () {

			$('#loadingBtn').hide()

		},
		success: function (data) {
			if (data.status == "ok") {

				let promotion = data.message[0]
				//Hiển thị loại khuyến mãi

				if (promotion.Type == 1) {
					//Nhập số tiền thẻ tặng

					toastr.error(`Mã ${promotion.Des} áp dụng tặng thẻ, vui lòng chọn chức năng cấp thẻ để sử dụng!`)
					$('input[name="MoneyPay"],input[name="Promotion]').val("")
					promotionInfo = null
					$('input[name="Promotion"]').attr('disabled', true)
				}
				if (promotion.Type == 2) {
					//Nhập số tiền thẻ tặng
					let moneypay = $('input[name="MoneyPay"]').val()
					if (moneypay == '' || Number(moneypay.replaceAll(',', '')) < promotion.MinimumMoney) {
						toastr.error(`Số tiền nạp phải đạt mức tối thiểu ${promotion.MinimumMoney.toLocaleString('en-US')} để nhận ưu đãi này!`, "Lỗi!")
						promotionInfo = null
						$('input[name="MoneyPay"],input[name="Promotion"]').val("")
						
						return
					}
					promotionInfo = promotion

					
					let total = Number($('input[name="CardMoneyPay"]').val().replaceAll(',', ''))
					let payTotal = total + Number(moneypay.replaceAll(',', ''))
				
					$('input[name="CardMoneyPay"]').val(payTotal.toLocaleString('en-US'))
					toastr.success(`Áp dụng thành công mã ${promotion.Des}!`)
					$('input[name="PromotionDes"]').val(promotion.Des)
					$('input[name="Promotion"]').attr('disabled', true)
				}


				

			} else {
				toastr.error(data.message, "Lỗi!")
				$('input[name="MoneyPay"],input[name="Promotion]').val("")
				promotionInfo = null


			}


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
			toastr.error("Lỗi!")
		}
	})
}
function SetMoney(rate, levelfee) {

	let money = Number($('input[name="MoneyPay"]').val().replaceAll(',', ''))
	///
	money = rate == 0 ? money : levelfee * (rate/100)
	let currBalance = Number($('input[name="CurrBalance"]').val().replaceAll(',', ''))
	let newBalance = money + currBalance
	$('input[name="CardMoneyPay"]').val(newBalance.toLocaleString('en-US'))

}

