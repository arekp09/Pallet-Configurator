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
        public CoordinatesModel PalletSize { get; set; }
        public CoordinatesModel BoxSize { get; set; }

        public List<ConfigurationModel> CalculatePalletConfiguration(PalletModel model)
        {
            var _configurationsList = new List<ConfigurationModel>();
            _configurationsList.Add(CalculateOptionA(model));

            return _configurationsList;
        }

        /// <summary>
        /// Calculate Option A - setup as per user input (boxes not turned)
        /// </summary>
        /// <param name="model">Model received from user input</param>
        /// <returns></returns>
        public ConfigurationModel CalculateOptionA(PalletModel model)
        {
            var _option = new ConfigurationModel();

            _option.PalletSize = new CoordinatesModel
            {
                X = model.PalletSizeX,
                Y = model.PalletSizeY,
                Z = model.PalletSizeZ
            };

            _option.BoxSize = new CoordinatesModel
            {
                X = model.BoxSizeX,
                Y = model.BoxSizeY,
                Z = model.BoxSizeZ
            };

            _option.OptionName = "Option A";
            _option.RowsPerLayer = _option.PalletSize.X / model.BoxSizeX;
            _option.ColumnsPerLayer = _option.PalletSize.Z / model.BoxSizeZ;

            int maxBoxesQuantity = Convert.ToInt32(model.PalletWeight / model.BoxWeight);
            double weightPerLayer = _option.RowsPerLayer * _option.ColumnsPerLayer * model.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(model.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (model.PalletHeight - _option.PalletSize.Y) / model.BoxSizeY;
            _option.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            var maxVolume = _option.PalletSize.X * _option.PalletSize.Z * (model.PalletHeight - _option.PalletSize.Y);
            var boxVolume = model.BoxSizeX * model.BoxSizeY * model.BoxSizeZ;
            _option.NumberOfBoxes = _option.RowsPerLayer * _option.ColumnsPerLayer * _option.LayersQuantity;
            var actualVolume = _option.NumberOfBoxes * boxVolume;
            _option.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);

            _option.TotalHeight = _option.PalletSize.Y + (model.BoxSizeY * _option.LayersQuantity);
            _option.TotalWeight = _option.NumberOfBoxes * model.BoxWeight;

            return _option;
        }

        /// <summary>
        /// Calculate Option B - setup with all boxes moved around 90 deg
        /// </summary>
        /// <param name="model">Model received from user input</param>
        /// <returns></returns>
        
    }
}
