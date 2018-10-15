using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace PalletConfig.Library
{
    public class Pallet
    {
        public int PalletSizeX { get; set; }
        public int PalletSizeY { get; set; }
        public int PalletSizeZ { get; set; }
        public int BoxSizeX { get; set; }
        public int BoxSizeY { get; set; }
        public int BoxSizeZ { get; set; }
        public double BoxWeight { get; set; }
        public int PalletHeight { get; set; }
        public double PalletWeight { get; set; }
        public bool StackOpposite { get; set; }
    }
}
