using System;
using System.Collections.Generic;
using System.Text;

namespace PalletConfig.Library
{
    public class Configuration
    {
        // Columns: 'Z' / Rows: 'X'
        //TODO: Create new model which will store pallet configuration and logic to calculate stacking
        public string OptionId { get; set; }
        public int LayersQuantity { get; set; }
        public int RowsPerLayer { get; set; }
        public int ColumnsPerLayer { get; set; }

        public List<Configuration> CalculatePalletConfiguration(Pallet model)
        {
            var _configurationsList = new List<Configuration>();


            return _configurationsList;
        }

        public Configuration CalculateOptionA(Pallet model)
        {
            var _optionA = new Configuration();
            _optionA.OptionId = "OptionA";
            _optionA.RowsPerLayer = model.PalletSizeX / model.BoxSizeX;
            _optionA.ColumnsPerLayer = model.PalletSizeZ / model.BoxSizeZ;

            int maxBoxesQuantity = Convert.ToInt32(model.PalletWeight / model.BoxWeight);
            float weightPerLayer = _optionA.RowsPerLayer * _optionA.ColumnsPerLayer * model.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(model.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (model.PalletHeight - model.PalletSizeY) / model.BoxSizeY;
            _optionA.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            return _optionA;
        }

    }
}
