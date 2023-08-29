// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ServiceProcess;

namespace EAM.LDAFolderWatcher
{
    /// <summary>
    /// The program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            
            ServicesToRun = new ServiceBase[] 
            { 
                new EAM.LDAFolderWatcher.EAMLDAFolderWatcher() 
            };

            ServiceBase.Run(ServicesToRun);
        }
    }
}
