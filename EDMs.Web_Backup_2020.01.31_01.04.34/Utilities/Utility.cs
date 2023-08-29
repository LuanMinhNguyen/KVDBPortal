using System.Text.RegularExpressions;

namespace EDMs.Web.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Objects.DataClasses;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.ServiceProcess;
    using System.Text;

    using EDMs.Business.Services;

    public class Utility
    {
        /// <summary>
        /// The passphrase.
        /// </summary>
        private const string Passphrase = "MASUpilamix!987";

        /// <summary>
        /// The alphabet dictionary.
        /// </summary>
        public static Dictionary<string, string> alphabetDictionary = new Dictionary<string, string>
            {
                { "1", "A" },
                { "2", "B" },
                { "3", "C" },
                { "4", "D" },
                { "5", "E" },
                { "6", "F" },
                { "7", "G" },
                { "8", "H" },
                { "9", "I" },
                { "10", "J" },
                { "11", "K" },
                { "12", "L" },
                { "13", "M" },
                { "14", "N" },
                { "15", "O" },
                { "16", "P" },
                { "17", "Q" },
                { "18", "R" },
                { "19", "S" },
                { "20", "T" },
                { "21", "U" },
                { "22", "V" },
                { "23", "W" },
                { "24", "X" },
                { "25", "Y" },
                { "26", "Z" },
            };

        public static Dictionary<int, string> WorkflowStepDefine = new Dictionary<int, string>
        {
            {0,string.Empty},
            {1,"Info Assign"},
            {2,"Review"},
            {3,"Review with update"},
            {4,"Approve"},
            {5,"Sign off doc revision"},
        };

        public static Dictionary<string, string> FileIcon = new Dictionary<string, string>()
                    {
                        { ".doc", "~/images/wordfile.png" },
                        { ".docx", "~/images/wordfile.png" },
                        { ".dotx", "~/images/wordfile.png" },
                        { ".xls", "~/images/excelfile.png" },
                        { ".xlsx", "~/images/excelfile.png" },
                        { ".pdf", "~/images/pdffile.png" },
                        { ".7z", "~/images/7z.png" },
                        { ".dwg", "~/images/dwg.png" },
                        { ".dxf", "~/images/dxf.png" },
                        { ".rar", "~/images/rar.png" },
                        { ".zip", "~/images/zip.png" },
                        { ".txt", "~/images/txt.png" },
                        { ".xml", "~/images/xml.png" },
                        { ".xlsm", "~/images/excelfile.png" },
                        { ".bmp", "~/images/bmp.png" },
                    };
        public static Dictionary<string, string> FileIcons = new Dictionary<string, string>()
                    {
                        { "doc", "~/images/wordfile.png" },
                        { "docx", "~/images/wordfile.png" },
                        { "dotx", "~/images/wordfile.png" },
                        { "xls", "~/images/excelfile.png" },
                        { "xlsx", "~/images/excelfile.png" },
                        { "pdf", "~/images/pdffile.png" },
                        { "7z", "~/images/7z.png" },
                        { "dwg", "~/images/dwg.png" },
                        { "dxf", "~/images/dxf.png" },
                        { "rar", "~/images/rar.png" },
                        { "zip", "~/images/zip.png" },
                        { "txt", "~/images/txt.png" },
                        { "xml", "~/images/xml.png" },
                        { "xlsm", "~/images/excelfile.png" },
                        { "bmp", "~/images/bmp.png" },
                    };
        /// <summary>
        /// The clone.
        /// </summary>
        /// <param name="entityObject">
        /// The entity object.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Clone<T>(T entityObject) where T : EntityObject, new()
        {
            return EntityCloner<T>.Clone(entityObject);
        }

        /// <summary>
        /// The clone with graph.
        /// </summary>
        /// <param name="entityObject">
        /// The entity object.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T CloneWithGraph<T>(T entityObject) where T : EntityObject, new()
        {
            return EntityCloner<T>.CloneWithGraph(entityObject);
        }

        /// <summary>
        /// The deep clone.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T DeepClone<T>(T obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Generate patient code
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// Patient code with format
        /// </returns>
        public static string GeneratePatientCode(string code)
        {
            var patientCodeFormat = System.Configuration.ConfigurationManager.AppSettings["PatientCodeFormat"];
            var result =
                (patientCodeFormat.Remove(patientCodeFormat.Length - 1 - code.Length, code.Length) + code).Replace(
                    "x", "0");

            return result;
        }

        /// <summary>
        /// Converts the type of the string to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The param.</param>
        /// <returns></returns>
        public static object ConvertStringToType<T>(string param)
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (underlyingType == null)
                return Convert.ChangeType(param, typeof(T), CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(param)
              ? null
              : Convert.ChangeType(param, underlyingType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The encrypt.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Encrypt(string message)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // to create the object for UTF8Encoding  class
            // TO create the object for MD5CryptoServiceProvider 
            var md5 = new MD5CryptoServiceProvider();
            byte[] deskey = md5.ComputeHash(utf8.GetBytes(Passphrase));

            // to convert to binary passkey
            // TO create the object for  TripleDESCryptoServiceProvider 
            var desalg = new TripleDESCryptoServiceProvider();
            desalg.Key = deskey; // to  pass encode key
            desalg.Mode = CipherMode.ECB;
            desalg.Padding = PaddingMode.PKCS7;
            byte[] encrypt_data = utf8.GetBytes(message);

            // to convert the string to utf encoding binary 

            try
            {
                // To transform the utf binary code to md5 encrypt    
                ICryptoTransform encryptor = desalg.CreateEncryptor();
                results = encryptor.TransformFinalBlock(encrypt_data, 0, encrypt_data.Length);
            }
            finally
            {
                // to clear the allocated memory
                desalg.Clear();
                md5.Clear();
            }

            // to convert to 64 bit string from converted md5 algorithm binary code
            return Convert.ToBase64String(results);
        }

        /// <summary>
        /// The decrypt.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Decrypt(string message)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();
            var md5 = new MD5CryptoServiceProvider();
            byte[] deskey = md5.ComputeHash(utf8.GetBytes(Passphrase));
            var desalg = new TripleDESCryptoServiceProvider();
            desalg.Key = deskey;
            desalg.Mode = CipherMode.ECB;
            desalg.Padding = PaddingMode.PKCS7;
            byte[] decrypt_data = Convert.FromBase64String(message);
            try
            {
                // To transform the utf binary code to md5 decrypt
                ICryptoTransform decryptor = desalg.CreateDecryptor();
                results = decryptor.TransformFinalBlock(decrypt_data, 0, decrypt_data.Length);
            }
            finally
            {
                desalg.Clear();
                md5.Clear();
            }

            // TO convert decrypted binery code to string
            return utf8.GetString(results);
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

        /// <summary>
        /// The convert to data table.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="DataTable"/>.
        /// </returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;

        }

        /// <summary>
        /// The service is available.
        /// </summary>
        /// <param name="serviceName">
        /// The service name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool ServiceIsAvailable(string serviceName)
        {
            var ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serviceName);
            if (ctl != null)
            {
                if (ctl.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
            }

            return false;
        }

        public static string RemoveSpecialCharacter(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z]+", string.Empty);
        }
        public static bool ConvertStringToDateTime(string input, ref DateTime output)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var strDatetime = input.Substring(0, input.LastIndexOf("/") + 5);

                if (DateTime.TryParseExact(strDatetime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }
                if (DateTime.TryParseExact(strDatetime, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

                if (DateTime.TryParseExact(strDatetime, "M/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

                if (DateTime.TryParseExact(strDatetime, "MM/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

                if (DateTime.TryParseExact(strDatetime, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

               

                if (DateTime.TryParseExact(strDatetime, "d/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

                if (DateTime.TryParseExact(strDatetime, "dd/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }

                if (DateTime.TryParseExact(strDatetime, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out output))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ReturnSequenceString(int currentSequence, int lenght)
        {
            var strSequence = string.Empty;
            for (int i = currentSequence.ToString().Length; i < lenght; i++)
            {
                strSequence += "0";
            }

            strSequence += currentSequence.ToString();
            return strSequence;
        }

        public static string RemoveSpecialCharacterForFolder(string input)
        {
            return Regex.Replace(input, @"[\\/\:\*\?\<\>\|]", string.Empty);
        }
        public static string RemoveSpecialCharacterFileName(string input)
        {
            return Regex.Replace(input, @"[\\/\:\*\?\<\>\&\+\^\%\$\#\@\!\~\=\|]", "-");
        }

        public static string ConvertToUnsign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
    }
}