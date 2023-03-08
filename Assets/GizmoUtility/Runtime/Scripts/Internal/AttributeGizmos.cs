using System;
using System.Collections.Generic;
using System.Reflection;
using BBG.GizmoUtility.GizmoUtility.Runtime.FieldImplementations;
using GizmoUtility.Editor.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace BBG.GizmoUtility.GizmoUtility.Runtime
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GizmoAttribute : Attribute
    {
        public float size = -1;
        public bool displayLength = true;
        public float r=1, g=1, b=1, a=1;

        public Color GetColor => new Color(r, g, b, a);

        /// <summary>
        /// True if r/g/b/a has been changed by user
        /// </summary>
        public bool DefinedCustomColor =>
            Mathf.Approximately(r, 1) &&
            Mathf.Approximately(g, 1) &&
            Mathf.Approximately(b, 1) &&
            Mathf.Approximately(a, 1);
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class MustBeSelected : Attribute
    {
        public bool mustBeSelected = true;
    }
    
    [ExecuteAlways]
    public class AttributeGizmos : MonoBehaviour
    {

        private readonly Dictionary<Type, BaseFieldImplementation> _implementations = new()
            {
                { typeof(float), new FloatFieldImplementation() },
                { typeof(Vector3), new Vector3FieldImplementation() },
                { typeof(Vector3[]), new Vector3ArrayFieldImplementation() },
                { typeof(Transform), new TransformFieldImplementation() },
            };
        
        
        private void Update()
        {
            if (!GizmoSettings.enabled)
            {
                return;
            }
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                foreach (var go in scene.GetRootGameObjects())
                {
                    
                    Transform trans = go.transform;
                    foreach (var component in go.GetComponents<MonoBehaviour>())
                    {
                        if (component == null)
                        {
                            continue;
                        }
                        var typeInfo = component.GetType().GetTypeInfo();
                        
                        // Retrieve the gizmo from the class level. This will be used as default in case a field 
                        // does contain the attribute
                        var componentAttr = typeInfo.GetCustomAttribute<GizmoAttribute>();

                        // Ignore unity (and other inherited) fields
                        var fields = typeInfo.GetFields();
                        foreach (FieldInfo field in fields)
                        {
                            var attr = field.GetCustomAttribute<GizmoAttribute>() ?? componentAttr;
                            if (attr != null)
                            {
                                var selectedAttribute = field.GetCustomAttribute<MustBeSelected>();
                                bool mustBeSelected = selectedAttribute?.mustBeSelected ?? GizmoSettings.onlyWhileSelected;
                                float width = attr.size < 0 ? 4.5f : attr.size;
                                if (mustBeSelected)
                                {
                                    #if UNITY_EDITOR
                                    if (!Selection.Contains(go))
                                    {
                                        continue;
                                    }
                                    #endif
                                }

                                if (_implementations.ContainsKey(field.FieldType))
                                {
                                    _implementations[field.FieldType].Handle(field, go, component, attr);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}