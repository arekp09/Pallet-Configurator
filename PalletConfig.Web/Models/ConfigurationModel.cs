using System;
using System.Collections.Generic;
using System.Text;

namespace PalletConfig.Web.Models
{
    public class ConfigurationModel
    {
        // Columns: 'Z' / Rows: 'X'
        //TODO: Create new model which will store pallet configuration and logic to calculate stacking
        public string OptionName { get; set; }
        public int LayersQuantity { get; set; }
        public int RowsPerLayer { get; set; }
        public int ColumnsPerLayer { get; set; }
        public double Volume { get; set; }
        public int NumberOfBoxes { get; set; }
        public double TotalWeight { get; set; }
        public double TotalHeight { get; set; }

        public List<ConfigurationModel> CalculatePalletConfiguration(PalletModel model)
        {
            var _configurationsList = new List<ConfigurationModel>();
            _configurationsList.Add(CalculateOptionA(model));

            return _configurationsList;
        }

        public ConfigurationModel CalculateOptionA(PalletModel model)
        {
            var _option = new ConfigurationModel();
            _option.OptionName = "Option A";
            _option.RowsPerLayer = model.PalletSizeX / model.BoxSizeX;
            _option.ColumnsPerLayer = model.PalletSizeZ / model.BoxSizeZ;

            int maxBoxesQuantity = Convert.ToInt32(model.PalletWeight / model.BoxWeight);
            double weightPerLayer = _option.RowsPerLayer * _option.ColumnsPerLayer * model.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(model.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (model.PalletHeight - model.PalletSizeY) / model.BoxSizeY;
            _option.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            var maxVolume = model.PalletSizeX * model.PalletSizeZ * (model.PalletHeight - model.PalletSizeY);
            var boxVolume = model.BoxSizeX * model.BoxSizeY * model.BoxSizeZ;
            _option.NumberOfBoxes = _option.RowsPerLayer * _option.ColumnsPerLayer * _option.LayersQuantity;
            var actualVolume = _option.NumberOfBoxes * boxVolume;
            _option.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);

            _option.TotalHeight = model.PalletSizeY + (model.BoxSizeY * _option.LayersQuantity);
            _option.TotalWeight = _option.NumberOfBoxes * model.BoxWeight;

            return _option;
        }
    }
}
