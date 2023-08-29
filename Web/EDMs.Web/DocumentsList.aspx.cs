// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data;
using System.Drawing;
using EDMs.Business.Services.Scope;

namespace EDMs.Web
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.ServiceProcess;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using Aspose.Cells;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities;
    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    using CheckBox = System.Web.UI.WebControls.CheckBox;
    using Label = System.Web.UI.WebControls.Label;
    using TextBox = System.Web.UI.WebControls.TextBox;

    /// <summary>
    /// Class customer
    /// </summary>
    public partial class DocumentsList : Page
    {

        private readonly DQREDocumentMasterService _dqredocumentService = new  DQREDocumentMasterService();



        private readonly PlantService _plantService = new PlantService();

        private readonly AreaService _areaService = new  AreaService();

        private readonly UnitService _unitService = new UnitService();
        private readonly MaterialService _MaterialService= new MaterialService();
        private readonly WorkService _WorkcodeService= new WorkService();
        private readonly DrawingService _DrawingcodeService = new DrawingService();

        private readonly DocumentTypeService documentTypeService = new DocumentTypeService();

        private readonly NotificationRuleService notificationRuleService = new NotificationRuleService();

        private readonly UserService userService = new UserService();

        private readonly AttachFileService attachFileService = new AttachFileService();

        private readonly AttachFilesPackageService attachFilesPackageService = new AttachFilesPackageService();

        private readonly DisciplineService disciplineService = new DisciplineService();

        private readonly RoleService roleService = new RoleService();



        private readonly OrganizationCodeService _organizationcodeService = new OrganizationCodeService();

        protected const string ServiceName = "EDMSFolderWatcher";

        public static RadTreeNode editedNode = null;

        /// <summary>
        /// The unread pattern.
        /// </summary>
        protected const string UnreadPattern = @"\(\d+\)";

        /// <summary>
        /// The list folder id.
        /// </summary>
        private List<int> listFolderId = new List<int>();

        /// <summary>
        /// The page_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = ConfigurationManager.AppSettings.Get("AppName");
            Session.Add("SelectedMainMenu", "Document Management");

            if (!Page.IsPostBack)
            {
                this.LoadObjectTree();
                Session.Add("IsListAll", false);

                if (!UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() && !UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    this.CustomerMenu.Items[0].Visible = false;
                    this.CustomerMenu.Items[1].Visible = false;
                    foreach (RadToolBarButton item in ((RadToolBarDropDown)this.CustomerMenu.Items[2]).Buttons)
                    {
                        if (item.Value == "Adminfunc")
                        {
                            item.Visible = false;
                        }
                    }

                    this.CustomerMenu.Items[3].Visible = false;

                    this.grdDocument.MasterTableView.GetColumn("IsSelected").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("EditColumn").Visible = false;
                    this.grdDocument.MasterTableView.GetColumn("DeleteColumn").Visible = false;
                }

                if (UserSession.Current.User.Role.IsAdmin.GetValueOrDefault() || UserSession.Current.User.IsDC.GetValueOrDefault())
                {
                    this.IsFullPermission.Value = "true";
                }

            }
        }

        /// <summary>
        /// The rad tree view 1_ node click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        ///  protected void rtvTreeNode_NodeClick1(object sender, RadTreeNodeEventArgs e)
        //{

        //}
       

        /// <summary>
        /// Load all document by folder
        /// </summary>
        /// <param name="isbind">
        /// The isbind.
        /// </param>
        protected void LoadDocuments(bool isbind = false, bool isListAll = false)
        {
           
            var docList = new List<DQREDocumentMaster>();
            if(this.rtvTreeNode.SelectedNode!= null)
            {
                var valuenode = Convert.ToInt32(this.rtvTreeNode.SelectedNode.Value);
                var tagnode = this.rtvTreeNode.SelectedNode.Target;
                var selectedNodesId =
                        this.rtvTreeNode.SelectedNode.GetAllNodes().Where(t=> t.Target==tagnode).Select(t => Convert.ToInt32(t.Value)).ToList();
                selectedNodesId.Insert(0, Convert.ToInt32(this.rtvTreeNode.SelectedNode.Value));
                selectedNodesId = selectedNodesId.Where(t => t != -1).ToList();
                if (!string.IsNullOrEmpty(tagnode))
                {
                    switch (tagnode)
                    {
                        case "Plant":
                            docList = this._dqredocumentService.GetAll().Where(t=>selectedNodesId.Contains(t.PlantId.GetValueOrDefault())).ToList();
                            break;
                        case "Area":
                            docList = this._dqredocumentService.GetAll().Where(t => selectedNodesId.Contains(t.AreaId.GetValueOrDefault())).ToList();
                            break;
                        case "Unit":
                            docList = this._dqredocumentService.GetAll().Where(t => selectedNodesId.Contains(t.UnitId.GetValueOrDefault())).ToList();
                            break;
                        case "DocType":
                            docList = this._dqredocumentService.GetAll().Where(t => selectedNodesId.Contains((int)t.DocumentTypeId)).ToList();
                            break;
                        case "Discipline":
                            docList = this._dqredocumentService.GetAll().Where(t => selectedNodesId.Contains(t.DisciplineId.GetValueOrDefault())).ToList();
                            break;
                        case "Organization":
                            docList = this._dqredocumentService.GetAll().Where(t => selectedNodesId.Contains(t.OriginatorId.GetValueOrDefault())).ToList();
                            break;
                        
                    }
                }
            }
            else
            {
                docList = this._dqredocumentService.GetAll().ToList();
            }
            

            this.grdDocument.DataSource = docList.OrderByDescending(t =>t.SystemDocumentNo);
        }

        /// <summary>
        /// RadAjaxManager1  AjaxRequest
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "ExportEMDRReport")
            {
                this.ExportCMDRDataFile();
            }
            else if (e.Argument == "DeleteAllDoc")
            {
                foreach (GridDataItem selectedItem in this.grdDocument.SelectedItems)
                {
                    
                    Guid docId;
                    Guid.TryParse(selectedItem.GetDataKeyValue("ID").ToString(), out docId);
                    var docObj = this._dqredocumentService.GetById(docId);
                    if (docObj != null)
                    {
                       
                            docObj.IsDelete = true;
                            docObj.LastUpdatedBy = UserSession.Current.User.Id;
                            docObj.LastUpdatedDate = DateTime.Now.Date;
                            this._dqredocumentService.Update(docObj);
                      
                    }
                }
                this.grdDocument.Rebind();
            }
           
            else if (e.Argument == "ExportMasterList")
            {
                var filePath = Server.MapPath("Exports") + @"\";
                var workbook = new Workbook();
                workbook.Open(filePath + @"Template\DQRE_MasterListTemplate.xls");
                var sheets = workbook.Worksheets;
                var tempSheet = sheets[0];
                var readMeSheet = sheets[1];

                var documenttypeList = this.documentTypeService.GetAll();
                var Disciplinelist = this.disciplineService.GetAll();
                var plantList = this._plantService.GetAll();
                var AreaList = this._areaService.GetAll();
                var UnitList = this._unitService.GetAll();
                var MaterialcodeList = this._MaterialService.GetAll();
                var WorkcodeList = this._WorkcodeService.GetAll();
                var Drawingcodelist = this._DrawingcodeService.GetAll();
            
                var listorginizationlist = this._organizationcodeService.GetAll();


                for (int i = 0; i < plantList.Count; i++)
                {
                    readMeSheet.Cells["B" + (7 + i)].PutValue(plantList[i].Name);
                    readMeSheet.Cells["C" + (7 + i)].PutValue(plantList[i].Description);
                }


                for (int i = 0; i < documenttypeList.Count; i++)
                {
                    readMeSheet.Cells["E" + (7 + i)].PutValue(documenttypeList[i].Code);
                    readMeSheet.Cells["F" + (7 + i)].PutValue(documenttypeList[i].Description);
                }

                for (int i = 0; i < Disciplinelist.Count; i++)
                {
                    readMeSheet.Cells["H" + (7 + i)].PutValue(Disciplinelist[i].Code);
                    readMeSheet.Cells["I" + (7 + i)].PutValue(Disciplinelist[i].Description);
                }

                for (int i = 0; i < AreaList.Count; i++)
                {
                    readMeSheet.Cells["K" + (7 + i)].PutValue(AreaList[i].Code);
                    readMeSheet.Cells["L" + (7 + i)].PutValue(AreaList[i].Description);
                }
                for (int i = 0; i < UnitList.Count; i++)
                {
                    readMeSheet.Cells["N" + (7 + i)].PutValue(UnitList[i].Code);
                    readMeSheet.Cells["O" + (7 + i)].PutValue(UnitList[i].Description);
                }
                for (int i = 0; i < MaterialcodeList.Count; i++)
                {
                    readMeSheet.Cells["Q" + (7 + i)].PutValue(MaterialcodeList[i].Code);
                    readMeSheet.Cells["R" + (7 + i)].PutValue(MaterialcodeList[i].Description);
                }
                for (int i = 0; i < listorginizationlist.Count; i++)
                {
                    readMeSheet.Cells["T" + (7 + i)].PutValue(listorginizationlist[i].Code);
                    readMeSheet.Cells["U" + (7 + i)].PutValue(listorginizationlist[i].Description);
                }
                for (int i = 0; i < WorkcodeList.Count; i++)
                {
                    readMeSheet.Cells["W" + (7 + i)].PutValue(WorkcodeList[i].Code);
                    readMeSheet.Cells["X" + (7 + i)].PutValue(WorkcodeList[i].Description);
                }
                for (int i = 0; i < Drawingcodelist.Count; i++)
                {
                    readMeSheet.Cells["Z" + (7 + i)].PutValue(Drawingcodelist[i].Code);
                    readMeSheet.Cells["AA" + (7 + i)].PutValue(Drawingcodelist[i].Description);
                }

               
              
           
             
                var rangeDocTypeList = readMeSheet.Cells.CreateRange("E7", "E" + (7 + (documenttypeList.Count == 0 ? 1 : documenttypeList.Count)));
                rangeDocTypeList.Name = "DocumentType";
                var rangDisciplineList = readMeSheet.Cells.CreateRange("H7", "H" + (7 + (Disciplinelist.Count == 0 ? 1 : Disciplinelist.Count)));
                rangDisciplineList.Name = "DisciplineCode";
                var rangAreaList = readMeSheet.Cells.CreateRange("K7", "K" +(7+ (AreaList.Count == 0 ? 1 : AreaList.Count)));
                rangAreaList.Name = "AreaCode";
                var rangUnitList = readMeSheet.Cells.CreateRange("N7", "N" + (7 + (UnitList.Count==0? 1: UnitList.Count)));
                rangUnitList.Name = "UnitCode";
                var rangMaterialList = readMeSheet.Cells.CreateRange("Q7", "Q" + (7 + (MaterialcodeList.Count == 0 ? 1 : MaterialcodeList.Count)));
                rangMaterialList.Name = "MaterialCode";
                var rangOriginatorList = readMeSheet.Cells.CreateRange("T7", "T" + (7 + (listorginizationlist.Count == 0 ? 1 : listorginizationlist.Count)));
                rangOriginatorList.Name = "OriginatorCode";
                var rangWorkList = readMeSheet.Cells.CreateRange("W7", "W" + (7 + (WorkcodeList.Count == 0 ? 1 : WorkcodeList.Count)));
                rangWorkList.Name = "WorkCode";
                var rangDrawingList = readMeSheet.Cells.CreateRange("Z7", "Z" + (7 + (Drawingcodelist.Count == 0 ? 1 : Drawingcodelist.Count)));
                rangDrawingList.Name = "DrawingCode";
                var rangPlantList = readMeSheet.Cells.CreateRange("B7", "B" + (7 + (plantList.Count == 0 ? 1 : plantList.Count)));
                rangPlantList.Name = "PlantCode";
              
                var validations = sheets[2].Validations;
                this.CreateValidation(rangOriginatorList.Name, validations, 2, 1000, 8, 8);
                this.CreateValidation(rangOriginatorList.Name, validations, 2, 1000, 9, 9);
                this.CreateValidation(rangOriginatorList.Name, validations, 2, 1000, 10, 10);
                this.CreateValidation(rangeDocTypeList.Name, validations, 2, 1000, 11, 11);
                this.CreateValidation(rangDisciplineList.Name, validations, 2, 1000, 12, 12);
                this.CreateValidation(rangMaterialList.Name, validations, 2, 1000, 13, 13);
                this.CreateValidation(rangWorkList.Name, validations, 2, 1000, 14, 14);
                this.CreateValidation(rangDrawingList.Name, validations, 2, 1000, 15, 15);
                this.CreateValidation(rangPlantList.Name, validations, 2, 1000, 16, 16);
                this.CreateValidation(rangAreaList.Name, validations, 2, 1000, 17, 17);
                this.CreateValidation(rangUnitList.Name, validations, 2, 1000, 18, 18);
                


                workbook.Worksheets[0].IsVisible = false;
                    //workbook.Worksheets.RemoveAt(2);

                    var filename = DateTime.Now.ToString("ddmmyyyy")+"_" + "$" + "MasterListTemplate.xls";
                    workbook.Save(filePath + filename);
                    this.DownloadByWriteByte(filePath + filename, filename, true);

                
            }
           
           
            else if (e.Argument == "RebindAndNavigate")
            {
                this.grdDocument.Rebind();
            }
            else if (e.Argument == "SendNotification")
            {
                var listDisciplineId = new List<int>();
                var listSelectedDoc = new List<Document>();
                var count = 0;
                foreach (GridDataItem item in this.grdDocument.MasterTableView.Items)
                {
                    var cboxSelected = (CheckBox)item["IsSelected"].FindControl("IsSelected");
                    if (cboxSelected.Checked)
                    {
                        count += 1;
                        var docItem = new Document();
                        var disciplineId = item["DisciplineID"].Text != @"&nbsp;"
                            ? item["DisciplineID"].Text
                            : string.Empty;
                        if (!string.IsNullOrEmpty(disciplineId) && disciplineId != "0")
                        {
                            listDisciplineId.Add(Convert.ToInt32(disciplineId));

                            docItem.ID = count;
                            docItem.DocumentNumber = item["DocumentNumber"].Text != @"&nbsp;"
                                ? item["DocumentNumber"].Text
                                : string.Empty;
                            docItem.Title = item["Title"].Text != @"&nbsp;"
                                ? item["Title"].Text
                                : string.Empty;
                            docItem.RevisionName = item["Revision"].Text != @"&nbsp;"
                                ? item["Revision"].Text
                                : string.Empty;
                            docItem.FilePath = item["FilePath"].Text != @"&nbsp;"
                                ? item["FilePath"].Text
                                : string.Empty;
                            docItem.DisciplineID = Convert.ToInt32(disciplineId);
                            listSelectedDoc.Add(docItem);
                        }
                    }
                }

                listDisciplineId = listDisciplineId.Distinct().ToList();

                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    Host = ConfigurationManager.AppSettings["Host"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    Credentials =
                        new NetworkCredential(UserSession.Current.User.Email,
                            Utility.Decrypt(UserSession.Current.User.HashCode))
                };

                foreach (var disciplineId in listDisciplineId)
                {
                    var notificationRule = this.notificationRuleService.GetAllByDiscipline(disciplineId);

                    if (notificationRule != null)
                    {
                        var message = new MailMessage();
                        message.From = new MailAddress(UserSession.Current.User.Email, UserSession.Current.User.FullName);
                        message.Subject = "Test send notification from EDMs";
                        message.BodyEncoding = new UTF8Encoding();
                        message.IsBodyHtml = true;
                        message.Body = @"******<br/>
                                        Dear users,<br/><br/>

                                        Please be informed that the following documents are now available on the BDPOC Document Library System for your information.<br/><br/>

                                        <table border='1' cellspacing='0'>
	                                        <tr>
		                                        <th style='text-align:center; width:40px'>No.</th>
		                                        <th style='text-align:center; width:350px'>Document number</th>
		                                        <th style='text-align:center; width:350px'>Document title</th>
		                                        <th style='text-align:center; width:60px'>Revision</th>
	                                        </tr>";

                        if (!string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listUserId =
                                notificationRule.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            foreach (var userId in listUserId)
                            {
                                var user = this.userService.GetByID(userId);
                                if (user != null)
                                {
                                    message.To.Add(new MailAddress(user.Email));
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(notificationRule.ReceiveGroupId) &&
                                 string.IsNullOrEmpty(notificationRule.ReceiverListId))
                        {
                            var listGroupId =
                                notificationRule.ReceiveGroupId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            var listUser = this.userService.GetSpecialListUser(listGroupId);
                            foreach (var user in listUser)
                            {
                                message.To.Add(new MailAddress(user.Email));
                            }
                        }

                        var subBody = string.Empty;
                        foreach (var document in listSelectedDoc)
                        {
                            var port = ConfigurationSettings.AppSettings.Get("DocLibPort");
                            if (document.DisciplineID == disciplineId)
                            {
                                subBody += @"<tr>
                                <td>" + document.ID + @"</td>
                                <td><a href='http://" + Server.MachineName +
                                           (!string.IsNullOrEmpty(port) ? ":" + port : string.Empty)
                                           + document.FilePath + "' download='" + document.DocumentNumber + "'>"
                                           + document.DocumentNumber + @"</a></td>
                                <td>"
                                           + document.Title + @"</td>
                                <td>"
                                           + document.RevisionName + @"</td>";
                            }
                        }


                        message.Body += subBody + @"</table>
                                        <br/><br/>
                                        Thanks and regards,<br/>
                                        ******";

                        smtpClient.Send(message);
                    }
                }
            }

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
                Response.AddHeader("Content-Type", "Application/octet-stream");
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
        /// <summary>
        /// The rad grid 1_ on need data source.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            var isListAll = this.Session["IsListAll"] != null && Convert.ToBoolean(this.Session["IsListAll"]);
            this.LoadDocuments(false, isListAll);
        }

        /// <summary>
        /// The grd khach hang_ delete command.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var item = (GridDataItem)e.Item;
           
            Guid docId;
            Guid.TryParse(item.GetDataKeyValue("ID").ToString(), out docId);
            var docObj = this._dqredocumentService.GetById(docId);
            if (docObj != null)
            {
                docObj.IsDelete = true;
                docObj.LastUpdatedBy = UserSession.Current.User.Id;
                docObj.LastUpdatedDate = DateTime.Now.Date;
                this._dqredocumentService.Update(docObj);
            }
        }

      
        /// <summary>
        /// The grd document_ item data bound.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void grdDocument_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
            if (e.Item is GridDataItem)
            {
                var item = e.Item as GridDataItem;
                if (item["HasAttachFile"].Text == "True")
                {
                    item.BackColor = Color.Aqua;
                    item.BorderColor = Color.Aqua;
                }
            }

            
        }

      

        protected void grdDocument_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
            }
        }

        protected void ckbEnableFilter_OnCheckedChanged(object sender, EventArgs e)
        {
            this.grdDocument.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdDocument.Rebind();
        }

        protected void radTreeFolder_OnNodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "Images/folderdir16.png";
        }

      

        private void CreateValidation(string formular, ValidationCollection objValidations, int startRow, int endRow, int startColumn, int endColumn)
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
            validation.ErrorMessage = "Please select item from the list";

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

        private void LoadObjectTree()
        {
            // add node plant
            var plantlist = this._plantService.GetAll();
            foreach(var item in plantlist)
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = item.Name +", " + item.Description;
                node.Value = item.ID.ToString();
                node.ImageUrl = "~/Images/status1.png";
                node.Target = "Plant";
                //Add node Area
                var arealist = this._areaService.GetAllPlant(item.ID);
                foreach( var area in arealist)
                {
                   var nodearea = new RadTreeNode();
                    nodearea.Text = area.Code ;
                    nodearea.Value = area.ID.ToString();
                    nodearea.ImageUrl = "~/Images/material.png";
                    nodearea.Target = "Area";
                    //Add node Unit
                    var unitlist = this._unitService.GetAllArea(area.ID);
                    foreach(var unit in unitlist)
                    {
                        var nodeunit = new RadTreeNode();
                        nodeunit.Text = unit.Code + ", " + unit.Description;
                        nodeunit.Value = unit.ID.ToString();
                        nodeunit.ImageUrl = "~/Images/checkList.png";
                        nodeunit.Target = "Unit";
                        nodearea.Nodes.Add(nodeunit);
                    }

                    node.Nodes.Add(nodearea);
                }
              
                this.rtvTreeNode.Nodes.Add(node);
            }

            // Add node DocumentType 
            var documenttypelist = this.documentTypeService.GetAll();
            var root = new RadTreeNode();
            root.Text = "Document type";
            root.ImageUrl = "~/Images/type1.png";
            root.Value = "-1";
            root.Target = "DocType";
            foreach(var type  in documenttypelist.Where(t=> t.ParentId==null))
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = type.Code + ", " + type.Description;
                node.Value = type.ID.ToString();
                node.ImageUrl = "~/Images/type1.png";
                node.Target = "DocType";
                foreach (var types in documenttypelist.Where(t => t.ParentId == type.ID))
                {
                    RadTreeNode nodes = new RadTreeNode();
                    nodes.Text = types.Code + ", " + types.Description;
                    nodes.Value = types.ID.ToString();
                    nodes.ImageUrl = "~/Images/type1.png";
                    nodes.Target = "DocType";
                    node.Nodes.Add(nodes);
                }
                root.Nodes.Add(node);
            }
            this.rtvTreeNode.Nodes.Add(root);
            // Add node Discipline
            var disciplinelist = this.disciplineService.GetAll();
            root = new RadTreeNode();
            root.Text = "Discipline";
            root.ImageUrl = "~/Images/stage.png";
            root.Value = "-1";
            root.Target = "Discipline";
            foreach (var type in disciplinelist)
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = type.Code + ", " + type.Description;
                node.Value = type.ID.ToString();
                node.ImageUrl = "~/Images/stage.png";
                node.Target = "Discipline";
                root.Nodes.Add(node);
            }
            this.rtvTreeNode.Nodes.Add(root);
            // Add Origination node

            var originationlist = this._organizationcodeService.GetAll();
            root = new RadTreeNode();
            root.Text = "Organization";
            root.ImageUrl = "~/Images/workflow.png";
            root.Value = "-1";
            root.Target = "Organization";
            foreach (var item in originationlist)
            {
                RadTreeNode node = new RadTreeNode();
                node.Text = item.Code + ", " + item.Description;
                node.Value = item.ID.ToString();
                node.ImageUrl = "~/Images/workflow.png";
                node.Target = "Organization";
                root.Nodes.Add(node);
            }
            this.rtvTreeNode.Nodes.Add(root);
            
        }

        

   

        protected void rtvTreeNode_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.grdDocument.CurrentPageIndex = 0;
            this.grdDocument.Rebind();
        }

        protected void grdDocument_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridFilteringItem)
            {
                var filterItem = (GridFilteringItem)e.Item;
                var selectedProperty = new List<string>();

                var ddlFilterRev = (RadComboBox)filterItem.FindControl("ddlFilterRev");
            }
        }

      
        
        protected void rtvTreeNode_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = @"Images/discipline.png";
        }

        protected void ddlProject_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            e.Item.ImageUrl = @"Images/project.png";
        }


        private void ExportCMDRDataFile()
        {
            var filePath = Server.MapPath("Exports") + @"\";
            var workbook = new Workbook();
            workbook.Open(filePath + @"Template\DQRE_CMDRMasterListTemplate.xlsm");

            var dataSheet = workbook.Worksheets[2];
            var duplicateSheet = workbook.Worksheets[3];
            var tempSheet = workbook.Worksheets[0];
            var countCol = 11;
            var totalColAdded = 0;
            var dtFull = new DataTable();
            var filename = "Master List Data File_Cut off - " + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsm";

            // dataSheet.Cells["E1"].PutValue(this.ddlProject.Text);
            dataSheet.Cells["U4"].PutValue(DateTime.Now.ToString("dd/MM/yyyy"));

            dtFull.Columns.AddRange(new[]
            {
                new DataColumn("DocId", typeof (String)),
                new DataColumn("NoIndex", typeof (String)),
                new DataColumn("System", typeof (String)),
                new DataColumn("DocTitle", typeof (String)),
                new DataColumn("EquipmentTag", typeof(String)),
                new DataColumn("DepartmentCode", typeof(String)),
                new DataColumn("MRSequenceNo", typeof(String)),
                new DataColumn("DocSequenceNo", typeof(String)),
                new DataColumn("SheetNo", typeof(String)),
                new DataColumn("Originator", typeof (String)),
                new DataColumn("Originatoring", typeof (String)),
                new DataColumn("Receiving", typeof (String)),
                new DataColumn("DocTypeCode", typeof (String)),
                new DataColumn("DisciplineCode", typeof (String)),
                new DataColumn("Material", typeof (String)),
                new DataColumn("Work", typeof (String)),
                new DataColumn("Drawing", typeof (String)),
                new DataColumn("Plant", typeof (String)),
                new DataColumn("Area", typeof (String)),
                new DataColumn("Unit", typeof (String)),
            });

            List<Guid> ListId = new List<Guid>();

            this.grdDocument.AllowPaging = false;
            this.grdDocument.Rebind();
            foreach (GridDataItem row in this.grdDocument.Items) // loops through each rows in RadGrid
            {
                Guid docId;
                Guid.TryParse(row.GetDataKeyValue("ID").ToString(), out docId);
                ListId.Add(docId);
            }
            this.grdDocument.AllowPaging = true;
            this.grdDocument.Rebind();
            var docList = this._dqredocumentService.GetAllDocList(ListId);
            var docGroupByDisciplineList = docList.GroupBy(t => t.DisciplineName);
            foreach (var docGroupByDiscipline in docGroupByDisciplineList)
            {
                var disciplineRowCount = 1;
                var docListOfDiscipline = docGroupByDiscipline.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["System"] = docGroupByDiscipline.Key;
                dtFull.Rows.Add(dataRow);


                foreach (var docObj in docListOfDiscipline)
                {
                    dataRow = dtFull.NewRow();

                    dataRow["DocId"] = docObj.ID;
                    dataRow["NoIndex"] = disciplineRowCount;
                    dataRow["System"] = docObj.SystemDocumentNo;
                    dataRow["DocTitle"] = docObj.Title;
                    dataRow["EquipmentTag"] = docObj.EquipmentTagName;
                    dataRow["DepartmentCode"] = docObj.DepartmentCode;
                    dataRow["MRSequenceNo"] = docObj.MRSequenceNo;
                    dataRow["DocSequenceNo"] = docObj.DocumentSequenceNo;
                    dataRow["SheetNo"] = docObj.SheetNo;
                    dataRow["Originator"] = docObj.OriginatorName;
                    dataRow["Originatoring"] = docObj.OriginatingOrganizationName;
                    dataRow["Receiving"] = docObj.ReceivingOrganizationName;
                    dataRow["DocTypeCode"] = docObj.DocumentTypeName;
                    dataRow["DisciplineCode"] = docObj.DisciplineName;
                    dataRow["Material"] = docObj.MaterialCodeName;
                    dataRow["Work"] = docObj.WorkCodeName;
                    dataRow["Drawing"] = docObj.DrawingCodeName;
                    dataRow["Plant"] = docObj.PlantName;
                    dataRow["Area"] = docObj.AreaName;
                    dataRow["Unit"] = docObj.UnitName;
                    disciplineRowCount += 1;
                    dtFull.Rows.Add(dataRow);
                }
            }
            dataSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);
          //  duplicateSheet.Cells.ImportDataTable(dtFull, false, 7, 1, dtFull.Rows.Count, dtFull.Columns.Count, false);

            var validations = dataSheet.Validations;

            dataSheet.Cells["A7"].PutValue(dtFull.Rows.Count);
           


            workbook.Save(filePath + filename);
            this.DownloadByWriteByte(filePath + filename, filename, true);

        }

        private void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            Response.End();
        }

        protected void ckbShowAll_CheckedChange(object sender, EventArgs e)
        {
            this.grdDocument.Rebind();
        }

       
    }
}