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
        /// Procentage of boxes in single layer that will NOT be rotated
        /// </summary>
        public double Rotation { get; set; }

        /// <summary>
        /// Rounding Mode: "Round", "RoundUp" or "RoundDown"
        /// </summary>
        public string Mode { get; set; }

        public static List<StackingOptionModel> GenerateListOfStackingOptions()
        {
            var _list = new List<StackingOptionModel>();

            _list.Add(new StackingOptionModel { Name = "Option A", Rotation = 1, Mode = "Round" });
            _list.Add(new StackingOptionModel { Name = "Option B", Rotation = 0, Mode = "Round" });
            _list.Add(new StackingOptionModel { Name = "Option C", Rotation = 0.99, Mode = "RoundDown" });
            _list.Add(new StackingOptionModel { Name = "Option D", Rotation = 0.01, Mode = "RoundUp" });
            _list.Add(new StackingOptionModel { Name = "Option E", Rotation = 0.5, Mode = "Round" });

            return _list;
        }
    }
}
