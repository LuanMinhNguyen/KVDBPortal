using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace WMSTestAmosConnection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTestConn_Click(object sender, EventArgs e)
        {
            try
            {
                var connStr = this.txtConn.Text.Trim();

                var conn = new OdbcConnection(connStr);
                conn.Open();
                this.lblMessage.Text = "Connect to AMOS data successfull.";

                conn.Close();
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
            
        }
    }
}
