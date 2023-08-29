using System.ServiceProcess;
using System.Configuration;

namespace EAM.LDAFolderWatcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Temp.IO;

    public partial class EAMLDAFolderWatcher : ServiceBase
    {
        /// <summary>
        /// The monitor path.
        /// </summary>
        private readonly string monitorPath = ConfigurationSettings.AppSettings.Get("Directory");


        /// <summary>
        /// The folder watcher.
        /// </summary>
        private MyFileSystemWatcher folderWatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAM.LDAFolderWatcher"/> class.
        /// </summary>
        public EAMLDAFolderWatcher()
        {
            InitializeComponent();

        }

        /// <summary>
        /// The on start.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected override void OnStart(string[] args)
        {
            this.folderWatcher = new MyFileSystemWatcher(this.monitorPath, "*.*")
                {
                    Interval = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("WatcherInterval")),
                    InternalBufferSize = Convert.ToInt32(ConfigurationSettings.AppSettings.Get("WatcherBufferSize"))
                };

            this.folderWatcher.Created += new System.IO.FileSystemEventHandler(fsw_Created);
            //this.folderWatcher.Changed += new System.IO.FileSystemEventHandler(fsw_Changed);
            this.folderWatcher.EnableRaisingEvents = true;
            EventLog.WriteEntry("EAM-LDA folder watcher service is started. Watching on '" + this.monitorPath + "'");
        }

        protected override void OnStop()
        {
            this.folderWatcher.Dispose();
            
            EventLog.WriteEntry("EAM-LDA folder watcher service is stoped.");
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
        private void fsw_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                File.Copy(e.FullPath, ConfigurationSettings.AppSettings.Get("CopyPath") + e.Name, true);
            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(exception.Message);
            }
        }
    }
}
