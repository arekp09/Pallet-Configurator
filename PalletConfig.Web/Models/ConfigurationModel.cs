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

        private List<StackingOptionModel> GenerateListOfStackingOptions()
        {
            var _list = new List<StackingOptionModel>();

            _list.Add(new StackingOptionModel {Name = "Option A", Rotation = 0, Mode = "Round"});
            _list.Add(new StackingOptionModel {Name = "Option B", Rotation = 1, Mode = "Round"});
            _list.Add(new StackingOptionModel {Name = "Option C", Rotation = 0.01, Mode = "RoundUp"});
            _list.Add(new StackingOptionModel {Name = "Option D", Rotation = 0.99, Mode = "RoundDown"});
            _list.Add(new StackingOptionModel {Name = "Option E", Rotation = 0.5, Mode = "Round"});
            _list.Add(new StackingOptionModel {Name = "Option F", Rotation = 0.5, Mode = "Round"});
            
            return _list;
        }

        public List<ConfigurationModel> CalculatePalletConfiguration(PalletModel palletModel)
        {
            var _configurationsList = new List<ConfigurationModel>();
            _configurationsList.Add(CalculateOptionA(palletModel));

            return _configurationsList;
        }

        // TODO: Re-factor to generic calculation method (input: model, calculationOption; output: ConfigurationModel)
        /// <summary>
        /// Calculate Option A - setup as per user input (boxes not turned)
        /// </summary>
        /// <param name="_palletModel">Model received from user input</param>
        /// <returns></returns>
        public ConfigurationModel CalculateOptionA(PalletModel _palletModel)
        {
            var _configModel = new ConfigurationModel();

            AssignParametersToObject(_palletModel);

            _configModel.OptionName = "Option A";
            _configModel.RowsPerLayer = _configModel.PalletSize.X / _palletModel.BoxSizeX;
            _configModel.ColumnsPerLayer = _configModel.PalletSize.Z / _palletModel.BoxSizeZ;

            int maxBoxesQuantity = Convert.ToInt32(_palletModel.PalletWeight / _palletModel.BoxWeight);
            double weightPerLayer = _configModel.RowsPerLayer * _configModel.ColumnsPerLayer * _palletModel.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(_palletModel.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (_palletModel.PalletHeight - _configModel.PalletSize.Y) / _palletModel.BoxSizeY;
            _configModel.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            var maxVolume = _configModel.PalletSize.X * _configModel.PalletSize.Z * (_palletModel.PalletHeight - _configModel.PalletSize.Y);
            var boxVolume = _palletModel.BoxSizeX * _palletModel.BoxSizeY * _palletModel.BoxSizeZ;
            _configModel.NumberOfBoxes = _configModel.RowsPerLayer * _configModel.ColumnsPerLayer * _configModel.LayersQuantity;
            var actualVolume = _configModel.NumberOfBoxes * boxVolume;
            _configModel.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);

            _configModel.TotalHeight = _configModel.PalletSize.Y + (_palletModel.BoxSizeY * _configModel.LayersQuantity);
            _configModel.TotalWeight = _configModel.NumberOfBoxes * _palletModel.BoxWeight;

            return _configModel;
        }

        // TODO: Change to be more generic
        private static ConfigurationModel AssignParametersToObject( PalletModel palletModel)
        {
            var output = new ConfigurationModel
            {
                BoxSize = new CoordinatesModel
                {
                    X = palletModel.BoxSizeX,
                    Y = palletModel.BoxSizeY,
                    Z = palletModel.BoxSizeZ
                },

                PalletSize = new CoordinatesModel
                {
                    X = palletModel.PalletSizeX,
                    Y = palletModel.PalletSizeY,
                    Z = palletModel.PalletSizeZ
                }
            };
            return output;
        }
    }
}
