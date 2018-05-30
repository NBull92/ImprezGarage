//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.MyGarage.ViewModels
{
    using ImprezGarage.Infrastructure;
    using ImprezGarage.Infrastructure.Dialogs;
    using ImprezGarage.Infrastructure.Model;
    using ImprezGarage.Modules.MyGarage.Views;
    using Prism.Commands;
    using Prism.Events;

    public class VehicleViewModel : Infrastructure.ViewModels.VehicleViewModel
    {
        private const string NOTIFICATION_HEADER = "Alert!";
        private const string VEHICLE_DELETED = "Vehicle deleted sucessfully!";
        private const string CONFIRM_VEHICLE_DELETE = "Are you sure you wish to delete this vehicle?";
        private readonly IEventAggregator _eventAggregator;

        public override DelegateCommand EditVehicleCommand { get; set; }
        public override DelegateCommand DeleteVehicleCommand { get; set; }

        public VehicleViewModel(IDialogService dialogService, IDataService dataService, IEventAggregator eventAggregator) : base(dialogService, dataService)
        {
            _eventAggregator = eventAggregator;
            EditVehicleCommand = new DelegateCommand(EditVehicleExecute);
            DeleteVehicleCommand = new DelegateCommand(DeleteVehicleExecute);
        }

        #region Command Handlers
        /// <summary>
        /// Deletes this vehicle in the database.
        /// </summary>
        private void DeleteVehicleExecute()
        {
            if (!DialogService.Confirm(CONFIRM_VEHICLE_DELETE))
                return;

            DataService.DeleteVehicle((error) => 
            {
                if(error != null)
                {
                    return;
                }

                DialogService.Alert(VEHICLE_DELETED, NOTIFICATION_HEADER);

                _eventAggregator.GetEvent<Events.RefreshDataEvent>().Publish();
                _eventAggregator.GetEvent<Events.SelectVehicleEvent>().Publish(null);
            }, Vehicle);
        }

        /// <summary>
        /// Take the current selected vechiel and open a window to edit it.
        /// </summary>
        public void EditVehicleExecute()
        {
            var view = new AddVehicle();
            var viewModel = view.DataContext as AddVehicleViewModel;
            viewModel.IsEdit = true;
            viewModel.Edit(Vehicle);

            DialogService.ShowWindow((model, error) =>
            {
                if (error != null)
                {
                    return;
                }

                Refresh();
            }, view, 382, 407);
        }
        #endregion        
    }
}   //ImprezGarage.Modules.MyGarage.ViewModels namespace 