using PalletConfig.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PalletConfig.Web.Models
{
    public class PalletConfigViewModel
    {
        public PalletConfigViewModel()
        {
            Configurations = new List<Configuration>();
            PalletData = new Pallet();
        }

        public List<Configuration> Configurations { get; set; }
        public Pallet PalletData { get; set; }

        public void GenerateConfigurations(Pallet model)
        {
            var config = new Configuration();
            Configurations = config.CalculatePalletConfiguration(model);
        }
    }
}
