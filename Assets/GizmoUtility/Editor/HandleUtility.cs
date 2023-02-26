using System.Reflection;
using BBG.GizmoUtility.Common;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GizmoUtility.Editor
{
    public static class HandleUtility
    {
        [DidReloadScripts]
        static void Init()
        {
            SceneView.duringSceneGui -= OnSceneView;
            SceneView.duringSceneGui += OnSceneView;
        }

        static void OnSceneView(SceneView view)
        {
            if (Event.current.type != EventType.Repaint)
            {
                //return;
            }
            foreach (var go in Selection.gameObjects)
            {
                foreach (var behaviour in go.GetComponents<MonoBehaviour>())
                {
                    if (behaviour == null)
                    {
                        continue;
                    }
                    
                    var typeInfo = behaviour.GetType().GetTypeInfo();
                    var asm = behaviour.GetType().Assembly;

                    foreach (FieldInfo field in typeInfo.GetFields())
                    {
                        var gizmo = field.GetCustomAttribute<HandleGizmoAttribute>();
                        if (gizmo == null)
                        {
                            continue;
                        }
                    

                        if (field.FieldType == typeof(float))
                        {
                            float value = (float)field.GetValue(behaviour);
                            //var controlId = GUIUtility.GetControlID(FocusType.Passive);
                            float updatedValue = Handles.RadiusHandle(Quaternion.identity, go.transform.position, value);
                            field.SetValue(behaviour, updatedValue);
                            //Handles.SphereHandleCap(controlId, go.transform.position, Quaternion.identity, value, EventType.Repaint);
                            //UnityEditor.HandleUtility.AddControl(controlId, 0);
                        }

                        if (field.FieldType == typeof(Vector3))
                        {
                            Vector3 value = (Vector3)field.GetValue(behaviour);

                            var targetPosition = go.transform.position + value;
                            float size = UnityEditor.HandleUtility.GetHandleSize(targetPosition) * 0.5f;
                            Vector3 snap = Vector3.one * 0.5f;
                            
                            /*if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(example, "Change Look At Target Position");
                                example.targetPosition = newTargetPosition;
                                example.Update();
                            }*/
                            //Vector3 newTargetPosition = Handles.FreeMoveHandle(example.targetPosition, Quaternion.identity, size, snap, Handles.RectangleHandleCap);
                            Vector3 updatedValue = Handles.FreeMoveHandle(targetPosition, Quaternion.identity, size,
                                snap, Handles.SphereHandleCap);
                            
                            field.SetValue(behaviour, updatedValue - go.transform.position);
                            
                            Handles.DrawAAPolyLine(go.transform.position, updatedValue);
                        }
                    }
                }
            }
        }
    }
}