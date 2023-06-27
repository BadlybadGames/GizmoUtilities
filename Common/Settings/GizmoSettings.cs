using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SettingsManagement;
#endif

namespace BBG.GizmoUtilities.Common.Settings
{
    #if UNITY_EDITOR
    internal static class SettingsGroup
    {
        internal const string General = "General Settings";
        internal const string GeneralDefaults = "General Defaults";
        internal const string GizmoColors = "Gizmo Colors";
    }

    public static class GizmoSettings
    {
        [UserSetting(SettingsGroup.General, "Enabled")]
        public static GizmoSetting<bool> enabled = new GizmoSetting<bool>("general.enabled", true, SettingsScope.User);

        [UserSetting(SettingsGroup.General, "Show Only While Selected")]
        public static GizmoSetting<bool> onlyWhileSelected =
            new GizmoSetting<bool>("general.onlyWhileSelected", false, SettingsScope.User);

        [UserSetting(SettingsGroup.General, "Auto Initialize")]
        public static GizmoSetting<bool> autoInitialize =
            new GizmoSetting<bool>("general.autoInitialize", true, SettingsScope.User);

        [UserSetting(SettingsGroup.GizmoColors, "Sphere")]
        public static GizmoSetting<Color> sphereDefaultColor =
            new GizmoSetting<Color>("colors.default.sphere", new Color(1f, 1f, 1f, 1f), SettingsScope.User);

        [UserSetting(SettingsGroup.GizmoColors, "Line")]
        public static GizmoSetting<Color> lineDefaultColor =
            new GizmoSetting<Color>("colors.default.line", new Color(1f, 1f, 1f, 1f), SettingsScope.User);

        [UserSetting(SettingsGroup.GizmoColors, "Arrow")]
        public static GizmoSetting<Color> arrowDefaultColor =
            new GizmoSetting<Color>("colors.default.arrow", new Color(1f, 1f, 1f, 1f), SettingsScope.User);
    }

    #if UNITY_EDITOR
    public class GizmoSetting<T> : UserSetting<T>
    {
        public GizmoSetting(string key, T value, SettingsScope scope = SettingsScope.Project)
            : base(SettingsManager.instance, key, value, scope)
        {
        }

        GizmoSetting(UnityEditor.SettingsManagement.Settings settings, string key, T value,
            SettingsScope scope = SettingsScope.Project)
            : base(settings, key, value, scope)
        {
        }

        public static implicit operator T(GizmoSetting<T> val)
        {
            return (T)val.GetValue();
        }
    }
    #endif

    internal static class SettingsManager
    {
        internal const string k_PackageName = "com.bbg.gizmo-utility";

        static UnityEditor.SettingsManagement.Settings s_Instance;

        internal static UnityEditor.SettingsManagement.Settings instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new UnityEditor.SettingsManagement.Settings(k_PackageName);

                return s_Instance;
            }
        }
    }

    static class GizmoSettingsProvider
    {
        const string k_PreferencesPath = "Preferences/BBG/Gizmo Utility";

        [SettingsProvider]
        static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider(k_PreferencesPath,
                SettingsManager.instance,
                new[] { typeof(GizmoSettingsProvider).Assembly });

            return provider;
            // The last parameter tells the provider where to search for settings.
        }
    }
#endif
}