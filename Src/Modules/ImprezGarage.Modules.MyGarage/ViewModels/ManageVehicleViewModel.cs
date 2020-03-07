using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Regions;

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    public class ManageVehicleViewModel : BindableBase, INavigationAware
    {
        public ManageVehicleViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
