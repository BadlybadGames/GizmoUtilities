using UnityEditor;

namespace GizmoUtility.Editor.Settings
{
    [FilePath("BBG/GizmoUtility/GizmoSettings.ini", FilePathAttribute.Location.PreferencesFolder)]
    public class GizmoSettings : ScriptableSingleton<GizmoSettings>
    {
        public bool Enabled = true;
        public bool OnlySelected = false;
    }
}