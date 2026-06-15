using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FastMember;
using static Dapper.SqlMapper;

namespace Common
{
    public static partial class myExtension
    {
        private static string pk = "kingsneverdie993";
        private static string BASE26 = "abcdefghijklmnopqrstuvwxyz";
        public static string ToDateRFC3987(this object obj)
        {
            try
            {
                var data = obj.ToDate();
                if (data.HasValue)
                {
                    return string.Empty;
                }
                else
                {
                    var dt = data.Value;
                    return string.Format("{0}-{1}-{2}",
                        dt.Year.ToString(),
                        dt.Month > 9 ? dt.Month.ToString() : ("0" + dt.Month.ToString()),
                        dt.Day > 9 ? dt.Day.ToString() : ("0" + dt.Day.ToString())
                        );

                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string ToNgayThangNamText(this DateTime obj)
        {
            String result = "";
            if (obj.Day < 10) { result = "ngày 0" + obj.Day.ToString(); } else { result = "ngày " + obj.Day.ToString(); }
            if (obj.Month < 10) { result += " tháng 0" + obj.Month.ToString(); } else { result += " tháng " + obj.Month.ToString(); }
            result += " năm " + obj.Year.ToString();
            return result;
        }
        public static int? ToInt32(this object obj)
        {
            try
            {

                return Int32.Parse(obj.ToString().RemoveAllSpace());
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static Double? ToDouble(this object obj)
        {
            try
            {
                return Double.Parse(obj.ToString());
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static float? ToFloat(this object obj)
        {
            try
            {
                return float.Parse(obj.ToString());
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static DateTime? ToDate(this object obj)
        {
            try
            {
                return DateTime.Parse(obj.ToString()).Date;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Boolean? ToBoolean(this object obj)
        {
            try
            {
                return (obj.ToString().ToUpper().Trim().Equals("TRUE") || obj.ToString().Trim().Equals("1")) ? true : false;
            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// Convert ra chữ không dấu
        /// </summary>
        /// <param name="chucodau"></param>
        /// <returns></returns>
        public static string ConvertToNoAccents(this string input)
        {
            string FindText = " áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            string ReplText = "_aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";
            int index = -1;
            char[] arrChar = FindText.ToCharArray();
            while ((index = input.IndexOfAny(arrChar)) != -1)
            {
                int index2 = FindText.IndexOf(input[index]);
                input = input.Replace(input[index], ReplText[index2]);
            }
            return input;
        }
        /// <summary>
        /// Bỏ hết khoảng trắng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveAllSpace(this string input)
        {
            do
            {
                input = input.Replace(" ", "");
            } while (input.IndexOf(" ") >= 0);
            return input;
        }
        /// <summary>
        /// Bỏ hết khoảng trắng liền nhau, thành 1 khoảng trắng thôi
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveAllDoubleSpace(this string input)
        {
            do
            {
                input = input.Replace("  ", " ");
            } while (input.IndexOf("  ") >= 0);
            return input;
        }

        /// <summary>
        /// Convert ra số thứ thự: 01, 02 , 03 ...
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertToSoThuTu(this object input)
        {
            try
            {
                if (input == null) return "00";
                var data = input.ToInt32();
                if (data.HasValue)
                {
                    return $"{(data.Value < 10 ? "0" : "")}{data.Value}";
                }
                return "00";
            }
            catch
            {
                return "00";
            }
        }
        public static int ConvertToInt(this object input)
        {
            try
            {
                if (input == null) return 0;
                var data = input.ToInt32();
                if (data.HasValue)
                {
                    return data.Value;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public static string ConvertToString(this object input)
        {
            try
            {
                if (input != null) return input.ToString();
                return string.Empty;
            }
            catch
            {

                return String.Empty;
            }
        }

        public static string GetTen(this string hoTen)
        {
            try
            {
                var arr = hoTen.ConvertToString().RemoveAllDoubleSpace().Trim().Split(" ");
                return arr[arr.Length - 1].ConvertToString();
            }
            catch
            {

                return String.Empty;
            }
        }
        public static string GetTenDem(this string hoTen)
        {
            try
            {
                var arr = hoTen.ConvertToString().RemoveAllDoubleSpace().Trim().Split(" ");
                var result = new List<string>();
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    result.Add(arr[i]);
                }
                return string.Join(" ", result);
            }
            catch
            {

                return String.Empty;
            }
        }


        public static bool isDate(this object date)
        {
            DateTime Temp;
            if (date == null) return false;
            if (DateTime.TryParse(date.ToString(), out Temp) == true)
                return true;
            else
                return false;
        }
        public static Double ConvertToDouble(this object input, int lenght = 2)
        {
            try
            {
                if (input == null) return 0;
                var data = input.ToDouble();
                if (data.HasValue)
                {
                    return Math.Round(data.Value, lenght, MidpointRounding.AwayFromZero);
                }
                return 0;
            }
            catch
            {

                return 0;
            }
        }
        public static string FormatAsHoTen(this string value)
        {
            try
            {
                var temp = String.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                while (temp.Contains("  "))
                {
                    temp = temp.Replace("  ", " ");
                }
                var words = temp.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].ToLower();
                    if (words[i].Length == 1)
                    {
                        words[i] = words[i].ToUpper();
                    }
                    if (words[i].Length > 1)
                    {
                        words[i] = words[i].Substring(0, 1).ToUpper() + words[i].Substring(1, words[i].Length - 1);
                    }
                }
                return String.Join(" ", words);
            }
            catch
            {
                return value;
            }
        }

        public static DateTime? MapDateFromddMMYYY(this string ddMMYYYY)
        {
            try
            {
                var datas = ddMMYYYY.Split('/');
                if (datas.Length >= 3)
                {
                    var dd = datas[0].ConvertToInt();
                    var mm = datas[1].ConvertToInt();
                    var yyyy = datas[2].ConvertToInt();
                    return new DateTime(yyyy, mm, dd);
                }
                datas = ddMMYYYY.Split('-');
                if (datas.Length >= 3)
                {
                    var dd = datas[0].ConvertToInt();
                    var mm = datas[1].ConvertToInt();
                    var yyyy = datas[2].ConvertToInt();
                    return new DateTime(yyyy, mm, dd);
                }
                return null;
            }
            catch
            {

                return null;
            }
        }
        public static Boolean ConvertToBoolean(this object input)
        {
            try
            {
                if (input == null) return false;
                var data = input.ToBoolean();
                if (data.HasValue)
                {
                    return data.Value;
                }
                return false;
            }
            catch
            {

                return false;
            }
        }
        // public class CustomXmlWriter : XmlTextWriter
        // {
        //     public CustomXmlWriter(Stream stream, Encoding encoding) : base(stream, encoding) { }

        //     public override void WriteString(string text)
        //     {
        //         if (decimal.TryParse(text, out decimal decimalValue))
        //         {
        //             text = decimalValue.ToString("0.######");
        //         }
        //         base.WriteString(text);
        //     }
        // }
        public static string ConvertToXml<T>(this T obj, bool indent = false, NewLineHandling newLineHandling = NewLineHandling.None)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = indent,    // Đặt Indent = true nếu  muốn làm đẹp XML
                NewLineHandling = newLineHandling, // Không thêm dòng mới
                Encoding = new UTF8Encoding(),
            };
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            // using (var stringWriter = new StringWriter())
            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    // Chuyển đổi đối tượng thành XML
                    serializer.Serialize(xmlWriter, obj, namespaces);

                    // Lấy chuỗi XML từ StringWriter
                    // var xmlString = stringWriter.ToString();
                    var xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());

                    return xmlString;
                }



            }

        }
        public static string ConvertToXmlAndAppendChild<T>(this T obj,
            string parentNode,
            string childNode,
            string childNodeValue,
            bool indent = false,
            NewLineHandling newLineHandling = NewLineHandling.None,
            bool isAddCKSNNT = false,
            string AttributeId = ""
        )
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = indent,    // Đặt Indent = true nếu  muốn làm đẹp XML
                NewLineHandling = newLineHandling, // Không thêm dòng mới
                Encoding = new UTF8Encoding(false),
            };
            // Tạo một đối tượng XmlSerializerNamespaces để bỏ qua namespace mặc định
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            // using (var stringWriter = new StringWriter())

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    // Chuyển đổi đối tượng thành XML
                    serializer.Serialize(xmlWriter, obj, namespaces);

                    // Lấy chuỗi XML từ StringWriter
                    // var xmlString = stringWriter.ToString();
                    var xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
                    var doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlElement rootElement = (XmlElement)doc.SelectSingleNode(parentNode);

                    XmlElement fileElement = doc.CreateElement(childNode);
                    // fileElement.Attributes.ad
                    if (AttributeId != string.Empty)
                    {
                        fileElement.SetAttribute("Id", AttributeId);
                    }
                    fileElement.InnerXml = childNodeValue.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                    fileElement.InnerXml = childNodeValue.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

                    rootElement.AppendChild(fileElement);
                    if (isAddCKSNNT)
                    {
                        XmlElement TDiep = (XmlElement)doc.SelectSingleNode("TDiep");
                        XmlElement fileElementCKSNNT = doc.CreateElement("CKSNNT");
                        TDiep.AppendChild(fileElementCKSNNT);

                    }
                    var xml = doc.OuterXml;
                    return xml;
                }
            }


        }
        public static string ConvertToXmlAndAppendChilds<T>(this T obj,
            string parentNode,
            string childNode,
            List<string> childNodeValues,
            bool indent = false,
            NewLineHandling newLineHandling = NewLineHandling.None,
            bool isAddCKSNNT = false,
            string AttributeId = ""
      )
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = indent,    // Đặt Indent = true nếu  muốn làm đẹp XML
                NewLineHandling = newLineHandling, // Không thêm dòng mới
                Encoding = new UTF8Encoding(false),
            };
            // Tạo một đối tượng XmlSerializerNamespaces để bỏ qua namespace mặc định
            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            // using (var stringWriter = new StringWriter())

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    // Chuyển đổi đối tượng thành XML
                    serializer.Serialize(xmlWriter, obj, namespaces);

                    // Lấy chuỗi XML từ StringWriter
                    // var xmlString = stringWriter.ToString();
                    var xmlString = Encoding.UTF8.GetString(memoryStream.ToArray());
                    var doc = new XmlDocument();
                    doc.LoadXml(xmlString);
                    XmlElement rootElement = (XmlElement)doc.SelectSingleNode(parentNode);

                    XmlElement fileElement = doc.CreateElement(childNode);
                    // fileElement.Attributes.ad
                    if (AttributeId != string.Empty)
                    {
                        fileElement.SetAttribute("Id", AttributeId);
                    }
                    foreach (var childNodeValue in childNodeValues)
                    {
                        var childXml = childNodeValue.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "")
                        .Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        fileElement.InnerXml += childXml;
                    }
                    rootElement.AppendChild(fileElement);
                    if (isAddCKSNNT)
                    {
                        XmlElement TDiep = (XmlElement)doc.SelectSingleNode("TDiep");
                        XmlElement fileElementCKSNNT = doc.CreateElement("CKSNNT");
                        TDiep.AppendChild(fileElementCKSNNT);

                    }
                    var xml = doc.OuterXml;
                    return xml;
                }
            }


        }
        public static T ConvertFromBase64<T>(this string base64String)
        {
            // Convert Base64 to bytes
            byte[] byteArray = Convert.FromBase64String(base64String);

            string jsonString = Encoding.UTF8.GetString(byteArray);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);

        }
        public static string ConvertToXmlFromBase64(this string base64String)
        {
            // Convert Base64 to bytes
            byte[] bytes = Convert.FromBase64String(base64String);

            // Convert bytes to XML string
            string xmlString = Encoding.UTF8.GetString(bytes);

            // Load XML string into an XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            return xmlDoc.OuterXml;
        }
        public static T ConvertToObject<T>(this string xmlString, bool isAdNameSpace = false)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                if (isAdNameSpace)
                {
                    // Khai báo namespace và thêm nó vào XmlSerializerNamespaces
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
                }
                using (TextReader reader = new StringReader(xmlString))
                {
                    T root = (T)serializer.Deserialize(reader);
                    return root;
                }
            }
            catch (System.Exception ex)
            {
                return default(T);
            }
        }

        public static string ConvertToBase64(this string input)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                string base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
            catch
            {

                return "";
            }
        }
        public static decimal ConvertToDecimal(this object input)
        {
            try
            {
                if (input == null) return 0;
                if (input.ToString() == "") return 0;
                return decimal.Parse(input.ToString());
            }
            catch
            {
                return 0;
            }
        }
        public static string ConvertToStringAndRemoveZeroPart(this decimal input)
        {
            try
            {
                if (input == null) return "0";
                return input.ToString("0.######");
            }
            catch
            {
                return "0";
            }
        }
        public static float ConvertToFloat(this object input)
        {
            try
            {
                if (input == null) return 0;
                var data = input.ToFloat();
                if (data.HasValue)
                {
                    return data.Value;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }
        public static DateTime ConvertUnixTimeStampToDateTime(this double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            if (unixTimeStamp > (new DateTime().Date).ConvertDateToUnixTimeStamp())
            {
                unixTimeStamp = Math.Floor(unixTimeStamp / 1000);
            }
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public static double ConvertDateToUnixTimeStamp(this DateTime input)
        {
            return (int)input.Subtract(input.Date).TotalSeconds;
        }
        public static DateTime? ConvertToDate(this string ddMMyyyy)
        {
            try
            {

                if (ddMMyyyy.ConvertToString().Length == 10)
                {
                    var dd = ddMMyyyy.Substring(0, 2).ConvertToInt();
                    var MM = ddMMyyyy.Substring(3, 2).ConvertToInt();
                    var yyyy = ddMMyyyy.Substring(6, 4).ConvertToInt();
                    return new DateTime(yyyy, MM, dd);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        #region "Doc so thanh chu"
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))

                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";
            }
            return Ktach;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";
            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";
            return Kdonvi;

        }
        private static string LoaiTienText(this string loaiTien)
        {
            var loaiTienText = "đồng";
            switch (loaiTien.ToUpper())
            {
                case "USD":
                    loaiTienText = "đô la mỹ";
                    break;
                case "EUR":
                    loaiTienText = "ơ-rô";
                    break;


            }
            return loaiTienText;
        }

        private static string DocSoThanhChu(double gNum, string loaiTien = "VND")
        {
            if (loaiTien != "VND")
            {
                var result = Common.ReadNumberWebSerivce.DocSoAsync((decimal)gNum, loaiTien).GetAwaiter().GetResult();
                return result;
            }
            return NumberToWords.ConvertToVietnameseWords((long)gNum.ConvertToDouble(0));
            //var rd = new ReadNumber();
            //var sotienbangchu = rd.DocSo(gNum.ToString().Replace(".", ","), loaiTien);
            //return sotienbangchu;
            // if (loaiTien != "VND")
            // {
            //     return NumberToWordsConverter.ConvertToWords(gNum.ConvertToDecimal(), loaiTien);
            // }
            if (gNum == 0)
                return $"Không {loaiTien.LoaiTienText()}";
            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";
            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";
            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();
            ///don vi hang mod 
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai
            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";
            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);
                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + $" {loaiTien.LoaiTienText()}.";
            return lso_chu.ToString().Trim();
        }
        // public static string ConvertToText(this double gNum, string loaiTien = "VND")
        // {
        //     return DocSoThanhChu(gNum, loaiTien);
        // }
        public static Task<string> ConvertToTextAsync(this double gNum, string loaiTien = "VND")
        {
            return Common.ReadNumberWebSerivce.DocSoAsync((decimal)gNum, loaiTien);
        }
        public static Task<string> ConvertToTextAsync(this decimal gNum, string loaiTien = "VND")
        {
            return Common.ReadNumberWebSerivce.DocSoAsync(gNum, loaiTien);
        }
        // public static string ConvertToText(this int gNum, string loaiTien = "VND")
        // {
        //     return DocSoThanhChu(gNum, loaiTien);
        // }
        public static string ConvertToText(this float gNum, string loaiTien = "VND")
        {
            return DocSoThanhChu(gNum, loaiTien);
        }
        public static int ConvertToThu(this DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            if ((int)dayOfWeek == 0) return 8;
            return (int)dayOfWeek + 1;
        }
        #endregion

        #region "Đọc thứ"
        public static string Doc_thu(int thu)
        {
            switch (thu)
            {
                case 0:
                    return "Hai";
                case 1:
                    return "Ba";
                case 2:
                    return "Bốn";
                case 3:
                    return "Năm";
                case 4:
                    return "Sáu";
                case 5:
                    return "Bảy";
                case 6:
                    return "Chủ nhật";
                default:
                    return "";
            }
        }
        #endregion



        public static void AddValue(this ExpandoObject obj, string propertyName, Object propertyValue)
        {
            //var expandoDict = obj as IDictionary;
            var expandoDict = obj as IDictionary<string, Object>;
            if (expandoDict.ContainsKey(propertyName))
            {
                expandoDict[propertyName] = propertyValue;
            }
            else
            {
                expandoDict.Add(propertyName, propertyValue);
            }

        }
        public static Object GetProperty(ExpandoObject obj, string propertyName)
        {
            var expandoDict = obj as IDictionary<string, Object>;
            if (expandoDict.ContainsKey(propertyName))
            {
                return expandoDict[propertyName];
            }
            else
            {
                return null;
            }
        }
        public static Object GetValue(this ExpandoObject obj, string propertyName)
        {
            var expandoDict = obj as IDictionary<string, Object>;
            if (expandoDict.ContainsKey(propertyName))
            {
                return expandoDict[propertyName];
            }
            else
            {
                return null;
            }
        }
        public static object GetPropValue(this object src, string propName)
        {
            var value = src?.GetType()?.GetProperty(propName)?.GetValue(src, null);
            return value;
        }
        public static void SetPropValue(this object src, string propName, object value)
        {
            var property = src?.GetType()?.GetProperty(propName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(src, value);
            }
            else
            {
                throw new ArgumentException($"Property '{propName}' not found or not writeable.");
            }
        }
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
        #region "MD5"
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
        public static string ToMD5(this string input)
        {
            return myExtension.CreateMD5(input);
        }
        public static string ToMD5WithPK(this string input)
        {
            input += pk;
            return myExtension.CreateMD5(input);
        }
        public static string CreateRandom()
        {
            string[] a = { "q", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "g", "h", "k", "l", "x", "c", "v", "b", "n", "m" };
            Random rd = new Random();
            var password = a[rd.Next(0, a.Length - 1)] + a[rd.Next(0, a.Length - 1)] + a[rd.Next(0, a.Length - 1)] + rd.Next(1, 9).ToString() + rd.Next(1, 9).ToString() + rd.Next(1, 9).ToString();
            return password;
        }
        #endregion


        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Not Found";
        }




        public static T Map<T>(this object model)
        where T : new()
        {
            var objBase = new T();
            var type = objBase.GetType();
            var props = (IList<PropertyInfo>)type.GetProperties();
            var model_props = (IList<PropertyInfo>)model.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (model_props.Where(x => x.Name == prop.Name).Count() > 0)
                {
                    var item = objBase.GetType().GetProperty(prop.Name);
                    if (item.CanWrite)
                        item.SetValue(objBase, model.GetType().GetProperty(prop.Name).GetValue(model));
                }

            }
            return objBase;
        }
        public static IEnumerable<T> MapList<X, T>(this IEnumerable<X> list)
        where T : new()
        {
            var result = new List<T>();
            foreach (var item in list)
            {
                result.Add(item.Map<T>());
            }
            return result;
        }

        public static TimeSpan? ToTimeSpan(this string hh_mm)
        {
            if (hh_mm.Split(':').Length == 2)
            {
                var hh = hh_mm.Split(':')[0].ConvertToInt();
                var mm = hh_mm.Split(':')[1].ConvertToInt();
                return new TimeSpan(hh, mm, 00);
            }
            return null;
        }
        public static string ConvertToHHmm(this TimeSpan ts)
        {
            try
            {
                var HH = ts.Hours < 10 ? "0" + ts.Hours.ToString() : ts.Hours.ToString();
                var mm = ts.Minutes < 10 ? "0" + ts.Minutes.ToString() : ts.Minutes.ToString();
                return string.Format("{0}:{1}", HH, mm);
            }
            catch
            {

                throw;
            }
        }

        public static List<int> ConvertToList(this string input, char character = ',')
        {
            var arr = input.Split(character);
            var result = new List<int>();
            foreach (var item in arr)
            {
                result.Add(item.ConvertToInt());
            }
            return result;
        }
        public static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            try
            {
                results = new List<ValidationResult>();

                return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
            }
            catch (Exception)
            {
                results = new List<ValidationResult>();
                return false;
            }
        }
        public static bool Validate<T>(T obj, out string mesage)
        {
            ICollection<ValidationResult> results = null;
            var isValid = Validate(obj, out results);
            if (isValid)
            {
                mesage = string.Empty;
                return true;
            }
            else
            {
                mesage = String.Join("\n", results.Select(o => o.ErrorMessage));
                return false;
            }
        }
        public static bool CheckValidation<T>(this T obj, out string mesage)
        {
            return Validate(obj, out mesage);
        }
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> data)
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data))
            {
                table.Load(reader);
            }
            return table;
        }
        public static DataTable ConvertToDataTable(this IEnumerable<string> data)
        {
            if (data == null)
            {
                return null;
            }
            DataTable table = new DataTable();
            table.Columns.Add("key", typeof(string));
            foreach (var item in data)
            {
                table.Rows.Add(item);
            }
            return table;
        }
        public static DataTable ConvertToDataTable(this IEnumerable<int> data)
        {
            if (data == null)
            {
                return null;
            }
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            foreach (var item in data)
            {
                table.Rows.Add(item);
            }
            return table;
        }
        public static ICustomQueryParameter ConvertToTableValuedParameter(this DataTable dt, string typeName = "Ids")
        {
            return dt.AsTableValuedParameter(typeName);
        }

        public static ICustomQueryParameter ConvertToTableValuedParameter<T>(this IEnumerable<T> source, string typeName = "Ids")
        {
            return source.ConvertToDataTable().AsTableValuedParameter(typeName);
        }

        public static ICustomQueryParameter ConvertToTableValuedParameter(this IEnumerable<int> source, string typeName = "Ids")
        {
            return source.ConvertToDataTable().AsTableValuedParameter(typeName);
        }
        public static ICustomQueryParameter ConvertToTableValuedParameter(this IEnumerable<string> source, string typeName = "OtherKeys")
        {
            return source.ConvertToDataTable().AsTableValuedParameter(typeName);
        }
        public static object TryDeserializeObject(this string content)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            }
            catch
            {
                return null;
            }
        }
        public static T TryDeserializeObject<T>(this string content)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            }
            catch
            {
                return default(T);
            }
        }
        public static string Join(this IEnumerable<int> list, string separator = ",")
        {
            return string.Join(separator, list);
        }
        public static string Join(this IEnumerable<string> list, string separator = ",")
        {
            return string.Join(separator, list);
        }
        public static List<T> DistinctValues<T>(this DataTable dt, string column_name)
        {
            try
            {
                if (!dt.Columns.Contains(column_name)) return new List<T>();
                var dv = dt.DefaultView;
                var dtDistinctValue = dv.ToTable(true, column_name);
                var result = new List<T>();
                for (int i = 0; i < dtDistinctValue.Rows.Count; i++)
                {
                    result.Add((T)dtDistinctValue.Rows[i][column_name]);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }
        public static string SerializeToXml<T>(this T list)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (StringWriter sw = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true }))
                    {
                        serializer.Serialize(xmlWriter, list);
                        return sw.ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                return string.Empty;
            }
        }
        public static string CreateRandomAlphabetString(int lenght = 6)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < lenght; i++)
            {
                char c = (char)('A' + random.Next(0, 26)); // Tạo một ký tự từ A đến Z
                sb.Append(c);
            }

            string randomString = sb.ToString();
            return randomString;
        }
        public static string CreateMaTraCuu(int length = 8)
        {

            string CHARS = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0");

            var result = new char[length];
            var buffer = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            for (int i = 0; i < length; i++)
            {
                result[i] = CHARS[buffer[i] % CHARS.Length];
            }

            return new string(result);
        }
        public static string ToCurl(this HttpRequestMessage request)
        {
            var curlBuilder = new StringBuilder();

            // Bắt đầu với curl command
            curlBuilder.Append("curl ");

            // Thêm method
            curlBuilder.Append($"-X {request.Method} ");

            // Thêm headers nếu có
            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    var headerValue = string.Join(",", header.Value);
                    curlBuilder.Append($"-H \"{header.Key}: {headerValue}\" ");
                }
            }

            // Thêm content headers và body nếu có
            if (request.Content != null)
            {
                // Thêm content headers
                foreach (var header in request.Content.Headers)
                {
                    var headerValue = string.Join(",", header.Value);
                    curlBuilder.Append($"-H \"{header.Key}: {headerValue}\" ");
                }

                // Thêm body
                var content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(content))
                {
                    // Escape các ký tự đặc biệt trong content
                    content = content.Replace("\"", "\\\"");
                    curlBuilder.Append($"-d \"{content}\" ");
                }
            }

            // Thêm URL cuối cùng
            curlBuilder.Append($"\"{request.RequestUri}\"");

            return curlBuilder.ToString().Trim();
        }

        // Hàm log tiện ích
        public static void LogAsCurl(this HttpRequestMessage request, Action<string> logger)
        {
            var curlCommand = request.ToCurl();
            logger?.Invoke(curlCommand);
        }
    }
}
