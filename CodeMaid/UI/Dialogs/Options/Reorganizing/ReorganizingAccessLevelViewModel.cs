#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing accessibility level options.
    /// </summary>
    public class ReorganizingAccessLevelViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
		/// Initializes a new instance of the <see cref="ReorganizingAccessLevelViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
		public ReorganizingAccessLevelViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Accessibility Level"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            Public = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelPublic);
			AssemblyOrFamily = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelAssemblyOrFamily);
			Project = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelProject);
			ProjectOrProtected = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelProjectOrProtected);
			Protected = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelProtected);
			Private = AccessLevelSetting.Deserialize(Settings.Default.Reorganizing_AccessLevelPrivate);

            CreateAccessLevelsFromCurrentState();
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
			Settings.Default.Reorganizing_AccessLevelPublic = Public.Serialize();
			Settings.Default.Reorganizing_AccessLevelAssemblyOrFamily = AssemblyOrFamily.Serialize();
			Settings.Default.Reorganizing_AccessLevelProject = Project.Serialize();
			Settings.Default.Reorganizing_AccessLevelProjectOrProtected = ProjectOrProtected.Serialize();
			Settings.Default.Reorganizing_AccessLevelProtected = Protected.Serialize();
			Settings.Default.Reorganizing_AccessLevelPrivate = Private.Serialize();
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessPublic"/> accessibility level.
		/// </summary>
		public AccessLevelSetting Public { get; set; }

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessAssemblyOrFamily"/> accessibility level.
		/// </summary>
		public AccessLevelSetting AssemblyOrFamily { get; set; }

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProject"/> accessibility level.
		/// </summary>
		public AccessLevelSetting Project { get; set; }

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProjectOrProtected"/> accessibility level.
		/// </summary>
		public AccessLevelSetting ProjectOrProtected { get; set; }

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProtected"/> accessibility level.
		/// </summary>
		public AccessLevelSetting Protected { get; set; }

		/// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessPrivate"/> accessibility level.
		/// </summary>
		public AccessLevelSetting Private { get; set; }

        #endregion Options

        #region Split Command

        private DelegateCommand _splitCommand;

        /// <summary>
        /// Gets the split command.
        /// </summary>
        public DelegateCommand SplitCommand
        {
            get { return _splitCommand ?? (_splitCommand = new DelegateCommand(OnSplitCommandExecuted)); }
        }

        /// <summary>
        /// Called when the <see cref="SplitCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnSplitCommandExecuted(object parameter)
        {
            var list = parameter as IList;
            if (list != null)
            {
                // Determine the position of the combined item and remove it.
                int index = AccessLevels.IndexOf(parameter);
                AccessLevels.Remove(parameter);

                // Reset each item in the list and insert it into the specified position.
				var accessLevelSettings = list.OfType<AccessLevelSetting>().Reverse();
                foreach (var accessLevelSetting in accessLevelSettings)
                {
                    accessLevelSetting.EffectiveName = accessLevelSetting.DefaultName;
                    AccessLevels.Insert(index, accessLevelSetting);
                }
            }
        }

        #endregion Split Command

		#region Logic

		/// <summary>
        /// Gets an observable collection of the accessibility levels.
        /// </summary>
        public ObservableCollection<object> AccessLevels
        {
            get { return GetPropertyValue<ObservableCollection<object>>(); }
            private set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Creates the accessibility levels collection from the current state.
        /// </summary>
        private void CreateAccessLevelsFromCurrentState()
        {
            var allAccessLevels = new[] { Public, AssemblyOrFamily, Project, ProjectOrProtected, Protected, Private };
            foreach (var accessLevel in allAccessLevels)
            {
                accessLevel.PropertyChanged += OnAccessLevelSettingPropertyChanged;
            }

            AccessLevels = new ObservableCollection<object>(allAccessLevels.GroupBy(x => x.Order)
                                                                         .Select(y => new List<object>(y))
                                                                         .OrderBy(z => ((AccessLevelSetting)z[0]).Order));

            AccessLevels.CollectionChanged += (sender, args) => UpdateAccessLevelSettings();
        }

        /// <summary>
        /// Handles a PropertyChanged event raised from a <see cref="AccessLevelSetting"/> and echoes it on the local object.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OnAccessLevelSettingPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var accessLevelSetting = sender as AccessLevelSetting;
            if (accessLevelSetting != null)
            {
                // Raise NotifyPropertyChanged on the DefaultName of the AccessLevelSetting which matches the property name on this class.
                RaisePropertyChanged(accessLevelSetting.DefaultName);

                // If the EffectiveName changed for one member in a group, be sure all other members in the group are synchronized.
                if (e.PropertyName == "EffectiveName")
                {
                    var list = AccessLevels.OfType<IList>().FirstOrDefault(x => x.Contains(accessLevelSetting));
                    if (list != null && list.Count > 1)
                    {
                        foreach (var accessLevel in list.OfType<AccessLevelSetting>())
                        {
                            accessLevel.EffectiveName = accessLevelSetting.EffectiveName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the accessibility level settings based on the current collection state.
        /// </summary>
        private void UpdateAccessLevelSettings()
        {
            int index = 1;

            foreach (var accessLevel in AccessLevels)
            {
                var accessLevelSetting = accessLevel as AccessLevelSetting;
                if (accessLevelSetting != null)
                {
                    accessLevelSetting.Order = index;
                }
                else
                {
                    var list = accessLevel as IList;
                    if (list != null)
                    {
						var levels = list.OfType<AccessLevelSetting>().ToList();

                        // If merged accessibility levels have distinct names, create a new effective name from joining their names together.
                        string newEffectiveName = null;
                        var distinctNames = levels.Select(x => x.EffectiveName).Distinct().ToList();
                        if (distinctNames.Count() > 1)
                        {
                            newEffectiveName = string.Join(" + ", distinctNames);
                        }

                        foreach (var level in levels)
                        {
                            level.Order = index;
                            if (!string.IsNullOrWhiteSpace(newEffectiveName))
                            {
                                level.EffectiveName = newEffectiveName;
                            }
                        }
                    }
                }

                index++;
            }
        }

        #endregion Logic
    }
}