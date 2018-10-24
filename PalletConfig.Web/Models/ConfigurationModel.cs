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
        public LayerModel Standard { get; set; }
        public LayerModel Rotated { get; set; }
        public double Volume { get; set; }
        public int NumberOfBoxes { get; set; }
        public double TotalWeight { get; set; }
        public double TotalHeight { get; set; }
        public CoordinatesModel PalletSize { get; set; }
        public CoordinatesModel BoxSize { get; set; }

        public List<ConfigurationModel> CalculatePalletConfiguration(PalletModel _palletModel)
        {
            var output = new List<ConfigurationModel>();

            foreach (var option in StackingOptionModel.GenerateListOfStackingOptions())
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

            // Calculate max possible rows and collumns
            var maxRows = output.PalletSize.X / _palletModel.BoxSizeX;
            var maxColumns = output.PalletSize.Z / _palletModel.BoxSizeZ;

            // Calculate Standard Layer
            //TODO: Need to distinguish between round up and round down!
            var standardPalletZ = Convert.ToInt32(Convert.ToDouble(output.PalletSize.Z) * (Convert.ToDouble(maxColumns) * _stackingOption.Rotation));
            output.Standard.RowsPerLayer = output.PalletSize.X / _palletModel.BoxSizeX;
            output.Standard.ColumnsPerLayer = standardPalletZ / _palletModel.BoxSizeZ;
            

            // Calculate Rotated Layer
            var rotatedPalletZ = output.PalletSize.Z - standardPalletZ;
            output.Rotated.RowsPerLayer = output.PalletSize.X / _palletModel.BoxSizeZ; //take Z from Box as it's rotated
            output.Rotated.ColumnsPerLayer = rotatedPalletZ / _palletModel.BoxSizeX; //take X from Box as it's rotated

            // TODO: Update below to work with new parameters

            int boxesPerLayer = ( output.Standard.RowsPerLayer * output.Standard.ColumnsPerLayer ) 
                                + ( output.Rotated.RowsPerLayer * output.Rotated.ColumnsPerLayer);

            // Calculate Layers Quantity
            double weightPerLayer = boxesPerLayer * _palletModel.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(_palletModel.PalletWeight / weightPerLayer);
            int maxHeightLayersQuantity = (_palletModel.PalletHeight - output.PalletSize.Y) / _palletModel.BoxSizeY;
            output.LayersQuantity = Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);

            // Calculate total number of boxes
            var maxVolume = output.PalletSize.X * output.PalletSize.Z * (_palletModel.PalletHeight - output.PalletSize.Y);
            var boxVolume = _palletModel.BoxSizeX * _palletModel.BoxSizeY * _palletModel.BoxSizeZ;
            output.NumberOfBoxes = boxesPerLayer * output.LayersQuantity;

            // Calculate Volume
            var actualVolume = output.NumberOfBoxes * boxVolume;
            output.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);

            // Calculate total Weight and Height
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
