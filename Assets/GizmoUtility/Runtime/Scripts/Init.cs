using BBG.Physics.Internal;
using UnityEditor;
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
            var gu = go.AddComponent<GizmoUtility>();
            go.AddComponent<AttributeGizmos>();


        }

        private static void OnSceneView(SceneView obj)
        {
            Debug.Log(Event.current.type);
        }
    }
}