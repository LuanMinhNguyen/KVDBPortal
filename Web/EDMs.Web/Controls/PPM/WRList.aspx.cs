// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   Class customer
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EDMs.Business.Services;
using EDMs.Business.Services.Document;
using EDMs.Data.Dto;
using EDMs.Data.Dto.PPM;
using EDMs.Data.Entities;
using EDMs.Web.Utilities.Sessions;
using Telerik.Web.UI;
using Unit = System.Web.UI.WebControls.Unit;

namespace EDMs.Web.Controls.PPM
{
    /// <summary>
    /// Class customer
    /// </summary>
    public partial class WRList : Page
    {
        /// <summary>
        /// The scope project service.
        /// </summary>
        private readonly EAMWorkRequestService wrService = new EAMWorkRequestService();

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
            Session.Add("SelectedMainMenu", "Quản Lý Công Việc");
            this.Title = ConfigurationManager.AppSettings.Get("AppName");


            if (!Page.IsPostBack)
            {
            }
        }

        protected void grdData_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var objectList = new List<EAMWorkRequest>();
            if (UserSession.Current.User.RoleId == 1)
            {
                objectList = this.wrService.GetAll();
            }
            else if (UserSession.Current.User.IsLeader.GetValueOrDefault())
            {
                objectList = this.wrService.GetAll().Where(t =>
                    t.CreatedById == UserSession.Current.User.Id.ToString()
                    || t.ManagerId == UserSession.Current.User.Id).ToList();
            }
            else
            {
                objectList = this.wrService.GetAll().Where(t => t.CreatedById == UserSession.Current.User.Id.ToString()).ToList();
            }

            grdData.DataSource = objectList;
        }

        protected void ckbEnableFilter_CheckedChange(object sender, EventArgs e)
        {
            this.grdData.AllowFilteringByColumn = ((CheckBox)sender).Checked;
            this.grdData.Height = Unit.Percentage(((CheckBox)sender).Checked ? 85 : 90);
            this.grdData.Rebind();
        }
    }
}

