using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PalletConfig.Library;


namespace PalletConfig.Tests
{
    public class ConfigurationTests
    {
        readonly Pallet testModel = new Pallet { BoxSizeX = 20, BoxSizeY = 10, BoxSizeZ = 30,
            BoxWeight = 2, PalletSizeX = 150, PalletSizeY = 25, PalletSizeZ = 250,
            PalletHeight = 200, PalletWeight = 500 };
        
        [Fact]
        public void CalculateOptionA_InputModel_ReturnConfigurationObject()
        {
            var config = new Configuration();

            var actual = config.CalculateOptionA(testModel);

            Assert.Equal(8, actual.ColumnsPerLayer);
            Assert.Equal(7, actual.RowsPerLayer);
            Assert.Equal(4, actual.LayersQuantity);
        }
    }
}
