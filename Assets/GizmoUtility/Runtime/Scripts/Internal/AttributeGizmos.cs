using System;
using System.Reflection;
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
    
    public class AttributeGizmos : MonoBehaviour
    {
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
                                    if (!Selection.Contains(go))
                                    {
                                        continue;
                                    }
                                }

                                object t = field.FieldType;

                                if (field.FieldType == typeof(Vector3))
                                {
                                    var value = (Vector3)field.GetValue(component);
                                    Vector3 start = trans.position;
                                    Vector3 end = trans.position + value;
                                    if (attr.displayLength)
                                    {
                                        var midWay = Vector3.Lerp(start, end, 0.5f);
                                        Utility.GizmoUtility.Label(midWay + new Vector3(0, 0.5f, 0), value.magnitude.ToString("F1"));
                                    }
                                    Utility.GizmoUtility.Arrow(trans.position, value.normalized, value.magnitude, width, color:color);
                                }
                                else if (field.FieldType == typeof(float))
                                {
                                    var value = (float)field.GetValue(component);
                                    Vector3 start = trans.position;
                                    if (attr.displayLength)
                                    {
                                        Utility.GizmoUtility.Label(start + new Vector3(0, 0.5f, 0), value.ToString("F1"));
                                    }
                                    Utility.GizmoUtility.Sphere(trans.position, value, color:color);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}