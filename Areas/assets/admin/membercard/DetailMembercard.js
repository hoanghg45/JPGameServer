jQuery(document).ready(function () {
	$('#AddMember').hide()
	if (CardID == null) {
		InitLoadingButton()
		var membercardHub = $.connection.membercardHub;
		console.log(membercardHub)
		membercardHub.client.notify = function (message) {
			function Click() {
				$('#CheckCardBtn').trigger("click");
			}

			if (message && message.toLowerCase() == "cardscanned" && ReaderID != null) {
				const throttledFunction = $.throttle(1000, Click)
				throttledFunction()
			}
		}
		$.connection.hub.start().done(function () {
			console.log('Hub started');
		});
		$('#scan').show()
		
	} else {
		$('#scan').hide()
		GetDetailCard(CardID)
    }
})

function InitLoadingButton() {
	$('#loadingBtn').hide()
	$('#iconStatus').hide()
	$('#CheckCardBtn').on('click', function () {
		let $this = $(this)
		$this.hide();
		$('#loadingBtn').show()
		$("#iconStatus").removeClass()
		GetCurrCard($this)
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

				$('input[name="CurrLevelName"]').val(data.card.LevelName)
				$('input[name="CurrGiftLevelName"]').val(data.card.GiftLevelName)
				$('input[name="CurrRewardRate"]').val(data.card.RewardRate)
				$('input[name="CurrPoints"]').val(data.card.Points)

				$('input[name="CurrBalance"]').val(data.card.Balance.toLocaleString('en-US'))
				
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
				if (data.card.isHaveOwner) {
					$('#UserInfor').show()
					$('input[name="FullName"]').val(data.card.Owner.FullName)
					$('input[name="DateOfBirth"]').val(formatDate(data.card.Owner.DateOfBirth))
					$('input[name="AccountName"]').val(data.card.Owner.UserName)
					$('input[name="Email"]').val(data.card.Owner.Email)
					$('input[name="Phone"]').val(data.card.Owner.Phone)
				} else {
					$('#UserInfor').hide()
					$('#AddMember').show()

                }

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
function GetDetailCard(id) {
	$.ajax({
		type: "GET",
		url: "/MemberCard/GetDetailCard",
		data: {
			id
        },

		datatype: 'json',
	
		success: function (data) {
			if (data.status == "Success") {
				$('#iconStatus').addClass("flaticon2-check-mark text-success");
				$('#textNoti').text("Thẻ hợp lệ!");
				$('input[name="CardID"]').val(data.card.Code)

				$('input[name="CurrLevelName"]').val(data.card.LevelName)
				$('input[name="CurrGiftLevelName"]').val(data.card.GiftLevelName)
				$('input[name="CurrRewardRate"]').val(data.card.RewardRate)
				$('input[name="CurrPoints"]').val(data.card.Points)

				$('input[name="CurrBalance"]').val(data.card.Balance.toLocaleString('en-US'))

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
				if (data.card.isHaveOwner) {
					$('#UserInfor').show()
					$('input[name="FullName"]').val(data.card.Owner.FullName)
					$('input[name="DateOfBirth"]').val(formatDate(data.card.Owner.DateOfBirth))
					$('input[name="AccountName"]').val(data.card.Owner.UserName)
					$('input[name="Email"]').val(data.card.Owner.Email)
					$('input[name="Phone"]').val(data.card.Owner.Phone)
				} else {
					$('#UserInfor').hide()
				}

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
function AddAccountForCard() {
	let accountID = $('#search_account').val()
	let cardID = $('input[name="CardID"]').val()
	$.ajax({
		type: "POST",
		url: "/MemberCard/AddAccountForCard",
		data: {
			accountID,
			cardID
		},

		datatype: 'json',

		success: function (data) {
			if (data.status == "Success") {
				toastr.success(
					'Thành công!',
					'Đã gán tài khoản',
					{
						timeOut: 1000,
						fadeOut: 1000,
						onHidden: function () {
							$(window).scrollTop(0);
							window.location.reload();
						}
					}
				);
			
			} else {
				toastr.error("Lỗi!")

				$('#textNoti').text(data.message);
				

			}


		},
		error: function () {
			// Xử lý lỗi (nếu cần thiết)
			// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo

		}
	})
}
