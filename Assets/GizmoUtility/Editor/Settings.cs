using UnityEngine;
using UnityEditor;

namespace Utility
{

    [FilePath("BBG/GizmoUtilitySettings.ini", FilePathAttribute.Location.PreferencesFolder)]
    public class Settings : ScriptableSingleton<Settings>
    {
        [SerializeField] public bool AutoInitialize = true;

    }
}