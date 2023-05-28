using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNStreamControl.Wpf.Models
{
    public class StationSelector
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public StationSelector(string name, string imgPath)
        {
            Name = name;
            ImagePath = $"/RPNStreamControl.Wpf;component/Assets/Images/Logos/{imgPath}-logo.png";
		}
    }
}
