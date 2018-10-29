﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PalletConfig.Web.Models;


namespace PalletConfig.Tests
{
    public class ConfigurationTests
    {
        readonly PalletModel testModel = new PalletModel {BoxSizeX = 20, BoxSizeY = 10, BoxSizeZ = 30,
            BoxWeight = 2, PalletSizeX = 150, PalletSizeY = 25, PalletSizeZ = 250,
            PalletHeight = 200, PalletWeight = 500, StackOpposite = false };

        readonly StackingOptionModel stackingOptionModel_A = new StackingOptionModel { Name = "Option A", Rotation = 1, Mode = "Round" };
        readonly StackingOptionModel stackingOptionModel_B = new StackingOptionModel { Name = "Option B", Rotation = 0, Mode = "Round" };
        readonly StackingOptionModel stackingOptionModel_C = new StackingOptionModel { Name = "Option C", Rotation = 0.99, Mode = "RoundDown" };
        readonly StackingOptionModel stackingOptionModel_D = new StackingOptionModel { Name = "Option D", Rotation = 0.01, Mode = "RoundUp" };
        readonly StackingOptionModel stackingOptionModel_E = new StackingOptionModel { Name = "Option E", Rotation = 0.5, Mode = "Round" };

        [Fact]
        public void CalculateOptionA_NoRotation_ReturnConfigurationObject()
        {
            var config = new ConfigurationModel();

            var actual = config.CalculateOption(testModel, stackingOptionModel_A);

            Assert.Equal(8, actual.Standard.ColumnsPerLayer);
            Assert.Equal(7, actual.Standard.RowsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }

        [Fact]
        public void CalculateOptionB_Rotation100_ReturnConfigurationObject()
        {
            var config = new ConfigurationModel();

            var actual = config.CalculateOption(testModel, stackingOptionModel_B);

            Assert.Equal(0, actual.Standard.ColumnsPerLayer);
            Assert.Equal(12, actual.Rotated.ColumnsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }

        [Fact]
        public void CalculateOptionC_Rotation1_ReturnConfigurationObject()
        {
            var config = new ConfigurationModel();

            var actual = config.CalculateOption(testModel, stackingOptionModel_C);

            Assert.Equal(7, actual.Standard.ColumnsPerLayer);
            Assert.Equal(2, actual.Rotated.ColumnsPerLayer);
            Assert.Equal(7, actual.Standard.RowsPerLayer);
            Assert.Equal(5, actual.Rotated.RowsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }

        [Fact]
        public void CalculateOptionD_Rotation99_ReturnConfigurationObject()
        {
            var config = new ConfigurationModel();

            var actual = config.CalculateOption(testModel, stackingOptionModel_D);

            Assert.Equal(1, actual.Standard.ColumnsPerLayer);
            Assert.Equal(11, actual.Rotated.ColumnsPerLayer);
            Assert.Equal(7, actual.Standard.RowsPerLayer);
            Assert.Equal(5, actual.Rotated.RowsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }

        [Fact]
        public void CalculateOptionE_Rotation50_ReturnConfigurationObject()
        {
            var config = new ConfigurationModel();

            var actual = config.CalculateOption(testModel, stackingOptionModel_E);

            Assert.Equal(4, actual.Standard.ColumnsPerLayer);
            Assert.Equal(6, actual.Rotated.ColumnsPerLayer);
            Assert.Equal(7, actual.Standard.RowsPerLayer);
            Assert.Equal(5, actual.Rotated.RowsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }


    }

}
