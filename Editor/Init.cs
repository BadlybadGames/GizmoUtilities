using BBG.GizmoUtility.GizmoUtility.Runtime;
using GizmoUtility.Editor.Settings;
using UnityEditor;
using UnityEngine;

namespace Utility
{
    [InitializeOnLoad]
    public static class Init
    {
        static Init()
        {
            if (GizmoSettings.autoInitialize)
            {
                InitGizmoUtility();
            }
        }

        static void InitGizmoUtility()
        {
            SceneView.duringSceneGui -= OnSceneView;
            SceneView.duringSceneGui += OnSceneView;

            if (GameObject.FindObjectOfType<GizmoUtility>())
            {
                Debug.LogError("Already initialized gizmos");
                return;
            }
            
            Debug.Log("Initializing gizmo utility");

            GameObject go = new GameObject("[GizmoUtility]");
            
            go.hideFlags = HideFlags.HideAndDontSave;
            //GameObject.DontDestroyOnLoad(go);
            var gu = go.AddComponent<GizmoUtility>();
            go.AddComponent<AttributeGizmos>();
        }

        private static void OnSceneView(SceneView obj)
        {
        }
    }
}