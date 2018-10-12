using System;
using System.Collections.Generic;
using System.Text;

namespace PalletConfig.Library
{
    public class Configuration
    {
        //TODO: Create new model which will store pallet configuration and logic to calculate stacking
        public string OptionId { get; set; }
        public int LayersQuantity { get; set; }
        public int RowsPerLayer { get; set; }
        public int ColumnsPerLayer { get; set; }



    }
}
