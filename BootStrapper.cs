using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReadTextByVoice
{
    public class BootStrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<Shell>();
        }
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
            ModuleCatalog moduleLog = this.ModuleCatalog as ModuleCatalog;
            moduleLog.AddModule(typeof(MainModule));
        }
        protected override void InitializeShell()
        {
            Application.Current.MainWindow = this.Shell as Window;
            Application.Current.MainWindow.Show();
        }
    }
}
