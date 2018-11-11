using System;
using System.Collections.Generic;
using System.Text;

namespace PalletConfig.Web.Models
{
    public class ConfigurationModel
    {
        // Columns: 'Z' / Rows: 'X'
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
            var standardPalletZ = CalculateStandardPalletZ(_stackingOption.Rotation, maxColumns, _palletModel.BoxSizeZ, _stackingOption.Mode);
            output.Standard = CalculateLayer(output.PalletSize.X, _palletModel.BoxSizeX, standardPalletZ, _palletModel.BoxSizeZ);
            // Calculate Rotated Layer
            var rotatedPalletZ = output.PalletSize.Z - standardPalletZ;
            output.Rotated = CalculateLayer(output.PalletSize.X, _palletModel.BoxSizeZ, rotatedPalletZ, _palletModel.BoxSizeX);
            // Calculate boxes per layer
            int boxesPerLayer = (output.Standard.RowsPerLayer * output.Standard.ColumnsPerLayer)
                                + (output.Rotated.RowsPerLayer * output.Rotated.ColumnsPerLayer);
            // Calculate Layers Quantity
            output.LayersQuantity = CalculateLayersQuantity(_palletModel, output.PalletSize.Y, boxesPerLayer);
            // Calculate total number of boxes
            int maxVolume, boxVolume;
            output.NumberOfBoxes = CalculateNumberOfBoxes(_palletModel, output, boxesPerLayer, out maxVolume, out boxVolume);
            int actualVolume = output.NumberOfBoxes * boxVolume;
            // Calculate Volume
            output.Volume = Convert.ToDouble(actualVolume) / Convert.ToDouble(maxVolume);
            // Calculate total Weight and Height
            output.TotalHeight = output.PalletSize.Y + (_palletModel.BoxSizeY * output.LayersQuantity);
            output.TotalWeight = output.NumberOfBoxes * _palletModel.BoxWeight;

            return output;
        }

        private int CalculateNumberOfBoxes(PalletModel _palletModel, ConfigurationModel output, int boxesPerLayer, out int maxVolume, out int boxVolume)
        {
            maxVolume = output.PalletSize.X * output.PalletSize.Z * (_palletModel.PalletHeight - output.PalletSize.Y);
            boxVolume = _palletModel.BoxSizeX * _palletModel.BoxSizeY * _palletModel.BoxSizeZ;

            return boxesPerLayer * output.LayersQuantity;
        }

        /// <summary>
        /// Calculates quantity of layers on pallet
        /// </summary>
        /// <param name="palletModel">PalletModel from form</param>
        /// <param name="palletSizeY">Height of pallet</param>
        /// <param name="boxesPerLayer">Quantity of all boxes per pallet</param>
        /// <returns>Integer value of layers on pallet</returns>
        private int CalculateLayersQuantity(PalletModel palletModel, int palletSizeY, int boxesPerLayer)
        {
            double weightPerLayer = boxesPerLayer * palletModel.BoxWeight;
            int maxWeightLayersQuantity = Convert.ToInt32(Math.Floor(palletModel.PalletWeight / weightPerLayer));
            int maxHeightLayersQuantity = Convert.ToInt32(Math.Floor(Convert.ToDouble(palletModel.PalletHeight - palletSizeY) / palletModel.BoxSizeY));
            return Math.Min(maxHeightLayersQuantity, maxWeightLayersQuantity);
        }

        /// <summary>
        /// Calculate layer setup (rows and columns) form given parameters
        /// </summary>
        /// <param name="palletSizeX">Width of pallet</param>
        /// <param name="boxSizeX">Width of box</param>
        /// <param name="palletSizeZ">Depth of pallet</param>
        /// <param name="boxSizeZ">Depth of box</param>
        /// <param name="roundingOption">Options of rounding a result to choose: Round, RoundUp or RoundDown</param>
        /// <returns></returns>
        private LayerModel CalculateLayer(double palletSizeX, double boxSizeX, double palletSizeZ, double boxSizeZ)
        {
            var output = new LayerModel
            {
                RowsPerLayer = Convert.ToInt32(Math.Floor(palletSizeX / boxSizeX)),
                ColumnsPerLayer = Convert.ToInt32(Math.Floor(palletSizeZ / boxSizeZ))
            };

            return output;
        }

        /// <summary>
        /// Calculates depth (in cm) available for standard stacking (before rotation)
        /// </summary>
        /// <param name="stackingRotation">% That will not be rotated</param>
        /// <param name="maxColumns">Maximum quantity of columns per layer (not rotated pallet)</param>
        /// <param name="boxSizeZ">Depth of box</param>
        /// <param name="stackingMode">Rounding option as string</param>
        /// <returns>Return depth for standard stacking (before rotation) in cm</returns>
        private int CalculateStandardPalletZ(double stackingRotation, int maxColumns, double boxSizeZ, string stackingMode)
        {
            int output;

            if(stackingMode == "RoundUp") output = Convert.ToInt32(Math.Ceiling(maxColumns * stackingRotation) * boxSizeZ);
            else if(stackingMode == "RoundDown") output = Convert.ToInt32(Math.Floor(maxColumns * stackingRotation) * boxSizeZ);
            else output = Convert.ToInt32((maxColumns * stackingRotation) * boxSizeZ);

            return output;
        }

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
