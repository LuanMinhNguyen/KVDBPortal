
namespace NitificationTaskOverdue
{
    using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Net;
using System.Net.Mail;
using System.Timers;
    public partial class NotificationTaskOverdue : ServiceBase
    {
        public NotificationTaskOverdue()
        {
            InitializeComponent();
          //  Function action = new Function();
          //  action.GetAllTaskOverdue();
        }
        

        protected override void OnStart(string[] args)
        {
            Timer time = new Timer();
            time.Interval = 60 * 60 * 1000;
            time.Elapsed += new ElapsedEventHandler(time_Elapsed);
            time.Enabled = true;

            TextWriter file = new StreamWriter(ConfigurationManager.AppSettings["File"], true);
            file.WriteLine("Start service. Time: " + DateTime.Now);
            file.Close();
        }
        void time_Elapsed(object sender, ElapsedEventArgs e)
        {
            var hour = DateTime.Now.Hour;
            var timeSet = (ConfigurationManager.AppSettings["TimeSet"]).Split(';').Where(t => !string.IsNullOrEmpty(t)).Select(t => Convert.ToInt32(t)).ToList();
            if((timeSet[0]==hour) && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday && Convert.ToBoolean(ConfigurationManager.AppSettings["SendEmail"]))
            {
                Function action = new Function();
                action.GetAllTaskOverdue();
            }
           

        }
        protected override void OnStop()
        {
            TextWriter file = new StreamWriter(ConfigurationManager.AppSettings["File"], true);
            file.WriteLine("Stop service. Time: " + DateTime.Now);
            file.Close();
        }

       
    }
}
