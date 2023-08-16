jQuery(document).ready(function () {
	
	
	InitLoadingButton()


})


function GetDetailCard() {
	let id = $('#OldCardID').val()
	$.ajax({
		type: "GET",
		url: "/MemberCard/GetDetailCard",
		data: {
			id
		},

		datatype: 'json',

		success: function (data) {
			if (data.status == "Success") {

				$('input[name="CardID"]').val(data.card.Code)

				$('input[name="CurrLevelName"]').val(data.card.LevelName)
				$('input[name="CurrLevelID"]').val(data.card.CardLevelID)
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
function Save() {
	let ID = $('#OldCardID').val()
	Swal.fire({
		title: 'Bạn có muốn làm mới thẻ này?',
		showDenyButton: false,
		showCancelButton: true,
		confirmButtonText: 'Có!',

		customClass: {
			actions: 'my-actions',
			cancelButton: 'order-1 right-gap',
			confirmButton: 'order-2',
			denyButton: 'order-3',
		}
	}).then((result) => {
		if (result.isConfirmed) {
			$.ajax({
				type: "POST",
				url: "/MemberCard/ClearMemberCard",
				data: {
					MemberCardID: ID,

				},

				datatype: 'json',

				success: function (data) {
					if (data.status == "Success") {
						toastr.success("Thành công!")
						location.reload();
					} else {
						toastr.error("Lỗi!")


					}


				},
				error: function () {
					// Xử lý lỗi (nếu cần thiết)
					// Ẩn loading indicator và cho phép lấy dữ liệu tiếp theo

				}
			})
		}
	})
	
}