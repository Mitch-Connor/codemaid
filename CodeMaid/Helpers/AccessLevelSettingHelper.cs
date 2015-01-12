#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class that simplifies access to <see cref="AccessLevelSetting"/> instances.
    /// </summary>
    public static class AccessLevelSettingHelper
    {
        #region Fields

        private static readonly CachedSetting<AccessLevelSetting> CachedPublicSettings;
        private static readonly CachedSetting<AccessLevelSetting> CachedAssemblyOrFamilySettings;
        private static readonly CachedSetting<AccessLevelSetting> CachedProjectSettings;
        private static readonly CachedSetting<AccessLevelSetting> CachedProjectOrProtectedSettings;
        private static readonly CachedSetting<AccessLevelSetting> CachedProtectedSettings;
        private static readonly CachedSetting<AccessLevelSetting> CachedPrivateSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
		/// The static initializer for the <see cref="AccessLevelSettingHelper"/> class.
        /// </summary>
		static AccessLevelSettingHelper()
        {
            CachedPublicSettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelPublic, AccessLevelSetting.Deserialize);
            CachedAssemblyOrFamilySettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelAssemblyOrFamily, AccessLevelSetting.Deserialize);
            CachedProjectSettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelProject, AccessLevelSetting.Deserialize);
            CachedProjectOrProtectedSettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelProjectOrProtected, AccessLevelSetting.Deserialize);
            CachedProtectedSettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelProtected, AccessLevelSetting.Deserialize);
            CachedPrivateSettings = new CachedSetting<AccessLevelSetting>(() => Settings.Default.Reorganizing_AccessLevelPrivate, AccessLevelSetting.Deserialize);            
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets an enumerable set of all of the settings.
        /// </summary>
		public static IEnumerable<AccessLevelSetting> AllSettings
        {
            get
            {
                return new[]
                {
                    PublicSettings, AssemblyOrFamilySettings, ProjectSettings, ProjectOrProtectedSettings,
                    ProtectedSettings, PrivateSettings
                };
            }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessPublic"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting PublicSettings
        {
            get { return CachedPublicSettings.Value; }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessAssemblyOrFamily"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting AssemblyOrFamilySettings
        {
            get { return CachedAssemblyOrFamilySettings.Value; }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProject"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting ProjectSettings
        {
            get { return CachedProjectSettings.Value; }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProjectOrProtected"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting ProjectOrProtectedSettings
        {
            get { return CachedProjectOrProtectedSettings.Value; }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessProtected"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting ProtectedSettings
        {
            get { return CachedProtectedSettings.Value; }
        }

        /// <summary>
		/// Gets the settings associated with the <see cref="vsCMAccess.vsCMAccessPrivate"/> accessibility level.
        /// </summary>
		public static AccessLevelSetting PrivateSettings
        {
            get { return CachedPrivateSettings.Value; }
        }        

        #endregion Properties
    }
}