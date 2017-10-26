using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTextByVoice
{
   public  class MainModule : IModule
    {
        IRegionManager manager;
        public MainModule(IRegionManager manager)
        {
            this.manager = manager;
        }

        public void Initialize()
        {
            manager.RegisterViewWithRegion("MainRegion", typeof(MainView));
        }
    }
}
