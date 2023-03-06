using System;
using System.Collections.Generic;
using System.Reflection;
using BBG.GizmoUtility.GizmoUtility.Runtime.Scripts.FieldImplementations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace BBG.Physics.Internal
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class GizmoAttribute : Attribute
    {
        public float size = -1;
        public bool mustBeSelected = false, displayLength = true;
        public float r=1, g=1, b=1, a=1;
    }
    
    [ExecuteAlways]
    public class AttributeGizmos : MonoBehaviour
    {

        private Dictionary<Type, BaseFieldImplementation> _implementations =
            new Dictionary<Type, BaseFieldImplementation>()
            {
                { typeof(float), new FloatFieldImplementation() },
                { typeof(Vector3), new Vector3FieldImplementation() },
                { typeof(Vector3[]), new Vector3ArrayFieldImplementation() },
                { typeof(Transform), new TransformFieldImplementation() },
            };
        
        
        private void Update()
        {
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
                                Color color = new Color(attr.r, attr.g, attr.b, attr.a);
                                float width = attr.size < 0 ? 4.5f : attr.size;
                                if (attr.mustBeSelected)
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