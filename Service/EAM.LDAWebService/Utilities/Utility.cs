using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EAM.LDAWebService.Utilities
{

    public class Utility
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime EpochMR = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ConvertToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalMilliseconds;
        }

        public static DateTime ConvertToDatetime(long unixTime)
        {
            return Epoch.AddMilliseconds(unixTime);
        }

        public static long ConvertToTimestampMR(DateTime value)
        {
            TimeSpan elapsedTime = value.AddHours(-7) - EpochMR;
            return (long)elapsedTime.TotalMilliseconds;
        }

        public static DateTime ConvertToDatetimeMR(long unixTime)
        {
            return EpochMR.AddMilliseconds(unixTime);
        }

        public static string SendVerifyCodeForSafeCityUser(string code, string phoneNumber, bool isAllowUnicode)
        {
            string message;
            try
            {
                var msg = "SafeCity App - Mã chứng thực của bạn là: '" + code + "'. Your verify code is: '" + code + "'";

                SendUnicodeSMSVNPT(msg, phoneNumber, isAllowUnicode);
                message = "Gửi mã xác thực thành công - " + phoneNumber;
            }
            catch (Exception ex)
            {
                message = "Có lỗi xảy ra: " + ex.Message;
            }

            return message;
        }

        public static string SendUnicodeSMSVNPT(string msg, string phone, bool isAllowUnicode)
        {
            var message = string.Empty;
            try
            {
                if (phone.StartsWith("0"))
                {
                    phone = "84" + phone.Remove(0, 1);
                }
                else
                {
                    phone = "84" + phone;
                }

                var temp1 = "{\"RQST\":" +
                            "{ \"name\": \"send_sms_list\"," +
                            "\"REQID\": \"12345\"," +
                            "\"LABELID\": \"87852\"," +
                            "\"CONTRACTTYPEID\": \"1\"," +
                            "\"CONTRACTID\": \"7615\"," +
                            "\"TEMPLATEID\": \"333577\"," +
                            "\"PARAMS\": [" +
                            "{" +
                            "\"NUM\": \"1\"," +
                            "\"CONTENT\": \"" + (isAllowUnicode ? msg : RemoveUnicode(msg)) + "\"" +
                            "}]," +
                            "\"SCHEDULETIME\": \"\", " +
                            "\"MOBILELIST\": \"" + phone + "\"," +
                            "\"ISTELCOSUB\": \"0\"," +
                            "\"AGENTID\": \"213\"," +
                            "\"APIUSER\": \"SmartCityPQ\"," +
                            "\"APIPASS\": \"123456\"," +
                            "\"USERNAME\": \"KGG_CS\"," +
                            "\"DATACODING\": \"8\"" +
                            "}" +
                            "}";

                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                message = webClient.UploadString("http://113.185.0.35:8888/smsmarketing/api", "POST", temp1);
                message = phone + "-" + message;
            }
            catch (Exception ex)
            {
                message = "Have Error: " + ex.Message;
            }

            return message;
        }

        public static string SendNotification(string title, string message, string deviceToken, string eventId)
        {
            var result = string.Empty;
            try
            {
                var strData = "{" +
                    "\"notification\" : " +
                    "{" +
                    "   \"body\" : \"" + message + "\", " +
                    "   \"title\" : \"" + title + "\"" +
                    "}," +
                    "\"data\": " +
                    "{" +
                    "   \"message\":" +
                    "   {" +
                    "       \"body\" : \"" + message + "\"," +
                    "       \"title\" : \"" + title + "\"," +
                    "       \"action\":1," +
                    "       \"id\":\"" + eventId + "\"," +
                    "       \"btnsTitle\":[\"Hủy\",\"Chi Tiết Sự Kiện\"]" +
                    "   }" +
                    "}," +
                    "   \"to\" :\"" + deviceToken + "\"" +
                    "}";

                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Headers["Authorization"] = "key=AAAALwGKZtw:APA91bH2KzoQFvBIo8teE81Ctxpy8abye_0KIe1S5rdhLcQU7ND0_gr7IKMmJA-VBCd8hrAwQBV5RXYyMYDsnqlU9sisyU5Seg8aPlkf98laMRiN0VpXskivegeWpe2PYqf6xPswKfH4";

                webClient.Encoding = Encoding.UTF8;
                result = webClient.UploadString("https://fcm.googleapis.com/fcm/send", "POST", strData);

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return result;
        }

        public static string SendChatNotification(string title, string message, string deviceToken, string fromId, string toId)
        {
            var result = string.Empty;
            try
            {
                var strData = 
                    "{" +
                    "   \"notification\": " +
                    "   {" +
                    "       \"body\" : \""+message+"\", " +
                    "       \"title\" : \""+title+"\"" +
                    "   }," +
                    "   \"data\": " +
                    "   {" +
                    "       \"message\":" +
                    "       {" +
                    "           \"body\" : \""+message+"\", " +
                    "           \"title\" : \""+title+"\", " +
                    "           \"action\":2, " +
                    "           \"content\":\""+message+"\", " +
                    "           \"to\":"+toId+", " +
                    "           \"from\":"+fromId+"" +
                    "       }" +
                    "   }, " +
                    "   \"to\" :\""+deviceToken+"\"" +
                    "}";

                WebClient webClient = new WebClient();
                webClient.Headers["Content-type"] = "application/json";
                webClient.Headers["Authorization"] = "key=AAAALwGKZtw:APA91bH2KzoQFvBIo8teE81Ctxpy8abye_0KIe1S5rdhLcQU7ND0_gr7IKMmJA-VBCd8hrAwQBV5RXYyMYDsnqlU9sisyU5Seg8aPlkf98laMRiN0VpXskivegeWpe2PYqf6xPswKfH4";

                webClient.Encoding = Encoding.UTF8;
                result = webClient.UploadString("https://fcm.googleapis.com/fcm/send", "POST", strData);

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

            return result;
        }

        public static string RemoveUnicode(string inputText)
        {
            string stFormD = inputText.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string str = "";
            for (int i = 0; i <= stFormD.Length - 1; i++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc == UnicodeCategory.NonSpacingMark == false)
                {
                    if (stFormD[i] == 'đ')
                        str = "d";
                    else if (stFormD[i] == 'Đ')
                        str = "D";
                    else if (stFormD[i] == '\r' | stFormD[i] == '\n')
                        str = "";
                    else
                        str = stFormD[i].ToString();
                    sb.Append(str);
                }
            }
            return sb.ToString();
        }

        public static string GetMd5Hash(string input)
        {
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}