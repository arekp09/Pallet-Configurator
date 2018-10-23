using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PalletConfig.Web.Models
{
    public class StackingOptionModel
    {
        /// <summary>
        /// Name of Stacking Option
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Procentage of boxes in single layer that will be rotated
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Rounding Mode: "Round", "RoundUp" or "RoundDown"
        /// </summary>
        public string Mode { get; set; }
    }
}
