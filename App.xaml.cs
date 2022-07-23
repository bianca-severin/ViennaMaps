using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ViennaMaps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // API Key for development
            Esri.ArcGISRuntime.ArcGISRuntimeEnvironment.ApiKey = "AAPK45946aac033744e7829cc44189d86fedZF6POt8sn3u9iO_fWel9o9FEUXH2upCLXgnOOGASVnVYHEy4DyuJjH3m4aljQmv_";
        }
    }
}
