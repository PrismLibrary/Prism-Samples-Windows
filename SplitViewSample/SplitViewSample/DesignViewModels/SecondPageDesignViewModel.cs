using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitViewSample.DesignViewModels
{
    public class SecondPageDesignViewModel
    {
        public SecondPageDesignViewModel()
        {
            DisplayText = "This is the main page!";
        }

        public string DisplayText { get; private set; }
    }
}
