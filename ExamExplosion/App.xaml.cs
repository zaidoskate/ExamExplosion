using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExamExplosion
{
    public partial class App : Application
    {
        App()
        {
            log4net.Config.XmlConfigurator.Configure();
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-MX");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ILog log = LogManager.GetLogger(typeof(App));
            log.Info("Exam Explosion - Cliente ha iniciado.");
        }
    }
}
