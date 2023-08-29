using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using Aspose.Cells;
using EAM.Business.Services;
using EAM.Data.Dto;
using Telerik.Web.UI;

namespace EAM.WebPortal
{
    public partial class StoreToStoreRequisition : System.Web.UI.Page
    {
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Remove("CheckedNodes");
                this.divMess.Visible = false;
                var partList = this.GetPartList();
                this.LoadPartList(partList);

                this.lbltest.Text = Request.QueryString["eamid"];
            }
        }

        private List<PartListSYTDto> GetPartList()
        {
            var userParam = new SqlParameter("@user", SqlDbType.NVarChar);
            userParam.Value = Request.QueryString["userid"];
            DataSet ds;
            var partList = new List<PartListSYTDto>();
            ds = this.eamService.GetDataSet("getSpareParts", new[] { userParam });
            if (ds != null)
            {
                partList = this.eamService.CreateListFromTable<PartListSYTDto>(ds.Tables[0]);
                foreach (var item in partList)
                {
                    item.FullName = item.MAVT + " - " + item.DIENGIAI;
                }

                Session.Add("PartList", partList);
            }

            return partList;
        }

        private void LoadPartList(List<PartListSYTDto> partList)
        {
            this.rtvPart.Nodes.Clear();
            var checkedNodes = new List<PartListDto>();
            if (Session["CheckedNodes"] != null)
            {
                checkedNodes = (List<PartListDto>) Session["CheckedNodes"];
            }

            foreach (var phanLoaiGroup in partList.GroupBy(t => t.PHANLOAI))
            {
                var mainNode = new RadTreeNode("<b>Nhóm: </b>" + phanLoaiGroup.Key, "ROOT$" + phanLoaiGroup.Key);
                //mainNode.ImageUrl = @"~/Images/folderdir16.png";
                foreach (var item in phanLoaiGroup)
                {
                    var childNode = new RadTreeNode(item.FullName, item.MAVT);
                    childNode.Checked = checkedNodes.Any(t => t.VatTu == childNode.Value);

                    //childNode.ImageUrl = @"~/Images/equiment16.png";
                    mainNode.Nodes.Add(childNode);
                    mainNode.Expanded = true;
                }

                this.rtvPart.Nodes.Add(mainNode);
            }
        }

        protected void RadWizard1_OnFinishButtonClick(object sender, WizardEventArgs e)
        {
            
        }

        protected void btnSearch_OnClick(object sender, ImageClickEventArgs e)
        {
            if (Session["PartList"] is List<PartListSYTDto> partList)
            {
                var searchText = this.txtSearch.Text.ToUpper().Trim();
                partList = partList.Where(t => (!string.IsNullOrEmpty(t.PHANLOAI) && t.PHANLOAI.ToUpper().Contains(searchText))
                                               || t.FullName.ToUpper().Contains(searchText)
                                               || t.DONVI.ToUpper().Contains(searchText)
                                               || t.DVT.ToUpper().Contains(searchText)
                ).ToList();

                this.LoadPartList(partList);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (rtvPart.CheckedNodes.Count > 0)
            {
                var workbook = new Workbook();
                var filePath = Server.MapPath(@"Resources\DataTemplate") + @"\";
                workbook.Open(filePath + "ReqDataTemplate.xlsx");
                var wsStoreList = workbook.Worksheets[3];
                var wsData = workbook.Worksheets[1];
                var wsStock = workbook.Worksheets[0];
                wsStock.Cells[1,0].PutValue("Ngày: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                var count = 0;
                var partList = Session["PartList"] as List<PartListSYTDto>;
                DataSet ds;
                if (Session["CheckedNodes"] != null)
                {
                    var checkedNodes = (List<PartListDto>)Session["CheckedNodes"];
                    if (checkedNodes.Count > 0)
                    {
                        foreach (var checkedNode in checkedNodes)
                        {
                            var partitem = partList.FirstOrDefault(t => t.MAVT == checkedNode.VatTu);
                            var partInStore = this.GetPartInStore(partitem.MAVT);
                            if (partInStore != null && (partInStore.KHO_PCD != 0 || partInStore.KHO_DUOC != 0 || partInStore.KHOTAITRO != 0))
                            {
                                count += 1;
                                wsData.Cells[0, 5 + count].PutValue(checkedNode.FullName + "\n(" + partitem.DVT + ")");
                                wsData.Cells[1, 5 + count].PutValue("Duyệt Cấp");

                                wsStock.Cells[4 + count, 0].PutValue(count);
                                wsStock.Cells[4 + count, 1].PutValue(partInStore.MAVT);
                                wsStock.Cells[4 + count, 2].PutValue(partInStore.TENVT);
                                wsStock.Cells[4+ count, 3].PutValue(partInStore.DVT);
                                wsStock.Cells[4 + count, 4].PutValue(partInStore.KHO_PCD);
                                wsStock.Cells[4 + count, 5].PutValue(partInStore.KHO_DUOC);
                                wsStock.Cells[4 + count, 6].PutValue(partInStore.KHOTAITRO);
                            }
                        }

                        ds = this.eamService.GetDataSet("GetAllStore");
                        if (ds.Tables.Count > 0)
                        {
                            wsStoreList.Cells.ImportDataTable(ds.Tables[0], false, 1, 0, ds.Tables[0].Rows.Count, 14, false);

                            var rangeStoreList = wsStoreList.Cells.CreateRange("A2", "A" + (ds.Tables[0].Rows.Count +1));
                            rangeStoreList.Name = "StoreList";

                            var validations = wsData.Validations[0];
                            this.CreateValidation("StoreList", validations, 1, 200, 2, 2);
                        }

                    }
                }
                wsData.Cells[0, 0].PutValue(count);

                var saveFileName = "Bảng PP HCVT " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
                var savePath = Server.MapPath(@"Resources\ExportData") + @"\";
                workbook.Save(savePath + saveFileName);
                this.DownloadByWriteByte(savePath + saveFileName, saveFileName, false);
            }
        }

        private void CreateValidation(string formular, Validation objValidations, int startRow, int endRow, int startColumn, int endColumn)
        {
            // Create a new validation to the validations list.
            Validation validation = objValidations;

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
            switch (formular)
            {
                case "StoreList":
                    validation.ErrorMessage = "Vui lòng chọn đơn vị trong danh mục có sẵn, nếu cần nhập đơn vị mới xin liên hệ Quản trị hệ thống.";
                    break;
            }

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

        private PartInStoreDto GetPartInStore(string partNumber)
        {
            DataSet ds;
            PartInStoreDto partInStore = null;
            var partParam = new SqlParameter("@PART", SqlDbType.NVarChar, 30);
            partParam.Value = partNumber;
            ds = this.eamService.GetDataSet("getStock", new[] { partParam });
            if (ds != null)
            {
                var partInStoreList = this.eamService.CreateListFromTable<PartInStoreDto>(ds.Tables[0]);
                partInStore = partInStoreList[0];
            }

            return partInStore;
        }

        private bool DownloadByWriteByte(string strFileName, string strDownloadName, bool DeleteOriginalFile)
        {
            try
            {
                //Kiem tra file co ton tai hay chua
                if (!File.Exists(strFileName))
                {
                    return false;
                }
                //Mo file de doc
                FileStream fs = new FileStream(strFileName, FileMode.Open);
                int streamLength = Convert.ToInt32(fs.Length);
                byte[] data = new byte[streamLength + 1];
                fs.Read(data, 0, data.Length);
                fs.Close();

                Response.Clear();
                Response.ClearHeaders();
                //Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                Response.AddHeader("Content-Length", data.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strDownloadName);
                Response.BinaryWrite(data);
                if (DeleteOriginalFile)
                {
                    File.SetAttributes(strFileName, FileAttributes.Normal);
                    File.Delete(strFileName);
                }

                Response.Flush();

                Response.End();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        protected void btnProcess_OnClick(object sender, EventArgs e)
        {
            var totalmess = string.Empty;
            foreach (UploadedFile docFile in this.radUpload.UploadedFiles)
            {
                var extension = docFile.GetExtension();
                if (extension == ".xls" || extension == ".xlsx" || extension == ".xlsm")
                {
                    var importPath = Server.MapPath(@"Resources\ImportData") + "/" + docFile.FileName;
                    docFile.SaveAs(importPath);
                    var dateValue = string.Empty;
                    var regex = new Regex(@"(\d{1,4}([.\-/])\d{1,2}([.\-/])\d{1,4})");
                    var m = regex.Match(docFile.FileName);
                    if (m.Success)
                    {
                        dateValue = m.Value;
                        // Instantiate a new workbook
                        var workbook = new Workbook();
                        workbook.Open(importPath);

                        // Get the first worksheet in the workbook
                        var worksheet = workbook.Worksheets[1];
                        try
                        {
                            var partCount = Convert.ToInt32(worksheet.Cells[0, 0].Value);
                            var dataTable = worksheet.Cells.ExportDataTable(2, 2, worksheet.Cells.MaxRow, 4 + partCount)
                                .AsEnumerable()
                                .Where(t => !string.IsNullOrEmpty(t["Column1"].ToString()))
                                .CopyToDataTable();

                            var storeToStoreReqList = this.PartReqObj(dataTable, partCount, worksheet);

                            foreach (var item in storeToStoreReqList)
                            {
                                var outputParam = new SqlParameter("@outputVal", SqlDbType.VarChar, 30);
                                outputParam.Direction = ParameterDirection.Output;
                                var reqSeq = this.eamService.GetOutputValue("get_seqREQ", new[] { outputParam })[0];

                                // Create store to store REQ
                                var reqSeqParam = new SqlParameter("@REQSEQ", SqlDbType.NVarChar, 30);
                                reqSeqParam.Value = reqSeq;
                                var fromCodeParam = new SqlParameter("@FROMCODE", SqlDbType.NVarChar, 30);
                                fromCodeParam.Value = item.FromCode;
                                var toCodeParam = new SqlParameter("@TOCODE", SqlDbType.NVarChar, 30);
                                toCodeParam.Value = item.ToCode;
                                var userParam = new SqlParameter("@USER", SqlDbType.NVarChar, 30);
                                userParam.Value = Request.QueryString["userid"].ToUpper();

                                var dateParam = new SqlParameter("@DATE", SqlDbType.NVarChar, 30);
                                dateParam.Value = dateValue;

                                var result1 = this.eamService.ExcuteQuery("INS_REQ", new[] { reqSeqParam, fromCodeParam, toCodeParam, userParam, dateParam });
                                // --------------------------------------------------------------------------------

                                // Create REQ Line
                                if (string.IsNullOrEmpty(result1))
                                {
                                    totalmess += "Tạo phiếu yêu cầu vật tư cho <b>'" + item.ToCodeName + "'</b> thành công với Số phiếu '" + reqSeq + "'. </br>";
                                    foreach (var itemLine in item.LineList)
                                    {
                                        var reqParam = new SqlParameter("@REQ", SqlDbType.NVarChar, 30);
                                        reqParam.Value = reqSeq;
                                        var partNumberParam = new SqlParameter("@PART", SqlDbType.NVarChar, 30);
                                        partNumberParam.Value = itemLine.PartNumber;
                                        var qtyParam = new SqlParameter("@QTY", SqlDbType.Decimal);
                                        qtyParam.Value = itemLine.Quality;

                                        var result2 = this.eamService.ExcuteQuery("INS_REQLINES", new[] { reqParam, partNumberParam, qtyParam });
                                        if (string.IsNullOrEmpty(result2))
                                        {
                                            totalmess += "---Tạo yêu cầu vật tư <b>'" + itemLine.PartNumber + "'</b> cho số phiếu '" + reqSeq + "' thành công. </br>";
                                        }
                                        else
                                        {
                                            totalmess += result2 + "</br>";
                                        }
                                    }
                                }
                                else
                                {
                                    totalmess += result1 + "</br>";
                                }
                                // --------------------------------------------------------------------------------

                                // Approve REQ
                                var approveReqParam = new SqlParameter("@REQ", SqlDbType.NVarChar, 30);
                                approveReqParam.Value = reqSeq;
                                var approveUserParam = new SqlParameter("@USER", SqlDbType.NVarChar, 30);
                                approveUserParam.Value = Request.QueryString["userid"].ToUpper();
                                var result3 = this.eamService.ExcuteQuery("A_REQ", new[] { approveReqParam, approveUserParam });
                                if (string.IsNullOrEmpty(result3))
                                {
                                    totalmess += "Duyệt phiếu yêu cầu vật tư số '" + reqSeq + "' thành công. </br>";
                                }
                                else
                                {
                                    totalmess += result3 + "</br>";
                                }
                                // ----------------------------------------------------------------
                            }
                        }
                        catch (Exception ex)
                        {
                            totalmess += "Error: " + ex.Message + "</br></br>";
                        }
                    }
                    else
                    {
                        totalmess += "Error: Tên file tải nạp không có thông tin dd-MM-yyyy, vui lòng kiểm tra lại!";
                    }
                }
            }

            this.divMess.Visible = true;
            this.lblMess.Text = totalmess;
            this.radUpload.UploadedFiles.Clear();
        }

        private List<StoreToStoreReqDto> PartReqObj(DataTable dataTable, int partCount, Worksheet worksheet)
        {
            var storeToStoreReqList = new List<StoreToStoreReqDto>();
            var regex = new Regex("^[0-9]+$", RegexOptions.Compiled);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var storeToStoreReq = new StoreToStoreReqDto()
                {
                    FromCode = dataRow["Column4"].ToString(),
                    ToCode = dataRow["Column2"].ToString(),
                    ToCodeName = dataRow["Column1"].ToString(),
                    UserName = Request.QueryString["userid"],
                    LineList = new List<StoreToStoreReqLineDto>()
                };

                for (int i = 1; i <= partCount; i++)
                {
                    if (worksheet.Cells[0, 5 + i].Value != null && !string.IsNullOrEmpty(worksheet.Cells[0, 5 + i].Value.ToString()))
                    {
                        var partItem = new StoreToStoreReqLineDto()
                        {
                            PartNumber = worksheet.Cells[0, 5 + i].Value.ToString().Split('-')[0].Trim(),
                            Quality = !string.IsNullOrEmpty(dataRow["Column" + (4 + i)].ToString()) && regex.Match(dataRow["Column" + (4 + i)].ToString()).Success 
                                ? Convert.ToDouble(dataRow["Column" + (4 + i)].ToString()) 
                                : 0
                        };

                        if (partItem.Quality != 0)
                        {
                            storeToStoreReq.LineList.Add(partItem);
                        }
                    }
                }

                if (storeToStoreReq.LineList.Count > 0)
                {
                    storeToStoreReqList.Add(storeToStoreReq);
                }
            }

            return storeToStoreReqList;
        }

        protected void rtvPart_OnNodeCheck(object sender, RadTreeNodeEventArgs e)
        {
            var checkedNodes = new List<PartListDto>();
            if (Session["CheckedNodes"] != null)
            {
                checkedNodes = (List<PartListDto>) Session["CheckedNodes"];
                var checkedNode = checkedNodes.FirstOrDefault(t => t.VatTu == e.Node.Value);
                if (e.Node.Checked && !e.Node.Value.Contains("ROOT$"))
                {
                    checkedNodes.Add(new PartListDto()
                    {
                        FullName = e.Node.Text,
                        VatTu = e.Node.Value
                    });
                }
                else
                {
                    checkedNodes.Remove(checkedNode);
                }

                Session.Add("CheckedNodes", checkedNodes);
            }
            else
            {
                if (e.Node.Checked && !e.Node.Value.Contains("ROOT$"))
                {
                    checkedNodes.Add(new PartListDto()
                    {
                        FullName = e.Node.Text,
                        VatTu = e.Node.Value
                    });
                }

                Session.Add("CheckedNodes", checkedNodes);

            }
        }
    }
}