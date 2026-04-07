import moment from "moment";

export const isNotEmpty = (value: string | undefined | null): boolean => {
  return value !== undefined && value !== null && value !== "";
};
export const getFirstDayOfMonth = (): string => {
  const today = moment();
  const firstDayOfMonth = today.startOf("month");
  return firstDayOfMonth.format("YYYY-MM-DD");
};
export const getLastDayOfMonth = (): string => {
  const today = moment();
  const endDayOfMonth = today.endOf("month");
  return endDayOfMonth.format("YYYY-MM-DD");
};
export const numberWithCommas = (x: any) => {
  return x?.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ") ?? "";
};

export function parseSoapResponse(soapXmlString: string) {
  const parser = new DOMParser();
  const xmlDoc = parser.parseFromString(soapXmlString, "text/xml");

  const resultNode = Array.from(xmlDoc.getElementsByTagName("*")).find((node) =>
    node.nodeName.endsWith("Result"),
  );

  if (!resultNode || !resultNode.textContent) {
    return null;
  }

  const jsonText = resultNode.textContent.trim();
  return JSON.parse(jsonText);
}

export function formatXml(xmlString: string): string {
  let formatted = "";
  const reg = /(>)(<)(\/*)/g;
  xmlString = xmlString.replace(reg, "$1\r\n$2$3"); // thêm xuống dòng giữa thẻ
  let pad = 0;

  xmlString.split("\r\n").forEach((node) => {
    let indent = 0;
    if (node.match(/.+<\/\w[^>]*>$/)) {
      // thẻ mở & đóng trên cùng 1 dòng
      indent = 0;
    } else if (node.match(/^<\/\w/)) {
      // thẻ đóng
      if (pad !== 0) pad -= 1;
    } else if (node.match(/^<\w([^>]*[^/])?>.*$/)) {
      // thẻ mở
      indent = 1;
    } else {
      indent = 0;
    }

    const padding = new Array(pad + 1).join("  "); // 2 spaces
    formatted += padding + node + "\r\n";
    pad += indent;
  });

  return formatted.trim();
}

export function ConvertTienChu(
  number: number,
  loaiTien = "VND",
  addUnit = true,
): string {

  if (number === null || number === undefined) return "";
  const n = Number(number);
  if (isNaN(n)) return "";

  const chuSo = [
    "không","một","hai","ba","bốn",
    "năm","sáu","bảy","tám","chín",
  ];

  const hang = ["","nghìn","triệu","tỷ"];

  const currencyConfig: any = {
    VND:{ major:"đồng", minor:"", digits:0, useAnd:false },

    USD:{ major:"đô la mỹ", minor:"cent", digits:2, useAnd:true },
    EUR:{ major:"euro", minor:"cent", digits:2, useAnd:true },
    SGD:{ major:"đô la singapore", minor:"cent", digits:2, useAnd:true },
    AUD:{ major:"đô la úc", minor:"cent", digits:2, useAnd:true },
    CAD:{ major:"đô la canada", minor:"cent", digits:2, useAnd:true },
    CHF:{ major:"franc thụy sĩ", minor:"centime", digits:2, useAnd:true },

    GBP:{ major:"bảng anh", minor:"pence", digits:2, useAnd:false },
    CNY:{ major:"tệ", minor:"xu", digits:2, useAnd:false },

    JPY:{ major:"yên nhật", minor:"xu", digits:0, useAnd:false },
  };

  const cfg = currencyConfig[loaiTien] || currencyConfig["VND"];

 const read3 = (num:number, isHighest:boolean):string => {

  const tram = Math.floor(num/100);
  const chuc = Math.floor((num%100)/10);
  const dv = num%10;

  let s = "";

  // trăm
  if(tram>0){
    s += chuSo[tram]+" trăm";
  }
  else if(!isHighest && num>0){
    s += "không trăm";
  }

  // chục
  if(chuc>1){

    s += (s?" ":"")+chuSo[chuc]+" mươi";

   if(dv==1) s+=" mốt";
else if(dv==4) s+=" tư";
else if(dv==5) s+=" lăm";
else if(dv>0) s+=" "+chuSo[dv];

  }
  else if(chuc==1){

    s += (s?" ":"")+"mười";

    if(dv==5) s+=" lăm";
    else if(dv>0) s+=" "+chuSo[dv];

  }
  else if(dv>0){

    // FIX giống C#
    if(tram>0 || !isHighest)
      s += (s?" ":"")+"linh";

    s += " "+chuSo[dv];
  }

  return s.trim();
};


  const readInt = (num:number):string => {

    if(num==0) return "không";

    let groups:number[]=[];
    let tmp=num;

    while(tmp>0){
      groups.push(tmp%1000);
      tmp=Math.floor(tmp/1000);
    }

    let result:string[]=[];

    for(let i=groups.length-1;i>=0;i--){

      const block = groups[i];

      if(block==0) continue;

      const isHighest = (i==groups.length-1);

      const txt = read3(block,isHighest);

      result.push(
        (txt+" "+hang[i%4]).trim()
      );
    }

    return result.join(" ").replace(/\s+/g," ").trim();
  };


  const abs = Math.abs(n);

  const integerPart = Math.floor(abs);

  const decimalPart =
    Math.round((abs-integerPart)*100);

  let result =
    readInt(integerPart);

  if(addUnit && cfg.major)
    result += " "+cfg.major;

  if(decimalPart>0 && cfg.digits>0){

    const frac =
      readInt(decimalPart);

    const join =
      cfg.useAnd ? " và " : " ";

    result +=
      join + frac +
      (cfg.minor?" "+cfg.minor:"");
  }

  result =
    result.charAt(0).toUpperCase()
    + result.slice(1);

  if(n<0) result="Âm "+result;

  return result;
}

export const toIsoDateOrEmpty = (value: string) => {
  if (!value) return "";
  if (moment(value).isSame("1900-01-01", "day")) return "";
  return moment(value).format("YYYY-MM-DD");
};
