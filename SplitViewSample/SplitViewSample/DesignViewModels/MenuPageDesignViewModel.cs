using SplitViewSample.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitViewSample.DesignViewModels
{
    public class MenuPageDesignViewModel
    {
        public MenuPageDesignViewModel()
        {
            Commands = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { DisplayName = "Main page", FontIcon = "\ue15f", Command = null },
                new MenuItemViewModel { DisplayName = "Second page", FontIcon = "\ue19f", Command = null }
            };
        }

        public ObservableCollection<MenuItemViewModel> Commands { get; private set; }
    }
}
