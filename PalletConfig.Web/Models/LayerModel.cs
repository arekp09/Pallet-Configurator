using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PalletConfig.Web.Models
{
    public class LayerModel
    {
        /// <summary>
        /// Number of rows (coordinate X) in single layer
        /// </summary>
        public int RowsPerLayer { get; set; }

        /// <summary>
        /// Number of columns (coordinate Z) in single layer
        /// </summary>
        public int ColumnsPerLayer { get; set; }
    }
}
