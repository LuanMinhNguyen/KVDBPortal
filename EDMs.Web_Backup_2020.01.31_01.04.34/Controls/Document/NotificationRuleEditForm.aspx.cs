// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerEditForm.aspx.cs" company="">
//   
// </copyright>
// <summary>
//   The customer edit form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Web.Controls.Document
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using EDMs.Business.Services;
    using EDMs.Business.Services.Document;
    using EDMs.Business.Services.Library;
    using EDMs.Business.Services.Security;
    using EDMs.Data.Entities;
    using System.Linq;

    using EDMs.Web.Utilities.Sessions;

    using Telerik.Web.UI;

    /// <summary>
    /// The customer edit form.
    /// </summary>
    public partial class NotificationRuleEditForm : Page
    {
        /// <summary>
        /// The role service.
        /// </summary>
        private readonly RoleService roleService;

        /// <summary>
        /// The user service.
        /// </summary>
        private readonly UserService userService;

        /// <summary>
        /// The attention service.
        /// </summary>
        private readonly AttentionService attentionService;

        /// <summary>
        /// The transmittal service.
        /// </summary>
        private readonly TransmittalService transmittalService;

        /// <summary>
        /// The discipline service.
        /// </summary>
        private readonly DisciplineService disciplineService;

        /// <summary>
        /// The notification rule service.
        /// </summary>
        private readonly NotificationRuleService notificationRuleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentInfoEditForm"/> class.
        /// </summary>
        public NotificationRuleEditForm()
        {
            this.userService = new UserService();
            this.transmittalService = new TransmittalService();
            this.attentionService = new AttentionService();
            this.roleService = new RoleService();
            this.disciplineService = new DisciplineService();
            this.notificationRuleService = new NotificationRuleService();
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

                if (!string.IsNullOrEmpty(this.Request.QueryString["notId"]))
                {
                    this.CreatedInfo.Visible = true;

                    var objNotifi = this.notificationRuleService.GetById(Convert.ToInt32(this.Request.QueryString["notId"]));
                    if (objNotifi != null)
                    {
                        this.txtName.Text = objNotifi.Name;
                        this.ddlDiscipline.SelectedValue = objNotifi.DisciplineID.ToString();
                        this.txtDescription.Text = objNotifi.Description;

                        this.ddlReceiveGroup.Text = objNotifi.ReceiveGroup;
                        if (!string.IsNullOrEmpty(objNotifi.ReceiveGroup))
                        {
                            var listGroupId = objNotifi.ReceiveGroupId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                            
                            foreach (RadComboBoxItem item in this.ddlReceiveGroup.Items)
                            {
                                if (listGroupId.Contains(Convert.ToInt32(item.Value)))
                                {
                                    item.Checked = true;
                                }
                            }

                            var listUser = this.userService.GetSpecialListUser(listGroupId);
                            this.ddlReceiver.DataSource = listUser;
                            this.ddlReceiver.DataValueField = "Id";
                            this.ddlReceiver.DataTextField = "FullName";
                            this.ddlReceiver.DataBind();

                            if (!string.IsNullOrEmpty(objNotifi.ReceiverListId))
                            {
                                var listUserId = objNotifi.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                                foreach (RadComboBoxItem item in ddlReceiver.Items)
                                {
                                    if (listUserId.Contains(Convert.ToInt32(item.Value)))
                                    {
                                        item.Checked = true;
                                    }
                                }
                            }
                        }

                        var createdUser = this.userService.GetByID(objNotifi.CreatedBy.GetValueOrDefault());

                        this.lblCreated.Text = "Created at " + objNotifi.CreatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (createdUser != null ? createdUser.FullName : string.Empty);

                        if (objNotifi.LastUpdatedBy != null && objNotifi.LastUpdatedDate != null)
                        {
                            this.lblCreated.Text += "<br/>";
                            var lastUpdatedUser = this.userService.GetByID(objNotifi.LastUpdatedBy.GetValueOrDefault());
                            this.lblUpdated.Text = "Last modified at " + objNotifi.LastUpdatedDate.GetValueOrDefault().ToString("dd/MM/yyyy hh:mm tt") + " by " + (lastUpdatedUser != null ? lastUpdatedUser.FullName : string.Empty);
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
            if (this.Page.IsValid)
            {
                var receiveGroupList = string.Empty;
                var receiveGroupListId = string.Empty;
                var receiverList = string.Empty;
                var receiverListId = string.Empty;
                foreach (var groupItem in this.ddlReceiveGroup.CheckedItems)
                {
                    receiveGroupList += groupItem.Text + "<br/>";
                    receiveGroupListId += groupItem.Value + ";";
                }

                foreach (var userItem in this.ddlReceiver.CheckedItems)
                {
                    receiverList += userItem.Text + "<br/>";
                    receiverListId += userItem.Value + ";";
                }

                if (!string.IsNullOrEmpty(this.Request.QueryString["notId"]))
                {
                    var notId = Convert.ToInt32(this.Request.QueryString["notId"]);
                    var objNotifi = this.notificationRuleService.GetById(notId);
                    if (objNotifi != null)
                    {
                        objNotifi.Name = this.txtName.Text.Trim();
                        objNotifi.DisciplineID = Convert.ToInt32(this.ddlDiscipline.SelectedValue);
                        objNotifi.Description = this.txtDescription.Text;

                        objNotifi.ReceiveGroup = this.ddlReceiveGroup.CheckedItems.Count > 0 ? receiveGroupList.Substring(0, receiveGroupList.Length - 5) : string.Empty;
                        objNotifi.ReceiveGroupId = this.ddlReceiveGroup.CheckedItems.Count > 0 ? receiveGroupListId.Substring(0, receiveGroupListId.Length - 1) : string.Empty;
                        objNotifi.ReceiverList = this.ddlReceiver.CheckedItems.Count > 0 ? receiverList.Substring(0, receiverList.Length - 5) : string.Empty;
                        objNotifi.ReceiverListId = this.ddlReceiver.CheckedItems.Count > 0 ? receiverListId.Substring(0, receiverListId.Length - 1) : string.Empty;

                        objNotifi.LastUpdatedBy = UserSession.Current.User.Id;
                        objNotifi.LastUpdatedDate = DateTime.Now;

                        this.notificationRuleService.Update(objNotifi);
                        this.Session.Remove("ListGroupId");
                    }
                }
                else
                {
                    

                    var objNotifi = new NotificationRule()
                    {
                        Name = this.txtName.Text.Trim(),
                        DisciplineID = Convert.ToInt32(this.ddlDiscipline.SelectedValue),
                        Description = this.txtDescription.Text,
                        ReceiveGroup = this.ddlReceiveGroup.Text,
                        ReceiverList = this.ddlReceiver.Text,
                        CreatedBy = UserSession.Current.User.Id,
                        CreatedDate = DateTime.Now,
                    };

                    objNotifi.ReceiveGroup = this.ddlReceiveGroup.CheckedItems.Count > 0 ? receiveGroupList.Substring(0, receiveGroupList.Length - 5) : string.Empty;
                    objNotifi.ReceiveGroupId = this.ddlReceiveGroup.CheckedItems.Count > 0 ? receiveGroupListId.Substring(0, receiveGroupListId.Length - 1) : string.Empty;
                    objNotifi.ReceiverList = this.ddlReceiver.CheckedItems.Count > 0 ? receiverList.Substring(0, receiverList.Length - 5) : string.Empty;
                    objNotifi.ReceiverListId = this.ddlReceiver.CheckedItems.Count > 0 ? receiverListId.Substring(0, receiverListId.Length - 1) : string.Empty;

                    this.notificationRuleService.Insert(objNotifi);
                }

                this.ClientScript.RegisterStartupScript(this.Page.GetType(), "mykey", "CloseAndRebind();", true);
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
        /// The server validation file name is exist.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        protected void ServerValidationFileNameIsExist(object source, ServerValidateEventArgs args)
        {
            if(this.txtName.Text.Trim().Length == 0)
            {
                this.fileNameValidator.ErrorMessage = "Please enter Notification rule name.";
                this.divFileName.Style["margin-bottom"] = "-26px;";
                args.IsValid = false;
            }
        }

        /// <summary>
        /// Load all combo data
        /// </summary>
        private void LoadComboData()
        {
            var groupList = this.roleService.GetAll(UserSession.Current.RoleId == 1);
            this.ddlReceiveGroup.DataSource = groupList;
            this.ddlReceiveGroup.DataValueField = "Id";
            this.ddlReceiveGroup.DataTextField = "Name";
            this.ddlReceiveGroup.DataBind();

            var disciplineList = this.disciplineService.GetAll();
            this.ddlDiscipline.DataSource = disciplineList;
            this.ddlDiscipline.DataValueField = "ID";
            this.ddlDiscipline.DataTextField = "Name";
            this.ddlDiscipline.DataBind();
        }

        protected void ddlReceiveGroup_SelectecIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ddlReceiveGroup.Focus();
            var comboText = e.Text;
            var listUserTotal = new List<User>();
            var listGroupId = string.Empty;
            foreach (var text in comboText.Split(','))
            {
                if (this.ddlReceiveGroup.FindItemByText(text.Trim()).Value != null)
                {
                    var groupId = Convert.ToInt32(this.ddlReceiveGroup.FindItemByText(text.Trim()).Value);
                    var listUser = this.userService.GetAllByRoleId(groupId);
                    listUserTotal = listUserTotal.Union(listUser).ToList();
                    listGroupId += groupId + ",";
                }
            }
            
            if(!string.IsNullOrEmpty(listGroupId))
            {
                Session.Add("ListGroupId", listGroupId);    
            }


            listUserTotal.Insert(0, new User() { FullName = string.Empty });
            this.ddlReceiver.DataSource = listUserTotal;
            this.ddlReceiver.DataValueField = "Id";
            this.ddlReceiver.DataTextField = "Email";
            this.ddlReceiver.DataBind();
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RebindUserList")
            {
            }
        }

        protected void btnGetUser_Click(object sender, EventArgs e)
        {
            var listRoleId = new List<int>();
            var listUser = new List<User>();
            foreach (var groupItem in ddlReceiveGroup.CheckedItems)
            {
                listRoleId.Add(Convert.ToInt32(groupItem.Value));
            }

            if (listRoleId.Count > 0)
            {
                listUser = this.userService.GetSpecialListUser(listRoleId);
            }
            else
            {
                listUser = this.userService.GetAllByRoleId(0);
            }
            this.ddlReceiver.DataSource = listUser;
            this.ddlReceiver.DataValueField = "Id";
            this.ddlReceiver.DataTextField = "FullName";
            this.ddlReceiver.DataBind();

            if (!string.IsNullOrEmpty(this.Request.QueryString["notId"]))
            {
                var objNotifi = this.notificationRuleService.GetById(Convert.ToInt32(this.Request.QueryString["notId"]));
                if (objNotifi != null)
                {
                    if (!string.IsNullOrEmpty(objNotifi.ReceiverListId))
                    {
                        var listUserId = objNotifi.ReceiverListId.Split(';').Select(t => Convert.ToInt32(t)).ToList();
                        foreach (RadComboBoxItem item in ddlReceiver.Items)
                        {
                            if (listUserId.Contains(Convert.ToInt32(item.Value)))
                            {
                                item.Checked = true;
                            }
                        }
                    }
                }
            }
        }
    }
}