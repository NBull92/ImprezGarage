//------------------------------------------------------------------------------
// Copyright of Nicholas Andrew Bull 2018
// This code is for portfolio use only.
//------------------------------------------------------------------------------

namespace ImprezGarage.Modules.Logger.ViewModels
{
    using DataModels;
    using Infrastructure;
    using Prism.Mvvm;
    using Prism.Regions;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    public class LogSettingsViewModel : BindableBase
    {
        #region Attributes
        /// <summary>
        /// Store the selected level of details for a log, that the user has selected.
        /// </summary>
        private LogDetail _selectedDetail;

        /// <summary>
        /// Store the selected log file type.
        /// </summary>
        private LogFileType _selectedFileType;

        /// <summary>
        /// Store the current instance of the data model.
        /// </summary>
        private readonly LogModel _loggerDataModel;
        #endregion

        #region Properties
        /// <summary>
        /// This is a collection of how many days the logs for ImprezGarage will be kept for.
        /// </summary>
        public ICollectionView LogLifeCollection { get; }

        /// <summary>
        /// Store the selected level of details for a log, that the user has selected.
        /// </summary>
        public LogDetail SelectedDetail
        {
            get => _selectedDetail;
            set => SetProperty(ref _selectedDetail, value, OnLogLevelChange);
        }

        /// <summary>
        /// Store the selected log file type.
        /// </summary>
        public LogFileType SelectedFileType
        {
            get => _selectedFileType;
            set => SetProperty(ref _selectedFileType, value, OnLogFileTypeChange);
        }
        #endregion

        #region Method
        public LogSettingsViewModel(IRegionManager regionManager)
        {
            LogLifeCollection = new ListCollectionView(new ObservableCollection<int> { 7, 14, 21, 28 });

            // Retrieve the data model of the logger stored in the region context.
            _loggerDataModel = (LogModel)regionManager.Regions[RegionNames.StatusBarRegion].Context;
            
            // Get the currently selected settings.
            SelectedDetail = _loggerDataModel.GetSelectedLogDetail();
            SelectedFileType = _loggerDataModel.GetSelectedLogFileTypes();

            LogLifeCollection.CurrentChanged += OnLogLifeChange;
            LogLifeCollection.MoveCurrentTo(Convert.ToInt32(_loggerDataModel.GetSelectedLogDetail()));
        }
        
        /// <summary>
        /// When a different log detail level is selected, update the data model.
        /// </summary>
        private void OnLogLevelChange()
        {
            _loggerDataModel.SetLogLevel(SelectedDetail.ToString());
        }

        /// <summary>
        /// When a different log file type is selected, update the data model.
        /// </summary>
        private void OnLogFileTypeChange()
        {
            _loggerDataModel.SetLogFileType(SelectedFileType.ToString());
        }

        /// <summary>
        /// When a different log level is selected, update the data model.
        /// </summary>
        private void OnLogLifeChange(object sender, EventArgs e)
        {
            _loggerDataModel.SetLogLife(Convert.ToInt32(LogLifeCollection.CurrentItem));
        }
        #endregion
    }
}   //ImprezGarage.Modules.Logger.ViewModels namespace 