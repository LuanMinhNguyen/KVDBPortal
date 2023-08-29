using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Aspose.Cells;
using EAM.Business.Services;
using EAM.Business.Services.Material;
using EAM.Data.Dto;
using EAM.Data.Entities;
using EAM.WebPortal.Resources.Utilities;
using Telerik.Web.UI;

namespace EAM.WebPortal.Control.Material
{
    public partial class StockSearching : System.Web.UI.Page
    {
        private readonly AA_MaterialRequestService mrService = new AA_MaterialRequestService();
        private readonly AA_MaterialRequestDetailService mrDetailService = new AA_MaterialRequestDetailService();
        private readonly EAMStoreProcedureService eamService = new EAMStoreProcedureService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["userId"]))
                {
                    Session.Remove("ItemId");
                    var userid = Request.QueryString["userId"].ToUpper();
                    var authGroup = ConfigurationManager.AppSettings.Get("AuthGroup");
                    var userInfor = Utility.GetUserInfor(userid);
                    if (userInfor != null)
                    {
                        Session.Add("UserInfor", userInfor);

                        var ds = eamService.GetDataSet("getPAvailable");
                        if (ds != null)
                        {
                            var partList = eamService.CreateListFromTable<PartListSYTAvailableDto>(ds.Tables[0]);
                            Session.Add("AvailablePart", partList);
                        }
                    }
                }
            }
        }
        protected void grdPartInStock_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var partStockList = this.GetPartStock();
            this.grdPartInStock.DataSource = partStockList;
        }

        private List<PartStockDto> GetPartStock()
        {
            DataSet ds;
            var partStockList = new List<PartStockDto>();
            ds = this.eamService.GetDataSet("uGetStockValue");
            if (ds != null)
            {
                partStockList = this.eamService.CreateListFromTable<PartStockDto>(ds.Tables[0]);
            }

            return partStockList;
        }
    }
}