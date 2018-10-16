using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PalletConfig.Library
{
    public class Pallet
    {
        [Required]
        [Range(50,1000)]
        public int PalletSizeX { get; set; }

        [Required]
        [Range(10, 100)]
        public int PalletSizeY { get; set; }

        [Required]
        [Range(50, 1000)]
        public int PalletSizeZ { get; set; }

        [Required]
        [Range(1, 1000)]
        public int BoxSizeX { get; set; }

        [Required]
        [Range(1, 1000)]
        public int BoxSizeY { get; set; }

        [Required]
        [Range(1, 1000)]
        public int BoxSizeZ { get; set; }

        [Required]
        [Range(0.1, 50)]
        public double BoxWeight { get; set; }

        [Required]
        [Range(20, 1000)]
        public int PalletHeight { get; set; }

        [Required]
        [Range(10, 5000)]
        public double PalletWeight { get; set; }

        public bool StackOpposite { get; set; }
    }
}
