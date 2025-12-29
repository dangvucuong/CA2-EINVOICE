export const readMoney = (amount: any): string => {
  var ones = [
    "",
    "một",
    "hai",
    "ba",
    "bốn",
    "năm",
    "sáu",
    "bảy",
    "tám",
    "chín",
  ];
  var teens = [
    "mười",
    "mười một",
    "mười hai",
    "mười ba",
    "mười bốn",
    "mười lăm",
    "mười sáu",
    "mười bảy",
    "mười tám",
    "mười chín",
  ];
  var tens = [
    "",
    "mười",
    "hai mươi",
    "ba mươi",
    "bốn mươi",
    "năm mươi",
    "sáu mươi",
    "bảy mươi",
    "tám mươi",
    "chín mươi",
  ];
  var scales = ["", "ngàn", "triệu", "tỷ", "nghìn tỷ", "triệu tỷ"];

  var words = "";

  var num = parseInt(amount, 10);

  if (isNaN(num)) {
    return "Số tiền không hợp lệ.";
  }

  if (num === 0) {
    return "Không đồng";
  }

  var scaleIndex = 0;

  while (num > 0) {
    var numChunk = num % 1000;
    if (numChunk !== 0) {
      var chunkWords = "";
      var hundreds = Math.floor(numChunk / 100);
      var tensUnits = numChunk % 100;

      if (hundreds !== 0) {
        chunkWords += ones[hundreds] + " trăm ";
      }

      if (tensUnits !== 0) {
        if (tensUnits < 10) {
          chunkWords += ones[tensUnits];
        } else if (tensUnits < 20) {
          chunkWords += teens[tensUnits - 10];
        } else {
          var tensDigit = Math.floor(tensUnits / 10);
          var unitsDigit = tensUnits % 10;
          chunkWords += tens[tensDigit];
          if (unitsDigit !== 0) {
            chunkWords += " " + ones[unitsDigit];
          }
        }
      }

      chunkWords += " " + scales[scaleIndex];
      words = chunkWords + " " + words;
    }

    scaleIndex++;
    num = Math.floor(num / 1000);
  }

  return words.trim();
};
