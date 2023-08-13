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
		var isGetCusData = false ///Phải nhập thông tin khách
		_wizardObj.on('change', function (wizard) {
			
			if (wizard.getStep() > wizard.getNewStep()) {
				/// Nếu là thẻ welcome thì bỏ bước thông tin
				if (wizard.getStep() == 3 && isWelcome)
					wizard.goTo(wizard.getStep()-2);
				return; // Skip if stepped back
			}

			// Validate form before change wizard step
			var validator = _validations[wizard.getStep() - 1]; // get validator for currnt step
			
			if (validator) {
				
				let level = $('input[name="MemberCardLevelID"]').val().trim()
				let step = wizard.getNewStep()
				if (level == "level1" && step == 3) {
					if (isGetCusData && ($('input[name="FullName"]').val() == '' || $('input[name="Phone"]').val() == '')) {
						toastr.error('Vui lòng nhập tên và số điện thoại khách hàng!', "Lỗi!")
					} else {
						SetReviewStep()
						wizard.goTo(3);
                    }
					
				} else {
					validator.validate().then(function (status) {
						if (status == 'Valid') {
							let nextStep = wizard.getNewStep()
							
							if (nextStep == 2) {
								/// kiểm tra thông tin
								if ($("input[name='radiospay']:checked").val() == 2 && $("input[name='Paycode']").val() == "") {
									$('#PaycodeNoti').show()
									toastr.error($('#PaycodeNoti').text(), "Lỗi!")
									KTUtil.scrollTop();
									return
								} else
									if ($("input[name='radiospay']:checked").val() == 1 && ($("input[name='CusMoney']").val() == "" || $("input[name='ChangeMoney']").val() == "")) {
										$('#CusmoneyNoti').show()
										toastr.error($('#CusmoneyNoti').text(), "Lỗi!")
										KTUtil.scrollTop();
										return
									}
								if (level == "level1") {
									$('input[name="FullName"]').attr("readonly", false);
									$('input[name="Phone"]').attr("readonly", false);
									isGetCusData = true
								} else {
									$('input[name="FullName"]').attr("readonly", true);
									$('input[name="Phone"]').attr("readonly", true);
									isGetCusData = false
                                }




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
				
			}
			///
			function SetReviewStep() {
				if ($('input[name = "AccountName"]').val() == "") {
					$('#NameContain').hide()
				} else {
					$('input[name = "FullNameReview"]').val($('input[name = "FullName"]').val())
					$('#NameContain').show()
                }
				$('input[name = "LevelNameReview"]').val($('input[name = "LevelName"]').val())
				let rate = Number($('input[name = "RewardRate"]').val())
				let money = Number($('input[name = "MoneyPay"]').val().replaceAll(",", ""))
				let moneyReward = rate != 0 ? money * rate / 100 : money
				if (promotionInfo != null && promotionInfo.Type == 2) {
					moneyReward += (money * promotionInfo.VoucherDiscount/100)
                }
				
				$('input[name = "Money"]').val(moneyReward.toLocaleString('en-US'))
				$('input[name = "Point"]').val($('input[name = "PointPlus"]').val())
			}
			//function SkipInfoStep(nextStep) {
			//	isWelcome = Number($('input[name = "MoneyPay"]').val().replaceAll(',','')) < 3000000
			//	return isWelcome ? 3 : nextStep
			//}
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
						AddMemberCard()
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
				url: "/Admin/MemberCard/Create",
                data: data,
                datatype: "json",
                success: function (result) {
                    if (result.status == "Success") {

                    
						toastr.success('Thành công!')
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
						let cardcode = $('input[name="CardID"]').val()
						Print(data, result.userID, result.sp, result.userName, "PHIEU CAP THE", result.cashier, paytype, result.member, cusMoney, changeMoney, promotiondes, cardcode);
						Print(data, result.userID, result.sp, result.userName, "PHIEU CAP THE", result.cashier, paytype, result.member, cusMoney, changeMoney, promotiondes, cardcode);
                    } else {
                       
                        toastr.error(data.message,'Lỗi')
                    }
                },
                error: function () {
                    toastr.error('Lỗi')
                }
            })
        
        }
	}
	function SaveReportCreateCard(data, userID, userName) {
		var idCard = "";
		
		$.each(data, function (k, v) {
			if (v.name == "CardID") {
				idCard = v.value
			}
		})
		$.ajax({
			type: "POST",
			url: "/MemberCard/SaveReportCreateCard",
			data: {
				userID: userID, idCard: idCard
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
			_formEl = KTUtil.getById('createMemberCardForm');

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
	let isScanStep = $('[data-wizard-state="current"]').first().data('step') == 4
	if (isScanStep) {
		$('#CheckCardBtn').trigger("click");
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
				
				$('input[name="MemberCardLevelID"]').val(data.data.CardLevelID)
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
				$('input[name="MoneyPay"]').val('')
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
		let	$this = $(this)
		$this.hide();
		$('#loadingBtn').show()
		$("#iconStatus").removeClass()
		GetCurrCard($this, $('input[name="LevelName"]').val())
		$('input[name="CardID"]').val("")
		$('#iconStatus').show()
	});

}
function GetCurrCard($this,level) {
	$.ajax({
		type: "GET",
		url: "/MemberCard/GetCurrentCardForCreate",
		data: { level },

		datatype: 'json',
		complete: function () {
			$this.show();
			$('#loadingBtn').hide()
		
        },
		success: function (data) {
			if (data.status == "Success") {
				$('#iconStatus').addClass("flaticon2-check-mark text-success");
				$('#textNoti').text("Thẻ hợp lệ!");
				$('input[name="CardID"]').val(data.card)
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

		if (promotionInfo != null && promotionInfo.Type == 1 && $(this).val() != promotionInfo.ReceiveMoney.toLocaleString('en-US')) {
		   $('input[name="Promotion"],input[name="PromotionDes"],input[name="CusMoney"],input[name="ChangeMoney"]').val("")
			$('input[name="PromotionMoney"]').val(0)
			promotionInfo = null
			
		}
		if (promotionInfo != null && promotionInfo.Type == 2 && Number($(this).val().replaceAll(',','')) < promotionInfo.MinimumMoney) {
		   $('input[name="Promotion"],input[name="PromotionDes"],input[name="CusMoney"],input[name="ChangeMoney"]').val("")
			$('input[name="PromotionMoney"]').val(0)
			promotionInfo = null
			
		}
		
		let money = $(this).val().replaceAll(',', '')
		
		GetMemberCardLevel(money)
	});
	$('input[name="MoneyPay"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();

			if (promotionInfo != null && promotionInfo.Type == 1 && $(this).val() != promotionInfo.ReceiveMoney.toLocaleString('en-US')) {
				$('input[name="Promotion"],input[name="PromotionDes"],input[name="CusMoney"],input[name="ChangeMoney"]').val("")
				$('input[name="PromotionMoney"]').val(0)
				promotionInfo = null

			}
			// Xử lý logic khi nhấn phím Enter
			let money = $(this).val().replaceAll(',', '')

			GetMemberCardLevel(money)
		}
	});
	
	/// tiền thừa
	$('input[name="CusMoney"]').on('keypress', function (event) {
		if (event.which === 13) {
			event.preventDefault();
			// Xử lý logic khi nhấn ngoài input
			let money = Number($('input[name="MoneyPay"]').val().replaceAll(',', ''))
			let cusMoney = Number($(this).val().replaceAll(',', ''))
			
			if ($(this).val() == '')
				return
			if (promotionInfo != null && promotionInfo.Type == 1 && $(this).val() == 0) {
				$('input[name="ChangeMoney"]').val(0)
				return
			}
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

	//	if ($(this).val() == '')
	//		return
	//	if (promotionInfo != null && promotionInfo.Type == 1 && $(this).val() == 0) {
	//		$('input[name="ChangeMoney"]').val(0)
	//		return
	//	}
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
					promotionInfo = promotion
					$('input[name="MoneyPay"]').val(promotion.ReceiveMoney.toLocaleString('en-US'))
					$('input[name="CusMoney"],input[name="ChangeMoney"]').val(0)
					var e = $.Event("keypress", { which: 13 });
					$('input[name="MoneyPay"],input[name="CusMoney"],input[name="ChangeMoney"]').trigger(e);
					$('input[name="Promotion"]').attr('disabled', true)


				}
				if (promotion.Type == 2) {
					//Nhập số tiền thẻ tặng
					let moneypay = $('input[name="MoneyPay"]').val()
					if (moneypay == '' || Number(moneypay.replaceAll(',', '')) < promotion.MinimumMoney) {
						toastr.error(`Số tiền nạp phải đạt mức tối thiểu ${promotion.MinimumMoney.toLocaleString('en-US')} để nhận ưu đãi này!`, "Lỗi!")
						return
                    }
					promotionInfo = promotion


				}

			
				$('input[name="PromotionDes"]').val(promotion.Des)
				toastr.success(`Áp dụng thành công mã ${promotion.Des}!`)
				
			} else {
				toastr.error(data.message, "Lỗi!")
				promotionInfo = null
				$('input[name="Promotion"]').attr('disabled', true)

			}


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo
			toastr.error("Lỗi!")
		}
	})
}