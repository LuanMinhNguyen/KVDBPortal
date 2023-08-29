// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Business.Services.Scope;
using EDMs.Web.Utilities;

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using EDMs.Web.Utilities.Sessions;

    using System.Collections.Generic;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DocumentListInfoEditForm : Page
    {

        /// <summary>
        /// The service name.
        /// </summary>
        protected const string ServiceName = "EDMSFolderWatcher";

        private readonly OrganizationCodeService _OrganizationcodeService;

        private readonly MaterialService _MaterialService;
        private readonly WorkService _WorkcodeService;
        private readonly DrawingService _DrawingcodeService;
        private readonly PlantService _PlantService;
        private readonly AreaService _AreaService;
        private readonly UnitService _UnitService;
   

        /// <summary>
        /// The document package service.
        /// </summary>
        private readonly DQREDocumentMasterService _DQREDocumentService;

        /// <summary>
        /// The document type service.
        /// </summary>
        private readonly DocumentTypeService documentTypeService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        private readonly DocumentSequenceManagementService documentSequenceManagementService;

        private readonly DocumentNumberingService documentNumberingService;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DocumentListInfoEditForm()
        {
            
            this.documentTypeService = new DocumentTypeService();
            this.disciplineService = new DisciplineService();
            this.userService = new UserService();
            this._DQREDocumentService = new  DQREDocumentMasterService();
            this._AreaService = new AreaService();
            this._DrawingcodeService = new DrawingService();
            this._MaterialService = new MaterialService();
            this._OrganizationcodeService = new OrganizationCodeService();
            this._PlantService = new PlantService();
            this._UnitService = new UnitService();
            this._WorkcodeService = new WorkService();
            this.documentSequenceManagementService = new DocumentSequenceManagementService();
            this.documentNumberingService = new DocumentNumberingService();
        }

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
            if (!this.IsPostBack)
            {
                this.LoadComboData();
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    this.CreatedInfo.Visible = true;
                    Guid docId;
                    Guid.TryParse(this.Request.QueryString["docId"].ToString(),out docId);
                    var docObj = this._DQREDocumentService.GetById(docId);
                    if (docObj != null)
                    {

                        // this.txtProjectName.Text = docObj.ProjectFullName;

                        this.LoadDocInfo(docObj);

                        var createdUser = this.userService.GetByID(docObj.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + docObj.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (docObj.LastUpdatedBy != null && docObj.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(docObj.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + docObj.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
                        }
                        else
                        {
                            this.lblUpdated.Visible = false;
                        }
                    }
                }
                else
                {
                    this.CreatedInfo.Visible = false;
                }
            }
        }

        /// <summary>
        /// The btn cap nhat_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid )
            {
                DQREDocumentMaster docObj;
                if (!string.IsNullOrEmpty(this.Request.QueryString["docId"]))
                {
                    Guid docId;
                    Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);
                    docObj = this._DQREDocumentService.GetById(docId);
                    if (docObj != null)
                    {
                        
                            this.CollectData(ref docObj);
                        

                        docObj.LastUpdatedBy = UserSession.Current.User.Id;
                        docObj.LastUpdatedDate = DateTime.Now;
                        this._DQREDocumentService.Update(docObj);

                        
                    }
                }
                else
                {
                    docObj = new DQREDocumentMaster()
                    {
                        ID=Guid.NewGuid(),
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                        IsDelete = false
                    };

                    this.CollectData(ref docObj);
                    if (!this._DQREDocumentService.IsExistByDocNo(docObj.SystemDocumentNo))
                    {
                        this._DQREDocumentService.Insert(docObj);
                    }
                    else
                    {
                        this.blockError.Visible = true;
                        this.lblError.Text = "System Document No. is already exist. ";
                        return;
                    }       
                   

                    

                }

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", "CloseAndRebind();", true);
            }
        }

        /// <summary>
        /// The btncancel_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected void btncancel_Click(object sender, EventArgs e)
        {
            this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CancelEdit();", true);
        }

        

        /// <summary>
        /// The rad ajax manager 1_ ajax request.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument.Contains("CheckFileName"))
            {
                //var fileName = e.Argument.Split('$')[1];
                //var folderId = Convert.ToInt32(Request.QueryString["folId"]);
                
                
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            
            //add document type
            var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
            if (rtvDocumentType != null)
            {
                var listDocumentType = this.documentTypeService.GetAll();

                listDocumentType.Insert(0, new DocumentType() { ParentId = null, Name = "(None)" });

                rtvDocumentType.DataSource = listDocumentType;
                rtvDocumentType.DataFieldParentID = "ParentId";
                rtvDocumentType.DataTextField = "FullName";
                rtvDocumentType.DataValueField = "ID";
                rtvDocumentType.DataFieldID = "ID";
                rtvDocumentType.DataBind();

            }
            //add discipline
            var Disciplinelist = this.disciplineService.GetAll();
            Disciplinelist.Insert(0, new Discipline() { ID = 0, Code = string.Empty });
            this.ddlDiscipline.DataSource = Disciplinelist;
            this.ddlDiscipline.DataTextField = "FullName";
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataBind();

            //add Plant
            var plantList = this._PlantService.GetAll();
            plantList.Insert(0, new Plant() { ID = 0, Code = string.Empty });
            this.ddlPlant.DataSource = plantList;
            this.ddlPlant.DataTextField = "Name";
            this.ddlPlant.DataValueField = "ID";
            this.ddlPlant.DataBind();
            //add Area
            var AreaList = this._AreaService.GetAll();
            AreaList.Insert(0, new Area() { ID = 0, Code = string.Empty });
            this.ddlArea.DataSource = AreaList;
            this.ddlArea.DataTextField = "Code";
            this.ddlArea.DataValueField = "ID";
            this.ddlArea.DataBind();
            //add Area
            var UnitList = this._UnitService.GetAll();
            UnitList.Insert(0, new Data.Entities.Unit() { ID = 0, Code = string.Empty });
            this.ddlUnit.DataSource = UnitList;
            this.ddlUnit.DataTextField = "Code";
            this.ddlUnit.DataValueField = "ID";
            this.ddlUnit.DataBind();
            //add MaterialCode
            var MaterialcodeList = this._MaterialService.GetAll();
            MaterialcodeList.Insert(0, new  MaterialCode() { ID = 0, Code = string.Empty });
            this.ddlMaterialCode.DataSource = MaterialcodeList;
            this.ddlMaterialCode.DataTextField = "FullName";
            this.ddlMaterialCode.DataValueField = "ID";
            this.ddlMaterialCode.DataBind();


            //add WorkCode
            var WorkcodeList = this._WorkcodeService.GetAll();
            WorkcodeList.Insert(0, new WorkCode() { ID = 0, Code = string.Empty });
            this.ddlWorkcode.DataSource = WorkcodeList;
            this.ddlWorkcode.DataTextField = "FullName";
            this.ddlWorkcode.DataValueField = "ID";
            this.ddlWorkcode.DataBind();

            //add Drawing
            var Drawingcodelist = this._DrawingcodeService.GetAll();
            Drawingcodelist.Insert(0, new DrawingCode() { ID = 0, Code = string.Empty });
            this.ddlDrawingCode.DataSource = Drawingcodelist;
            this.ddlDrawingCode.DataTextField = "FullName";
            this.ddlDrawingCode.DataValueField = "ID";
            this.ddlDrawingCode.DataBind();
            //Originator
            var listorginizationlist = this._OrganizationcodeService.GetAll();
            listorginizationlist.Insert(0, new OrganizationCode() { ID = 0, Code = string.Empty });
            this.ddlOriginator.DataSource = listorginizationlist;
            this.ddlOriginator.DataTextField = "FullName";
            this.ddlOriginator.DataValueField = "ID";
            this.ddlOriginator.DataBind();
            this.ddlOriginating.DataSource = listorginizationlist;
            this.ddlOriginating.DataTextField = "FullName";
            this.ddlOriginating.DataValueField = "ID";
            this.ddlOriginating.DataBind();
            this.ddlReceiving.DataSource = listorginizationlist;
            this.ddlReceiving.DataTextField = "FullName";
            this.ddlReceiving.DataValueField = "ID";
            this.ddlReceiving.DataBind();



        }

        private void CollectData(ref DQREDocumentMaster docObj)
        {
            docObj.SystemDocumentNo = this.txtSystemDocNumber.Text;
      
            docObj.Title = this.txtDocumentTitle.Text ;
            docObj.OriginatorId= this.ddlOriginator.SelectedItem!= null? 
                                    Convert.ToInt32( this.ddlOriginator.SelectedValue)
                                    :0 ;
            docObj.OriginatorName= this.ddlOriginator.SelectedItem != null ? 
                                    this.ddlOriginator.SelectedItem.Text 
                                    : string.Empty;
             docObj.OriginatingOrganizationId= this.ddlOriginating.SelectedItem!= null?
                                       Convert.ToInt32(this.ddlOriginating.SelectedValue )
                                       :0;
            docObj.OriginatingOrganizationName = this.ddlOriginating.SelectedItem != null ?
                this.ddlOriginating.SelectedItem.Text
                : string.Empty;

            docObj.ReceivingOrganizationId= this.ddlReceiving.SelectedItem!=null?
                Convert.ToInt32(this.ddlReceiving.SelectedValue )
                :0;
            docObj.ReceivingOrganizationName = this.ddlReceiving.SelectedItem != null ?
                this.ddlReceiving.SelectedItem.Text
                : string.Empty;
           
            var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
            if (rtvDocumentType != null)
            {
                docObj.DocumentTypeId = Convert.ToInt32(rtvDocumentType.SelectedNode.Value);
                docObj.DocumentTypeName = rtvDocumentType.SelectedNode.Text;
            }

            docObj.DisciplineId= this.ddlDiscipline.SelectedItem!=null?
                Convert.ToInt32(this.ddlDiscipline.SelectedValue )
                :0;
            docObj.DisciplineName = this.ddlDiscipline.SelectedItem != null ?
                this.ddlDiscipline.SelectedItem.Text
                : string.Empty;

            docObj.MaterialCodeId= this.ddlMaterialCode.SelectedItem!= null?
                Convert.ToInt32(this.ddlMaterialCode.SelectedValue )
                :0;
            docObj.MaterialCodeName = this.ddlMaterialCode.SelectedItem != null ?
                this.ddlMaterialCode.SelectedItem.Text
                : string.Empty;
            
            docObj.WorkCodeId= this.ddlWorkcode.SelectedItem!=null?
                Convert.ToInt32(this.ddlWorkcode.SelectedValue)
                :0;
            docObj.WorkCodeName = this.ddlWorkcode.SelectedItem != null ?
                this.ddlWorkcode.SelectedItem.Text
                : string.Empty;

            docObj.DrawingCodeId= this.ddlDrawingCode.SelectedItem!= null?
                Convert.ToInt32(this.ddlDrawingCode.SelectedValue )
                :0;
            docObj.DrawingCodeName = this.ddlDrawingCode.SelectedItem != null ?
                this.ddlDrawingCode.SelectedItem.Text
                : string.Empty;
            docObj.EquipmentTagName= this.txtEquipmentTagNumber.Text ;
            docObj.DepartmentCode= this.txtDepartmentcode.Text ;
            docObj.MRSequenceNo= this.txtMRSequenceNo.Text ;
            docObj.DocumentSequenceNo= this.txtDocumentSequenceNo.Text ;
            docObj.SheetNo= this.txtsheetno.Text ;
            docObj.PlantId= this.ddlPlant.SelectedItem!= null?
                Convert.ToInt32(this.ddlPlant.SelectedValue)
                :0;
            docObj.PlantName = this.ddlPlant.SelectedItem != null ?
                this.ddlPlant.SelectedItem.Text
                : string.Empty;

            docObj.AreaId= this.ddlArea.SelectedItem!= null?
                Convert.ToInt32(this.ddlArea.SelectedValue )
                :0;
            docObj.AreaName = this.ddlArea.SelectedItem != null ?
                this.ddlArea.SelectedItem.Text
                : string.Empty;

             docObj.UnitId= this.ddlUnit.SelectedItem!=null?
                Convert.ToInt32(this.ddlUnit.SelectedValue)
                :0;
            docObj.UnitName= this.ddlUnit.SelectedItem!= null? this.ddlUnit.SelectedItem.Text:string.Empty;
        }

        private void LoadDocInfo(DQREDocumentMaster docObj)
        {
            this.txtSystemDocNumber.Text = docObj.SystemDocumentNo;
            this.txtDocumentTitle.Text = docObj.Title;
            this.ddlOriginator.SelectedValue = docObj.OriginatorId.ToString();
            this.ddlOriginating.SelectedValue = docObj.OriginatingOrganizationId.ToString();
            this.ddlReceiving.SelectedValue = docObj.ReceivingOrganizationId.ToString();
            var rtvDocumentType = (RadTreeView)this.ddlParent.Items[0].FindControl("rtvDocumentType");
            if (rtvDocumentType != null)
            {
                var selectedNode = rtvDocumentType.GetAllNodes().First(t => t.Value == docObj.DocumentTypeId.ToString());
                if (selectedNode != null)
                {
                    rtvDocumentType.GetAllNodes().First(t => t.Value == docObj.DocumentTypeId.ToString()).Selected = true;
                    this.ddlParent.Items[0].Text = selectedNode.Text;
                    rtvDocumentType.GetAllNodes().First(t => t.Value == docObj.DocumentTypeId.ToString()).ParentNode.Expanded = true;
                }
            }
            this.ddlDiscipline.SelectedValue = docObj.DisciplineId.ToString();
            this.ddlMaterialCode.SelectedValue = docObj.MaterialCodeId.ToString();
            this.ddlWorkcode.SelectedValue = docObj.WorkCodeId.ToString();
            this.ddlDrawingCode.SelectedValue = docObj.DrawingCodeId.ToString();
            this.txtEquipmentTagNumber.Text = docObj.EquipmentTagName;
            this.txtDepartmentcode.Text = docObj.DepartmentCode;
            this.txtMRSequenceNo.Text = docObj.MRSequenceNo;
            this.txtDocumentSequenceNo.Text = docObj.DocumentSequenceNo;
            this.txtsheetno.Text = docObj.SheetNo;
            this.ddlPlant.SelectedValue = docObj.PlantId.ToString();
            this.ddlArea.SelectedValue = docObj.AreaId.ToString();
            this.ddlUnit.SelectedValue = docObj.UnitId.ToString();

        }

      

       

        protected void ddlPlant_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                var listArea = this._AreaService.GetAllPlant(Convert.ToInt32(this.ddlPlant.SelectedValue));
            if(listArea != null)
            {
                listArea.Insert(0, new Area() { ID = 0, Code = string.Empty });
                this.ddlArea.DataSource = listArea;
                this.ddlArea.DataTextField = "Code";
                this.ddlArea.DataValueField = "ID";
                this.ddlArea.DataBind();
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            var UnitList = this._UnitService.GetAllArea(Convert.ToInt32(this.ddlArea.SelectedValue));
            if (UnitList != null)
            {
                UnitList.Insert(0, new Data.Entities.Unit() { ID = 0, Code = string.Empty });
                this.ddlUnit.DataSource = UnitList;
                this.ddlUnit.DataTextField = "Code";
                this.ddlUnit.DataValueField = "ID";
                this.ddlUnit.DataBind();
            }

        }

        protected void fileNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this.txtSystemDocNumber.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter System Document Number.";
                this.divDocNo.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["docId"]))
            {
                Guid docId;
                Guid.TryParse(this.Request.QueryString["docId"].ToString(), out docId);

                if (this._DQREDocumentService.IsExistByDocNo(this.txtSystemDocNumber.Text.Trim()) && docId == null)
                {
                    this.fileNameValidator.ErrorMessage = "System Document No. is already exist.";
                    this.divDocNo.Style["margin-bottom"] = "-5px;";
                    args.IsValid = false;
                }
            }
        }
    }
}