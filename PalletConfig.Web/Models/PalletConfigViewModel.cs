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
            Start();
        }
        
        public List<ConfigurationModel> Configurations { get; set; }
        public PalletModel PalletData { get; set; }
        public string EventCommand { get; set; }
        public string JsonConfiguration { get; set; }
        ConfigurationModel configurationModel;

        /// <summary>
        /// Responsible for managing events in application, switch between different actions.
        /// </summary>
        public void EventHandler()
        {
            switch (EventCommand.ToLower())
            {
                case "start":
                    Start();
                    break;

                case "confirm":
                    GenerateConfigurations();
                    break;

                case "reset":
                    PalletData = new PalletModel();
                    configurationModel = new ConfigurationModel();
                    break;

                default:
                    break;
            }
        }

        public void Start()
        {
            Configurations = new List<ConfigurationModel>();
            PalletData = new PalletModel();
            configurationModel = new ConfigurationModel();
            EventCommand = "start";
        }

        public void GenerateConfigurations()
        {
            Configurations = configurationModel.CalculatePalletConfiguration(PalletData);
        }
    }
}
