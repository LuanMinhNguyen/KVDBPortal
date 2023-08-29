// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Net;

namespace TestFolderWatcher
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Data.Entities;
    using OpenPop.Pop3;
    using OpenPop.Mime;
    using System.Data;
    using System.Data.OracleClient;

    /// <summary>
    /// The program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The is create.
        /// </summary>
        private bool isCreate = false;

        /// <summary>
        /// The file icon.
        /// </summary>
        private static Dictionary<string, string> fileIcon = new Dictionary<string, string>()
                {
                    { "doc", "images/wordfile.png" },
                    { "docx", "images/wordfile.png" },
                    { "dotx", "images/wordfile.png" },
                    { "xls", "images/excelfile.png" },
                    { "xlsx", "images/excelfile.png" },
                    { "pdf", "images/pdffile.png" },
                    { "7z", "images/7z.png" },
                    { "dwg", "images/dwg.png" },
                    { "dxf", "images/dxf.png" },
                    { "rar", "images/rar.png" },
                    { "zip", "images/zip.png" },
                    { "txt", "images/txt.png" },
                    { "xml", "images/xml.png" },
                    { "xlsm", "images/excelfile.png" },
                    { "bmp", "images/bmp.png" },
                };

        /// <summary>
        /// The folder service.
        /// </summary>
        private static FolderService folderService = new FolderService();

        /// <summary>
        /// The document service.
        /// </summary>
        private static DocumentService documentService = new DocumentService();

        private static ProcurementRequirementTypeService prTypeService = new ProcurementRequirementTypeService();

        private static DocumentPackageService documentPackageService = new DocumentPackageService();
        private static RevisionService revisionService = new RevisionService();
        private static StatusService statusService = new StatusService();
        private static DisciplineService disciplineService = new DisciplineService();
        private static DocumentTypeService documentTypeService = new DocumentTypeService();
        private static ReceivedFromService receivedFromService = new ReceivedFromService();

        private static PermissionWorkgroupService PermissionWorkgroupService = new PermissionWorkgroupService();

        private static AttachFileService attachFileService = new AttachFileService();

        /// <summary>
        /// The file extensions.
        /// </summary>
        private static List<string> fileExtensions = ConfigurationSettings.AppSettings.Get("Extension").Split(',').ToList();

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        static void Main(string[] args)
        {
            var copiedMonth = 0;
            var dayOfMonth = Convert.ToInt32(ConfigurationManager.AppSettings["CopyDate"]);
            Console.Write(dayOfMonth);

            var desFolder = new DirectoryInfo(ConfigurationManager.AppSettings["EAMCopyDataFolder"]);
            try
            {
                if (DateTime.Now.Day == dayOfMonth && DateTime.Now.Month != copiedMonth)
                {
                    Console.Write("Delete older folder");
                    foreach (var subFolder in desFolder.GetDirectories())
                    {
                        Directory.Delete(subFolder.FullName, true);
                    }

                    CopyDataFile();
                    copiedMonth = DateTime.Now.Month;
                }
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);

            }



            Console.ReadLine();


            //using (WebClient client = new WebClient())
            //{
            //    string htmlCode = client.DownloadString("https://hextracoin.co/exchange");
            //}

            //var contentCode = "P0ASB122";
            //var unitCode = contentCode.Substring(0, 2);
            //var kksCode = contentCode.Substring(2, contentCode.Length - 2);
            //var trainNo = contentCode.Substring(contentCode.IndexOf('(') + 1,
            //    contentCode.IndexOf(')') - contentCode.IndexOf('(') - 1);


            //ImportPR();
            //ImportContract();
            //string hexString = DateTime.Now.Ticks.ToString("X2");

            //var conn = new OdbcConnection();
            //conn.ConnectionString =
            //           "Driver={SYBASE SYSTEM 11};" +
            //           "Srvr=amos;" +
            //           "Uid=admin;" +
            //           "Pwd=123456;";
            //conn.Open();

            ////var client = new SmtpClient("smtp.gmail.com", 587)
            ////{
            ////    Credentials = new NetworkCredential("biendong.pps.ptsc@gmail.com", "ptsc1234"),
            ////    EnableSsl = true
            ////};
            //////client.Send("biendong.pps.ptsc@gmail.com", "nguyenvanhong@truetech.com.vn", "test", "testbody");


            ////using (StreamWriter sw = File.CreateText("C:\\temp.sql"))
            ////{
            ////    sw.WriteLine("Hello");
            ////    sw.WriteLine("And");
            ////    sw.WriteLine("Welcome");
            ////}
            ////var connectionString = "Dsn=amos;userid=admin;databasefile='F:\\DATA AMOS LAM SON- 19 Aug 16\\amos.db';servername=amos;autostop=YES;integrated=NO;debug=NO;disablemultirowfetch=NO;compress=NO;description=amos;uid=admin;pwd=123456";
            ////var conn = new OdbcConnection(connectionString);
            ////var da = new OdbcDataAdapter();
            ////var cmd = new OdbcCommand();
            ////cmd.CommandText = "SELECT " +
            ////                  "u.PartID as PartID," +
            ////                  "u.PartTypeID," +
            ////                  "u.StockMax," +
            ////                  "u.StockMin," +
            ////                  "spLocation.InStock as ROB," +
            ////                  "unit.Name as UnitName," +
            ////                  "t.PartTypeNo as PartNo," +
            ////                  "t.PartName as Name," +
            ////                  "t.MakerRef as MakerRef " +
            ////                  "FROM amos.SpareUnit u, amos.SpareLocation spLocation, amos.SpareType t " +
            ////                  "left outer join amos.SpareTypeFinancial f ON t.PartTypeID = f.PartTypeID AND f.DeptID = amos.GetSharing(0, 'SpareTypeFinancial') " +
            ////                  "left join amos.Unit unit ON unit.UnitID = t.StockUnitID " +
            ////                  "WHERE u.PartTypeID = t.PartTypeID " +
            ////                  "AND u.PartID = spLocation.PartID " +
            ////                  "AND t.PartTypeNo like '%888%' ";
            ////cmd.Connection = conn;
            ////da.SelectCommand = cmd;
            ////conn.Open();
            ////var ds = new DataSet();
            ////da.Fill(ds);
            ////conn.Close();
            /// 

            //Pop3Client pop3Client;
            //pop3Client = new Pop3Client();
            //pop3Client.Connect("pop.gmail.com", 995, true);
            //pop3Client.Authenticate("pps.ptsc@gmail.com", "ptsc1234", AuthenticationMethod.UsernameAndPassword);

            //int count = pop3Client.GetMessageCount();
            //int counter = 0;
            //for (int i = count; i >= 1; i--)
            //{
            //    Message message = pop3Client.GetMessage(i);

            //    List<MessagePart> attachments = message.FindAllAttachments();
            //    foreach (var messagePart in attachments)
            //    {
            //        ByteArrayToFile("D:\\" + messagePart.FileName, messagePart.Body);
            //    }
            //}
            //Console.WriteLine("Completed!!!");
            //Console.ReadLine();
        }

        private static void CopyDataFile()
        {

            var ds = new DataSet();
            var strConn = ConfigurationManager.ConnectionStrings["cmmsdbconnection"].ConnectionString;
            var conn = new OracleConnection(strConn);
            var da = new OracleDataAdapter();
            var cmd = new OracleCommand();
            cmd.Connection = conn;

            cmd.CommandText = @"SELECT EVT_ORG, EVT_MRC, EVT_CODE, EVT_DESC, DOC_FILENAME
                                FROM r5docentities INNER JOIN R5DOCUMENTS ON DOC_CODE = DAE_DOCUMENT
                                INNER JOIN R5EVENTS ON DAE_CODE = EVT_CODE
                                WHERE DAE_ENTITY = 'EVNT' AND(EVT_PPM LIKE '%-4K' OR EVT_PPM LIKE '%-8K' OR EVT_PPM LIKE '%-PM4K' OR EVT_PPM LIKE '%-PM8K')
                                AND ((YEAR(EVT_SCHEDEND) = YEAR(SYSDATE) AND MONTH(EVT_SCHEDEND) = MONTH(SYSDATE) - 1 AND MONTH(SYSDATE) > 1)
                                OR (YEAR(EVT_SCHEDEND) = YEAR(SYSDATE) - 1 AND MONTH(EVT_SCHEDEND) = 12 AND MONTH(SYSDATE) = 1))
                                ORDER BY EVT_ORG,  EVT_MRC, EVT_CODE";
            cmd.CommandType = CommandType.Text;

            da.SelectCommand = cmd;

            try
            {
                conn.Open();
                da.Fill(ds);
                conn.Close();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Console.WriteLine("Total data row: " + ds.Tables[0].Rows.Count);

                    var dataList = ds.Tables[0].AsEnumerable().ToList();
                    var sourceFolder = ConfigurationManager.AppSettings["EAMUploadDataFolder"];
                    var desFolder = ConfigurationManager.AppSettings["EAMCopyDataFolder"];
                    var dataGroupByOrg = dataList.GroupBy(t => t["EVT_ORG"].ToString());
                    foreach (var orgGroup in dataGroupByOrg)
                    {
                        var dataGroupByDiscipline = orgGroup.GroupBy(t => t["EVT_MRC"].ToString());
                        foreach (var disciplineGroup in dataGroupByDiscipline)
                        {
                            Directory.CreateDirectory(desFolder + orgGroup.Key + @"\" + disciplineGroup.Key + @"\");
                            foreach (DataRow item in disciplineGroup)
                            {
                                var filePath = sourceFolder + item["DOC_FILENAME"].ToString();
                                var fileInfor = new FileInfo(filePath);
                                if (fileInfor.Exists)
                                {
                                    //File.Copy(filePath, desFolder + orgGroup.Key + @"\" + disciplineGroup.Key + @"\" + item["DOC_FILENAME"].ToString(), true);
                                    Console.WriteLine("Copy completed: '" + filePath + "'");
                                }
                                else
                                {
                                    Console.WriteLine("File not found: '" + filePath + "'");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Don't have data");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Have Error: " + ex.Message);
            }
        }

        static bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                // Open file for reading
                System.IO.FileStream _FileStream =
                   new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
                                            System.IO.FileAccess.Write);
                // Writes a block of bytes to this stream using data from
                // a byte array.
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);

                // close file stream
                _FileStream.Close();

                return true;
            }
            catch (Exception _Exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}",
                                  _Exception.ToString());
            }

            // error occured, return false
            return false;
        }

        static void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations[objValidations.Add()];

            // Set the validation type.
            validation.Type = Aspose.Cells.ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = "=" + formular;

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "Error";

            // Set the error message.
            validation.ErrorMessage = "Please select a color from the list";

            // Specify the validation area.
            CellArea area;
            area.StartRow = startRow;
            area.EndRow = endRow;
            area.StartColumn = startColumn;
            area.EndColumn = endColumn;

            // Add the validation area.
            validation.AreaList.Add(area);

            ////return validation;
        }

        /// <summary>
        /// The fsw_ renamed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        static void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                if (e.FullPath.IndexOf(@"RevisionHistory", StringComparison.Ordinal) == -1 &&
                    e.Name.LastIndexOf(".", StringComparison.Ordinal) != -1)
                {
                    var fullPath = e.OldName;
                    var fileExt = fullPath.Substring(
                        fullPath.LastIndexOf(".") + 1, fullPath.Length - fullPath.LastIndexOf(".") - 1);

                    if (fileExtensions.Contains(fileExt.ToLower()))
                    {
                        var lastPosition = fullPath.LastIndexOf(@"\");
                        var oldFileName = fullPath.Substring(lastPosition + 1, fullPath.Length - lastPosition - 1);
                        var oldPath = "DocumentLibrary/" + fullPath.Substring(0, lastPosition).Replace(@"\", "/");

                        var newFileName = e.Name.Substring(lastPosition + 1, e.Name.Length - lastPosition - 1);

                        var listDocRename = documentService.GetSpecificDocument(oldFileName, oldPath);
                        foreach (var document in listDocRename)
                        {
                            ////var oldRevisionPath = e.FullPath.Substring(0, e.FullPath.IndexOf("DocumentLibrary"))
                            ////                      + "DocumentLibrary/RevisionHistory/" + document.RevisionFileName;

                            document.Name = newFileName;
                            document.FileNameOriginal = newFileName;
                            document.LastUpdatedDate = DateTime.Now;

                            if (!string.IsNullOrEmpty(document.RevisionName))
                            {
                                document.RevisionFileName = document.RevisionName + "_" + newFileName;
                            }
                            else
                            {
                                document.RevisionFileName = document.DocIndex + "_" + newFileName;
                            }

                            document.FilePath = document.FilePath.Replace(oldFileName, newFileName);
                            ////document.RevisionFilePath = document.RevisionFilePath.Replace(oldFileName, newFileName);

                            ////var newRevisionPath = e.FullPath.Substring(0, e.FullPath.IndexOf("DocumentLibrary"))
                            ////                      + "DocumentLibrary/RevisionHistory/" + document.RevisionFileName;

                            ////File.Move(oldRevisionPath, newRevisionPath);

                            documentService.Update(document);
                        }

                        ////Console.WriteLine("Renamed: FileName - {0}, ChangeType - {1}, Old FileName - {2}", e.Name, e.ChangeType, e.OldName);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// The fsw_ deleted.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        static void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(@"RevisionHistory", StringComparison.Ordinal) == -1 &&
                e.Name.LastIndexOf(".", StringComparison.Ordinal) != -1)
            {
                var fullPath = e.Name;
                var fileExt = fullPath.Substring(
                    fullPath.LastIndexOf(".", StringComparison.Ordinal) + 1, fullPath.Length - fullPath.LastIndexOf(".", StringComparison.Ordinal) - 1);

                if (fileExtensions.Contains(fileExt))
                {
                    try
                    {
                        var lastPosition = fullPath.LastIndexOf(@"\");
                        var fileName = fullPath.Substring(lastPosition + 1, fullPath.Length - lastPosition - 1);
                        var path = "DocumentLibrary/" + fullPath.Substring(0, lastPosition).Replace(@"\", "/");

                        var listDocDelete = documentService.GetSpecificDocument(fileName, path);
                        foreach (var document in listDocDelete)
                        {
                            document.IsDelete = true;
                            ////documentService.Update(document);

                            ////var revisionPath = e.FullPath.Substring(0, e.FullPath.IndexOf("DocumentLibrary"))
                            ////                   + "DocumentLibrary/RevisionHistory/" + document.RevisionFileName;
                            ////var fileTemp = new FileInfo(revisionPath);
                            ////fileTemp.Delete();
                        }

                        Console.WriteLine("Deleted: FileName - {0}, ChangeType - {1}", e.Name, e.ChangeType);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.InnerException.Message);
                    }
                }
            }
        }

        /// <summary>
        /// The fsw_ changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        static void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(@"RevisionHistory", StringComparison.Ordinal) == -1 &&
                e.Name.LastIndexOf(".", StringComparison.Ordinal) != -1)
            {
                var fullPath = e.Name;
                var fileExt = fullPath.Substring(
                    fullPath.LastIndexOf(".") + 1, fullPath.Length - fullPath.LastIndexOf(".") - 1);

                if (fileExtensions.Contains(fileExt.ToLower()))
                {
                    var lastPosition = fullPath.LastIndexOf(@"\");
                    var fileName = fullPath.Substring(lastPosition + 1, fullPath.Length - lastPosition - 1);
                    var path = fullPath.Substring(0, lastPosition).Replace(@"\", "/");

                    var objFolder = folderService.GetByDirName("DocumentLibrary/" + path);
                    if (objFolder != null)
                    {
                        var docLeaf = documentService.GetSpecificDocument(objFolder.ID, fileName);

                        var objDoc = new Document()
                            {
                                Name = docLeaf.Name,
                                DocumentNumber = docLeaf.DocumentNumber,
                                Title = docLeaf.Title,
                                DocumentTypeID = docLeaf.DocumentTypeID,
                                StatusID = docLeaf.StatusID,
                                DisciplineID = docLeaf.DisciplineID,
                                ReceivedFromID = docLeaf.ReceivedFromID,
                                ReceivedDate = docLeaf.ReceivedDate,
                                TransmittalNumber = docLeaf.TransmittalNumber,
                                LanguageID = docLeaf.LanguageID,
                                Well = docLeaf.Well,
                                Remark = docLeaf.Remark,
                                KeyWords = docLeaf.KeyWords,
                                
                                IsLeaf = true,
                                DocIndex = docLeaf.DocIndex + 1,
                                IsDelete = false,
                                FolderID = docLeaf.FolderID,
                                DirName = docLeaf.DirName,
                                FileExtension = docLeaf.FileExtension,
                                FileExtensionIcon = docLeaf.FileExtensionIcon,
                                FileNameOriginal = docLeaf.FileNameOriginal,
                                FilePath = docLeaf.FilePath,
                                CreatedDate = DateTime.Now
                            };

                        if (docLeaf.ParentID == null)
                        {
                            objDoc.ParentID = docLeaf.ID;
                        }
                        else
                        {
                            objDoc.ParentID = docLeaf.ParentID;
                        }

                        objDoc.RevisionFileName = objDoc.DocIndex + "_" + objDoc.FileNameOriginal;
                        objDoc.RevisionFilePath = docLeaf.RevisionFilePath.Replace(
                            docLeaf.RevisionFileName, objDoc.RevisionFileName);
                        // Copy to revision folder
                        var revisionPath = e.FullPath.Substring(0, e.FullPath.IndexOf("DocumentLibrary"))
                                           + "DocumentLibrary/RevisionHistory/";
                        var tempFile = new FileInfo(e.FullPath);


                        tempFile.CopyTo(revisionPath + objDoc.RevisionFileName, true);

                        docLeaf.IsLeaf = false;

                        documentService.Update(docLeaf);
                        documentService.Insert(objDoc);
                    }

                    Console.WriteLine("Changed: FileName - {0}, ChangeType - {1}", e.Name, e.ChangeType);
                }
            }
        }

        /// <summary>
        /// The fsw_ created.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        static void fsw_Created(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath.IndexOf(@"RevisionHistory", StringComparison.Ordinal) == -1 && 
                e.Name.LastIndexOf(".", StringComparison.Ordinal) != -1)
            {
                var fullPath = e.Name;
                var fileExt = fullPath.Substring(
                    fullPath.LastIndexOf(".") + 1, fullPath.Length - fullPath.LastIndexOf(".") - 1);

                if (fileExtensions.Contains(fileExt.ToLower()))
                {
                    try
                    {
                        var lastPosition = fullPath.LastIndexOf(@"\");
                        var fileName = fullPath.Substring(lastPosition + 1, fullPath.Length - lastPosition - 1);
                        var path = fullPath.Substring(0, lastPosition).Replace(@"\", "/");

                        var objFolder = folderService.GetByDirName("DocumentLibrary/" + path);

                        var objDoc = new Document()
                            {
                                Name = fileName,
                                DocIndex = 1,
                                CreatedDate = DateTime.Now,
                                IsLeaf = true,
                                IsDelete = false
                            };
                        objDoc.RevisionFileName = objDoc.DocIndex + "_" + fileName;
                        objDoc.FilePath = "/bdpocedms/DocumentLibrary/" + path + "/" + fileName;
                        objDoc.RevisionFilePath = "/bdpocedms/DocumentLibrary/RevisionHistory/" + objDoc.DocIndex + "_" + fileName;
                        objDoc.FileExtension = fileExt;
                        objDoc.FileExtensionIcon = fileIcon.ContainsKey(fileExt.ToLower())
                                                       ? fileIcon[fileExt.ToLower()]
                                                       : "images/otherfile.png";
                        objDoc.FileNameOriginal = fileName;
                        objDoc.DirName = "DocumentLibrary/" + path;

                        if (objFolder != null)
                        {
                            objDoc.FolderID = objFolder.ID;
                        }

                        // Copy to revision folder
                        var revisionPath = e.FullPath.Substring(0, e.FullPath.IndexOf("DocumentLibrary"))
                                           + "DocumentLibrary/RevisionHistory/";
                        var tempFile = new FileInfo(e.FullPath);


                        tempFile.CopyTo(revisionPath + objDoc.RevisionFileName, true);
                        documentService.Insert(objDoc);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException.Message);
                    }

                    Console.WriteLine("Created: FileName - {0}, ChangeType - {1}", e.Name, e.ChangeType);
                }
            }
        }

        /// <summary>
        /// The create folder.
        /// </summary>
        static void CreateFolder()
        {
            var rootPath = ConfigurationSettings.AppSettings.Get("rootPath");
            var listFolder = File.ReadAllLines(ConfigurationSettings.AppSettings.Get("listFolder")).ToList();

            ////Directory.CreateDirectory(rootPath + listFolder[1]);
            foreach (var folder in listFolder)
            {
                if (!string.IsNullOrEmpty(folder))
                {
                    Directory.CreateDirectory(rootPath + folder);
                    var listSubfolder = folder.Split('/').Where(t => !string.IsNullOrEmpty(t)).ToList();
                    var dirFather = listSubfolder[0] + "/" + listSubfolder[1];

                    for (int i = 2; i < listSubfolder.Count; i++)
                    {
                        var folFa = folderService.GetByDirName(dirFather);
                        var folChild = folderService.GetByDirName(dirFather + "/" + listSubfolder[i]);
                        if (folChild == null)
                        {
                            folChild = new Folder
                            {
                                Name = listSubfolder[i],
                                DirName = dirFather + "/" + listSubfolder[i],
                                ParentID = folFa.ID,
                                CategoryID = folFa.CategoryID,
                                Description = listSubfolder[i]
                            };

                            folderService.Insert(folChild);
                        }

                        dirFather += "/" + listSubfolder[i];
                    }
                }
            }

        }
    }
}
