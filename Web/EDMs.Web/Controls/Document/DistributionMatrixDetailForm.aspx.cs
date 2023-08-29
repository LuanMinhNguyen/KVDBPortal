// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services.Document;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.Entities;
using Telerik.Web.UI;

namespace EDMs.Web.Controls.Document
{
    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class DistributionMatrixDetailForm : Page
    {

        private readonly DisciplineService disciplineService;
        private readonly DocumentTypeService documentTypeService;
        private readonly UnitService unitService;
        private readonly UserService userService;
        private readonly DistributionMatrixService dmService;
        private readonly DistributionMatrixDetailService dmDetailService;
        private readonly GroupCodeService groupCodeService;
        private int DistributionMatrixId
        {
            get
            {
                return Convert.ToInt32(this.Request.QueryString["disMatrixId"]);
            }
        }

        private DistributionMatrix DistributionMatrixObj
        {
            get { return this.dmService.GetById(this.DistributionMatrixId); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public DistributionMatrixDetailForm()
        {
            this.disciplineService = new DisciplineService();
            this.documentTypeService = new DocumentTypeService();
            this.userService = new UserService();
            this.dmService = new DistributionMatrixService();
            this.dmDetailService = new DistributionMatrixDetailService();
            this.groupCodeService = new GroupCodeService();
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
            }
        }

        protected void grdDocument_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var dmDetailList = this.dmDetailService.GetAllByDM(this.DistributionMatrixId).OrderBy(t=> t.UserName).ToList();
            var dmUserList = dmDetailList.Select(t => t.UserId).Distinct().Select(t => this.userService.GetByID(t.GetValueOrDefault())).ToList();
            var dtFull = new DataTable();
            if (this.DistributionMatrixObj != null)
            {
                this.BuildMatrixDetailView(dtFull, dmDetailList, dmUserList);

                //switch (this.DistributionMatrixObj.TypeId)
                //{
                //    // Document have Material/Work Code
                //    case 1:
                //        this.MaterialWorkCodeMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // Document have Drawing Code (00) Matrix
                //    case 2:
                //        this.DrawingCode00MatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // Document have Drawing Code Matrix
                //    case 3:
                //        this.DrawingCodeMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // AU, CO, PLG, QIR, GTC, PO Matrix
                //    case 4:
                //        this.AU_CO_PLGMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // EL, ML Matrix
                //    case 5:
                //        this.EL_MLMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // PP Matrix
                //    case 6:
                //        this.PPMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //    // Vendor Document Matrix
                //    case 7:
                //        this.VendorDocumentMatrixDetail(dtFull, dmDetailList, dmUserList);
                //        break;
                //}
            }

            this.grdDocument.DataSource = dtFull;
        }

        private void BuildMatrixDetailView(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("GroupCode", typeof (String)),
                //new DataColumn("Discipline", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByGroupCodeList = dmDetailList.GroupBy(t => t.GroupCodeName);
            foreach (var dmDetailGroupByGroupCode in dmDetailGroupByGroupCodeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByGroupCode.ToList();
                var dataRow = dtFull.NewRow();
                dataRow["GroupCode"] = dmDetailGroupByGroupCode.Key;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    if (!string.IsNullOrEmpty(dataRow[dmDetail.UserName.Split('/')[0]].ToString()))
                    {
                        dataRow[dmDetail.UserName.Split('/')[0]] = dataRow[dmDetail.UserName.Split('/')[0]]+"; "+ dmDetail.ActionTypeName;
                    }
                    else
                    {
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }
                   
                }

                dtFull.Rows.Add(dataRow);
            }
        }

        private void VendorDocumentMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("MaterialCode", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByMaterialCodeList = dmDetailList.GroupBy(t => t.MaterialCodeName);
            foreach (var dmDetailGroupByMaterialCode in dmDetailGroupByMaterialCodeList)
            {
                var dmDetailOfMaterialCodeList = dmDetailGroupByMaterialCode.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfMaterialCodeList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["MaterialCode"] = dmDetailGroupByMaterialCode.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }
        }

        private void PPMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("SerialNo", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }
            
            var dmDetailGroupByDocTypeList = dmDetailList.GroupBy(t => t.DocTypeName);
            foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                var dataRow = dtFull.NewRow();
                
                dataRow["DocType"] = dmDetailGroupByDocType.Key;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    dataRow["SerialNo"] = dmDetail.SerialNo;
                    dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                }

                dtFull.Rows.Add(dataRow);
            }
        }

        private void EL_MLMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Unit", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByUnitList = dmDetailList.GroupBy(t => t.UnitCodeName);
            foreach (var dmDetailGroupByUnit in dmDetailGroupByUnitList)
            {
                var dmDetailOfUnitList = dmDetailGroupByUnit.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfUnitList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Unit"] = dmDetailGroupByUnit.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }
        }

        private void AU_CO_PLGMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                //new DataColumn("Discipline", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDocTypeList = dmDetailList.GroupBy(t => t.DocTypeName);
            foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
            {
                var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                var dataRow = dtFull.NewRow();
                //dataRow["Discipline"] = dmDetailGroupByDiscipline.Key;
                dataRow["DocType"] = dmDetailGroupByDocType.Key;
                foreach (var dmDetail in dmDetailOfDisAndDocType)
                {
                    //dataRow["ID"] = dmDetail.ID;
                    dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                }

                dtFull.Rows.Add(dataRow);

            }
        }

        private void DrawingCodeMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDisciplineList = dmDetailList.GroupBy(t => t.DisciplineName);
            foreach (var dmDetailGroupByDiscipline in dmDetailGroupByDisciplineList)
            {
                var dmDetailOfDisciplineList = dmDetailGroupByDiscipline.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfDisciplineList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Discipline"] = dmDetailGroupByDiscipline.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }
        }

        private void DrawingCode00MatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Unit", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByUnitList = dmDetailList.GroupBy(t => t.UnitCodeName);
            foreach (var dmDetailGroupByUnit in dmDetailGroupByUnitList)
            {
                var dmDetailOfUnitList = dmDetailGroupByUnit.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfUnitList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Unit"] = dmDetailGroupByUnit.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }
        }

        private void MaterialWorkCodeMatrixDetail(DataTable dtFull, List<DistributionMatrixDetail> dmDetailList, List<User> dmUserList)
        {
            dtFull.Columns.AddRange(new[]
            {
                //new DataColumn("ID", typeof (int)),
                //new DataColumn("Empty", typeof (String)),
                new DataColumn("DocType", typeof (String)),
                new DataColumn("Discipline", typeof (String)),
            });

            for (int i = 0; i < dmUserList.Count; i++)
            {
                var userColumn = new DataColumn(dmUserList[i].Username, typeof(String));
                dtFull.Columns.Add(userColumn);
            }

            var dmDetailGroupByDisciplineList = dmDetailList.GroupBy(t => t.DisciplineName);
            foreach (var dmDetailGroupByDiscipline in dmDetailGroupByDisciplineList)
            {
                var dmDetailOfDisciplineList = dmDetailGroupByDiscipline.ToList();
                var dmDetailGroupByDocTypeList = dmDetailOfDisciplineList.GroupBy(t => t.DocTypeName);
                foreach (var dmDetailGroupByDocType in dmDetailGroupByDocTypeList)
                {
                    var dmDetailOfDisAndDocType = dmDetailGroupByDocType.ToList();
                    var dataRow = dtFull.NewRow();
                    dataRow["Discipline"] = dmDetailGroupByDiscipline.Key;
                    dataRow["DocType"] = dmDetailGroupByDocType.Key;
                    foreach (var dmDetail in dmDetailOfDisAndDocType)
                    {
                        //dataRow["ID"] = dmDetail.ID;
                        dataRow[dmDetail.UserName.Split('/')[0]] = dmDetail.ActionTypeName;
                    }

                    dtFull.Rows.Add(dataRow);
                }

            }
        }

        protected void ajaxDocument_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }

        protected void grdDocument_OnPreRender(object sender, EventArgs e)
        {
            foreach (GridColumn column in grdDocument.MasterTableView.Columns)
            {
                column.HeaderStyle.Width = 80;
                column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}