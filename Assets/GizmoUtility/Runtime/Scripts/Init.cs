using UnityEngine;

namespace Utility
{
    public static class Init
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitGizmoUtility()
        {
            if (!Settings.instance.AutoInitialize)
            {
                return;
            }

            GameObject go = new GameObject("[GizmoUtility]");
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<GizmoUtility>();
        }
    }
}