using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitViewSample.DesignViewModels
{
    public class MainPageDesignViewModel
    {
        public MainPageDesignViewModel()
        {
            DisplayText = "This is the main page!";
        }

        public string DisplayText { get; private set; }
    }
}
