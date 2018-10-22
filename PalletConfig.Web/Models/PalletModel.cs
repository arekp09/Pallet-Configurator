using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PalletConfig.Web.Models
{
    public class PalletModel
    {
        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(50,1000, ErrorMessage = "Pallet size need to be between {1} and {2}")]
        public int PalletSizeX { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(10, 100, ErrorMessage = "Pallet size need to be between {1} and {2}")]
        public int PalletSizeY { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(50, 1000, ErrorMessage = "Pallet size need to be between {1} and {2}")]
        public int PalletSizeZ { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(1, 1000, ErrorMessage = "Box size need to be between {1} and {2}")]
        public int BoxSizeX { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(1, 1000, ErrorMessage = "Box size need to be between {1} and {2}")]
        public int BoxSizeY { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(1, 1000, ErrorMessage = "Box size need to be between {1} and {2}")]
        public int BoxSizeZ { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(0.1, 50, ErrorMessage = "Box weight need to be between {1} and {2}")]
        public double BoxWeight { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(20, 1000, ErrorMessage = "Pallet height need to be between {1} and {2}")]
        public int PalletHeight { get; set; }

        [Required(ErrorMessage = "Field need to be filled in")]
        [Range(10, 5000, ErrorMessage = "Pallet weight need to be between {1} and {2}")]
        public double PalletWeight { get; set; }

        public bool StackOpposite { get; set; }
    }
}
