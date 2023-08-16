
const RUTA_API = "http://localhost:8000";
const $listaDeImpresoras = document.querySelector("#listaDeImpresoras"),
    $btnRefrescarLista = document.querySelector("#btnRefrescarLista")

const limpiarLista = () => {
    for (let i = $listaDeImpresoras.options.length; i >= 0; i--) {
        $listaDeImpresoras.remove(i);
    }
};

const obtenerListaDeImpresoras = () => {
    Impresora.getImpresoras()
        .then(listaDeImpresoras => {
            limpiarLista();
            listaDeImpresoras.forEach(nombreImpresora => {
                const option = document.createElement('option');
                option.value = option.text = nombreImpresora;
                $listaDeImpresoras.appendChild(option);
            })
        });
}



$btnRefrescarLista.addEventListener("click", obtenerListaDeImpresoras);

function PrintEndShiftMoney(cashiersName, sp, userName) {
    let impresora = new Impresora(RUTA_API);
    impresora.cut();
    impresora.cutPartial();
    impresora.setFontSize(3, 2);
    impresora.write("JP SPORT GAME\n");
    impresora.setFontSize(1, 1);
    impresora.write("TANG 7, GIGAMALL, 240 - 242 PHAM VAN DONG\n P HIEP BINH CHANH, Q THU DUC, TP HCM\n");
    impresora.write("SDT: \n");
    impresora.feed(1);
    impresora.setFontSize(2, 2);
    impresora.write("BAO CAO KET CA\n");
    impresora.feed(1);
    impresora.setFontSize(1, 1);
    impresora.setAlign("left");
    impresora.write("QUAY:");
    impresora.write(cashiersName + "\n");
    impresora.write("SO PHIEU:");
    impresora.write(sp.Id + "\n");
    impresora.write("THU NGAN:");
    impresora.write(userName.trim() + "\n");
    impresora.write("THOI GIAN THU:");
    impresora.write(formatDate(sp.CreateDate) + " " + getTime(sp.CreateDate + '') + "\n");
    impresora.write("CA:");
    impresora.write(GetShift(getTime(sp.CreateDate + '')) + "\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.write("SO TIEN VAO CA:");
    impresora.write(sp.FirstShiftMoney + "\n");
    impresora.write("SO TIEN KET CA:");
    impresora.write(sp.EndShiftMoney + "\n");
    impresora.write("SO TIEN THUC NHAN:");
    impresora.write(sp.RealMoneySale + "\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.setAlign("center");
    impresora.write("(BAO CAO KET QUA CHI TIET)\n");
    impresora.write("WIFI: JP SPORT GAME\nPASSWORD: tang7gigamall\n");
    impresora.feed(1);
    impresora.cut();
    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
    impresora.end()
        .then(valor => {
            location.reload();
        });
}

function PrintFirstShiftMoney(cashiersName, sp, userName) {
    let impresora = new Impresora(RUTA_API);
    impresora.cut();
    impresora.cutPartial();
    impresora.setFontSize(3, 2);
    impresora.write("JP SPORT GAME\n");
    impresora.setFontSize(1, 1);
    impresora.write("TANG 7, GIGAMALL, 240 - 242 PHAM VAN DONG\n P HIEP BINH CHANH, Q THU DUC, TP HCM\n");
    impresora.write("SDT: \n");
    impresora.feed(1);
    impresora.setFontSize(2, 2);
    impresora.write("BAO CAO DAU CA\n");
    impresora.feed(1);
    impresora.setFontSize(1, 1);
    impresora.setAlign("left");
    impresora.write("QUAY:");
    impresora.write(cashiersName + "\n");
    impresora.write("SO PHIEU:");
    impresora.write(sp.Id + "\n");
    impresora.write("THU NGAN:");
    impresora.write(userName.trim() + "\n");
    impresora.write("THOI GIAN THU:");
    impresora.write(formatDate(sp.CreateDate) + " " + getTime(sp.CreateDate + '') + "\n");
    impresora.write("CA:");
    impresora.write(GetShift(getTime(sp.CreateDate + ''))+"\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.write("SO TIEN VAO CA:");
    impresora.write(sp.FirstShiftMoney + "\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.setAlign("center");
    impresora.write("(BAO CAO KET QUA CHI TIET)\n");
    impresora.write("WIFI: JP SPORT GAME\nPASSWORD: tang7gigamall\n");
    impresora.feed(1);
    impresora.cut();
    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
    impresora.end()
        .then(valor => {
            location.reload();
        });
}


function Print(data, userID, sp, userName, name, cashier, paytype, member, cusmoney, changemoney, promotiondes,cardcode) {
    var MoneyPay = "",LevelNameReview=""
    $.each(data, function (k, v) {
        if (v.name == "MoneyPay") {
            MoneyPay = v.value
        }
        if (v.name == "LevelNameReview") {
            LevelNameReview = v.value
        }
    })
    let impresora = new Impresora(RUTA_API);
    impresora.cut();
    impresora.cutPartial();
    impresora.setFontSize(3, 2);
    impresora.write("JP SPORT GAME\n");
    impresora.setFontSize(1, 1);
    impresora.write("TANG 7, GIGAMALL, 240 - 242 PHAM VAN DONG\n P HIEP BINH CHANH, Q THU DUC, TP HCM\n");
    impresora.write("SDT: \n");
    impresora.feed(1);
    impresora.setFontSize(2, 2);
    impresora.write(name+"\n");
    impresora.feed(1);
    impresora.setFontSize(1, 1);
    impresora.setAlign("left");
    impresora.write("QUAY:");
    impresora.write(cashier + "\n");
    impresora.write("SO PHIEU:");
    impresora.write(sp.RecordID + "\n");
    impresora.write("NHAN VIEN:");
    impresora.write(userName.trim() + "\n");
    impresora.write("THOI GIAN THU:");
    impresora.write(formatDate(sp.ChargeDate) + " " + getTime(sp.ChargeDate+'') + "\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.write("SO TIEN:");
    impresora.write(MoneyPay + "\n");
    impresora.feed(1);
    impresora.write("LOAI THANH TOAN:");
    impresora.write(paytype + "\n");
    if (cusmoney != '') {
        impresora.write("SO TIEN KHACH TRA:");
        impresora.write(cusmoney + "\n");
        impresora.write("SO TIEN TRA LAI:");
        impresora.write(changemoney + "\n");
    }
    if (promotiondes != '') {
        impresora.write("KHUYEN MAI:");
        impresora.write(removeAccents(promotiondes) + "\n");
    }
    
    //impresora.write("SO TIEN (BANG CHU):");
    //impresora.write(to_vietnamese(MoneyPay.replaceAll(",", "")) + "\n");
    impresora.feed(1);
    impresora.write("===============================================\n");
    impresora.feed(1);
    impresora.write("CAP DO THE:");
    impresora.write(LevelNameReview + "\n");
    impresora.feed(1);
    impresora.write("MA THE:");
    impresora.write(cardcode + "\n");
    impresora.feed(1);
    if (member != "") {
        impresora.write("THANH VIEN:");
        impresora.write(member + "\n");
        impresora.feed(1);
    }
    impresora.setAlign("center");
    impresora.setFontSize(1, 1);
    impresora.write("(QUY KHACH KHI NAP 500K TRO LEN DUOC TANG VE\n");
    impresora.write("CHOI VR CANH TAY ROBOT MIEN PHI TAI TANG 1)\n");
    impresora.feed(1);
    impresora.write("*XIN CAM ON QUY KHACH VA HEN GAP LAI*\n");
    impresora.feed(1);
    impresora.write("WIFI: JP SPORT GAME\nPASSWORD: tang7gigamall\n");
    impresora.qr("00020101021138530010A0000007270123000697042801093123456660208QRIBFTTA53037045802VN6304D728");
    impresora.feed(1);
    impresora.cash();
    impresora.cut();
    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
    impresora.end()
        .then(valor => {
            location.reload();
        });
}


function PrintAccount(id,name) {
    let impresora = new Impresora(RUTA_API);
    impresora.cut();
    impresora.cutPartial();
    impresora.qr(id);
    impresora.feed(1);
    impresora.setFontSize(1, 1);
    impresora.setAlign("center");
    impresora.write(name);
    impresora.cash();
    impresora.cut();
    impresora.cutPartial(); // Pongo este y también cut porque en ocasiones no funciona con cut, solo con cutPartial
    impresora.end()
        .then(valor => {
            window.location.href = "/Admin/Administrator"
        });
}

// En el init, obtenemos la lista
obtenerListaDeImpresoras();
// Y también la impresora seleccionada

const defaultNumbers = ' hai ba bon nam sau bay tam chin';

const chuHangDonVi = ('1 mot' + defaultNumbers).split(' ');
const chuHangChuc = ('le muoi' + defaultNumbers).split(' ');
const chuHangTram = ('khong mot' + defaultNumbers).split(' ');

//--------------number convert text--------------
function convert_block_three(number) {
  
    if (number == '000') return '';
    var _a = number + ''; //Convert biến 'number' thành kiểu string

    //Kiểm tra độ dài của khối
    switch (_a.length) {
        case 0: return '';
        case 1: return chuHangDonVi[_a];
        case 2: return convert_block_two(_a);
        case 3:
            var chuc_dv = '';
            if (_a.slice(1, 3) != '00') {
                chuc_dv = convert_block_two(_a.slice(1, 3));
            }
           
            var tram = chuHangTram[_a[0]] + ' tram';
            return tram + ' ' + chuc_dv;
    }
}

function convert_block_two(number) {
    var dv = chuHangDonVi[number[1]];
    var chuc = chuHangChuc[number[0]];
    var append = '';

    // Nếu chữ số hàng đơn vị là 5
    if (number[0] > 0 && number[1] == 5) {
        dv = 'lam'
    }

    // Nếu số hàng chục lớn hơn 1
    if (number[0] > 1) {
        append = ' muoi';

        if (number[1] == 1) {
            dv = ' mot';
        }
    }

    return chuc + '' + append + ' ' + dv;
}
const dvBlock = '1 nghin trieu ty'.split(' ');

function to_vietnamese(number) {
    var str = parseInt(number) + '';
    var i = 0;
    var arr = [];
    var index = str.length;
    var result = [];
    var rsString = '';

    if (index == 0 || str == 'NaN') {
        return '';
    }

    // Chia chuỗi số thành một mảng từng khối có 3 chữ số
    while (index >= 0) {
        arr.push(str.substring(index, Math.max(index - 3, 0)));
        index -= 3;
    }

    // Lặp từng khối trong mảng trên và convert từng khối đấy ra chữ Việt Nam
    for (i = arr.length - 1; i >= 0; i--) {
        if (arr[i] != '' && arr[i] != '000') {
            result.push(convert_block_three(arr[i]));

            // Thêm đuôi của mỗi khối
            if (dvBlock[i]) {
                result.push(dvBlock[i]);
            }
        }
    }

    // Join mảng kết quả lại thành chuỗi string
    rsString = result.join(' ');

    // Trả về kết quả kèm xóa những ký tự thừa
    return rsString.replace(/[0-9]/g, '').replace(/ /g, ' ').replace(/ $/, '');
}