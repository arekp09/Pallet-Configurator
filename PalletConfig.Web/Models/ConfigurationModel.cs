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

        public List<ConfigurationModel> CalculatePalletConfiguration(PalletModel _palletModel)
        {
            var output = new List<ConfigurationModel>();

            foreach (var option in GenerateListOfStackingOptions())
            {
                output.Add(CalculateOption(_palletModel, option));
            }
            
            return output;
        }

        // TODO: Re-factor to generic calculation method (input: model, calculationOption; output: ConfigurationModel)
        // Need to split in 2 areas(Standard and Rotated), calculate load

        /// <summary>
        /// Calculate Option A - setup as per user input (boxes not turned)
        /// </summary>
        /// <param name="_palletModel">Model received from user input</param>
        /// <returns></returns>
        public ConfigurationModel CalculateOption(PalletModel _palletModel, StackingOptionModel _stackingOption)
        {
            var output = new ConfigurationModel();

            // GetBoxSize
            output.BoxSize = GetBoxSize(_palletModel);
            // GetPalletSize
            output.PalletSize = GetPalletSize(_palletModel);
            // Option Name
            output.OptionName = _stackingOption.Name;

            //Calculate rows per layer
            output.RowsPerLayer = output.PalletSize.X / _palletModel.BoxSizeX;
            output.ColumnsPerLayer = output.PalletSize.Z / _palletModel.BoxSizeZ;

            int maxBoxesQuantity = Convert.ToInt32(_palletModel.PalletWeight / _palletModel.BoxWeight);
            double weightPerLayer = output.RowsPerLayer * output.ColumnsPerLayer * _palletModel.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(_palletModel.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (_palletModel.PalletHeight - output.PalletSize.Y) / _palletModel.BoxSizeY;
            output.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            var maxVolume = output.PalletSize.X * output.PalletSize.Z * (_palletModel.PalletHeight - output.PalletSize.Y);
            var boxVolume = _palletModel.BoxSizeX * _palletModel.BoxSizeY * _palletModel.BoxSizeZ;
            output.NumberOfBoxes = output.RowsPerLayer * output.ColumnsPerLayer * output.LayersQuantity;
            var actualVolume = output.NumberOfBoxes * boxVolume;
            output.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);

            output.TotalHeight = output.PalletSize.Y + (_palletModel.BoxSizeY * output.LayersQuantity);
            output.TotalWeight = output.NumberOfBoxes * _palletModel.BoxWeight;

            return output;
        }
        
        //TODO: Add logic to swap X and Y when needed

        /// <summary>
        /// Takes Box size from form (model)
        /// </summary>
        /// <param name="palletModel">PalletModel from form</param>
        /// <returns>Box Size as CoordinatesModel</returns>
        private CoordinatesModel GetBoxSize(PalletModel palletModel)
        {
            var output = new CoordinatesModel
            {
                X = palletModel.BoxSizeX,
                Y = palletModel.BoxSizeY,
                Z = palletModel.BoxSizeZ
            };
            return output;
        }

        /// <summary>
        /// Takes Pallet size from form (model)
        /// </summary>
        /// <param name="_palletModel">PalletModel from form</param>
        /// <returns>Pallet Size as CoordinatesModel</returns>
        private CoordinatesModel GetPalletSize(PalletModel _palletModel)
        {
            var output = new CoordinatesModel
            {
                X = _palletModel.PalletSizeX,
                Y = _palletModel.PalletSizeY,
                Z = _palletModel.PalletSizeZ
            };
            return output;
        }
    }
}
