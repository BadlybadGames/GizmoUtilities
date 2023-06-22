using System;
using System.Collections.Generic;
using System.Reflection;
using BBG.GizmoUtilities.Common;
using GizmoUtilities.Editor.Settings;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace GizmoUtilities.Editor
{
    public static class HandleUtility
    {
        [DidReloadScripts]
        static void Init()
        {
            SceneView.duringSceneGui -= OnSceneView;
            SceneView.duringSceneGui += OnSceneView;
        }

        public static void OnSceneView(SceneView view)
        {
            if (!GizmoSettings.enabled)
            {
                return;
            }
            if (Event.current.type != EventType.Repaint)
            {
                return;
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

                    Vector3 offset = new Vector3(0, 2f, 0f);
                    float buttonSize = 1f;
                    var color = Handles.color;
                    foreach (MethodInfo method in typeInfo.GetMethods())
                    {
                        Handles.color = new Color(0.8f, 0.6f, 0, 0.5f);
                        var button = method.GetCustomAttribute<ButtonHandle>();
                        if (button != null)
                        {
                            var style = new GUIStyle(GUI.skin.button);

                            Quaternion oldrot = go.transform.rotation;
                            go.transform.LookAt(SceneView.currentDrawingSceneView.position.position);
                            Handles.Label(go.transform.position + offset, method.Name+"()", style:style);
                            Handles.color = Color.clear;
                            if (Handles.Button(go.transform.position + offset, go.transform.rotation, buttonSize, buttonSize,
                                    Handles.SphereHandleCap))
                            {
                                method.Invoke(behaviour, default);
                                Debug.Log("Clicked");
                            }

                            go.transform.rotation = oldrot;
                        }
                    }

                    Handles.color = color;

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
                            Vector3 updatedValue = Handles.FreeMoveHandle(targetPosition, size,
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