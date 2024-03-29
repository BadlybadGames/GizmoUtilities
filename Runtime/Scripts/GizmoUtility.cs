﻿using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using BBG.GizmoUtilities.Common.Settings;
#endif
using UnityEditor;
using UnityEngine;

namespace BBG.GizmoUtilities
{
    // Should only have a single instance!!
    /// <summary>
    /// Utilities for drawing gizmo-likes in a static manner
    /// </summary>
    #if UNITY_EDITOR
    [ExecuteAlways]
    #endif
    
    public class GizmoUtility : MonoBehaviour
    {
        private float previousUpdateTime = -1f;

#if UNITY_EDITOR
        private void OnEnable()
        {
            SceneView.duringSceneGui += this._OnSceneView;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= this._OnSceneView;
        }
#endif

        private class Job
        {
            public float duration = -1;
            internal float durationLeft = -1;
            public JobType typeOfJob;
            public object[] arguments;
            public Color color, EndColor;
            public bool AnimatedColor = false;

            public bool DoneOnce = false;

            // The gizmo will only be rendered if this gameobject is selected. Basically replicating OnGizmosSelected
            public GameObject MustBeSelected;


            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            // Auto-generated by visual studio
            public override int GetHashCode()
            {
                var hashCode = 2112717951;
                hashCode = hashCode * -1521134295 + duration.GetHashCode();
                hashCode = hashCode * -1521134295 + typeOfJob.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<object[]>.Default.GetHashCode(arguments);
                hashCode = hashCode * -1521134295 + EqualityComparer<Color>.Default.GetHashCode(color);
                hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(MustBeSelected);
                return hashCode;
            }
        }

        private enum JobType
        {
            Line,
            Lines,
            Label,
            Circle,
            Sphere,
        }

        public static Color color { private get; set; }


        public static void Circle(Vector3 position, float radius, Color? color = null, float duration = -1,
            GameObject mustBeSelected = null, bool animatedColor = false, Color endColor = default)
        {
            Job job = new Job();

            job.arguments = new object[] { position, radius };
            job.duration = duration;
            job.durationLeft = duration;
            job.typeOfJob = JobType.Circle;
            job.EndColor = endColor;
            job.AnimatedColor = animatedColor;

            job.MustBeSelected = mustBeSelected;

            // Use white if no color defined
            job.color = color.HasValue ? color.Value : Color.white;

            AddJob(job);
        }

        public static void Sphere(Vector3 position, float radius, Color? color = null, float duration = -1,
            GameObject mustBeSelected = null, bool animatedColor = false, Color endColor = default)
        {
            Job job = new Job();

            job.arguments = new object[] { position, radius };
            job.duration = duration;
            job.durationLeft = duration;
            job.typeOfJob = JobType.Sphere;
            job.EndColor = endColor;
            job.AnimatedColor = animatedColor;

            job.MustBeSelected = mustBeSelected;

            // Use white if no color defined
            job.color = color.HasValue ? color.Value : Color.white;

            AddJob(job);
        }

        /// <summary>
        /// Render a label using Handles.label if in editor.
        /// </summary>
        /// <param name="position">Position of text</param>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Color of text</param>
        /// <param name="duration">Duration to display text</param>
        public static void Label(Vector3 position, string text, Color? color = null, float duration = -1,
            GameObject mustBeSelected = null, bool animatedColor = false, Color endColor = default)
        {
            Job job = new Job();

            job.arguments = new object[] { position, text };
            job.duration = duration;
            job.durationLeft = duration;
            job.typeOfJob = JobType.Label;
            job.EndColor = endColor;
            job.AnimatedColor = animatedColor;

            job.MustBeSelected = mustBeSelected;

            // Use white if no color defined
            job.color = color.HasValue ? color.Value : Color.white;

            AddJob(job);
        }

        public static void ArrowTo(Vector3 startPosition, Vector3 endPosition, float width = 4.5f,
            Color? color = null, float duration = -1, GameObject mustBeSelected = null, bool animatedColor = false,
            Color endcolor = default)
        {
            var diff = endPosition - startPosition;
            Arrow(startPosition, diff.normalized, diff.magnitude, width, color, duration, mustBeSelected, animatedColor, endcolor);
        }
        public static void Arrow(Vector3 startPosition, Vector3 direction, float length, float width = 4.5f,
            Color? color = null, float duration = -1, GameObject mustBeSelected = null, bool animatedColor = false,
            Color endcolor = default)
        {
            direction.Normalize();
            Vector2 directionIn2d = (Vector2)direction;
            Vector3 endPosition = startPosition + direction * length;

            Line(startPosition, endPosition, width: width, color: color, animatedColor: animatedColor,
                endColor: endcolor, duration: duration, mustBeSelected: mustBeSelected);
            directionIn2d = directionIn2d.Rotate(135);
            Line(endPosition, endPosition + (Vector3)directionIn2d * 0.25f, width: width * 1.5f, color: color,
                animatedColor: animatedColor, endColor: endcolor, duration: duration, mustBeSelected: mustBeSelected);
            directionIn2d = directionIn2d.Rotate(90);
            Line(endPosition, endPosition + (Vector3)directionIn2d * 0.25f, width: width * 1.5f, color: color,
                animatedColor: animatedColor, endColor: endcolor, duration: duration, mustBeSelected: mustBeSelected);
        }

        public static void Lines(Vector3[] positions, float width = 3.5f, Color? color = null,
            float duration = -1, GameObject mustBeSelected = null, bool animatedColor = false, Color endColor = default)
        {
            Job job = new Job();

            job.arguments = new object[] { positions, width };
            job.duration = duration;
            job.durationLeft = duration;
            job.typeOfJob = JobType.Lines;
            job.EndColor = endColor;
            job.AnimatedColor = animatedColor;

            job.MustBeSelected = mustBeSelected;

            // Use white if no color defined
            job.color = color.HasValue ? color.Value : Color.white;

            AddJob(job);
        }

        public static void Line(Vector3 startPosition, Vector3 endPosition, float width = 3.5f, Color? color = null,
            float duration = -1, GameObject mustBeSelected = null, bool animatedColor = false, Color endColor = default)
        {
            Job job = new Job();

            job.arguments = new object[] { startPosition, endPosition, width };
            job.duration = duration;
            job.durationLeft = duration;
            job.typeOfJob = JobType.Line;
            job.EndColor = endColor;
            job.AnimatedColor = animatedColor;

            job.MustBeSelected = mustBeSelected;

            // Use white if no color defined
            job.color = color.HasValue ? color.Value : Color.white;

            AddJob(job);
        }

        private static void AddJob(Job job)
        {
            #if UNITY_EDITOR
            if (!GizmoSettings.enabled)
            {
                return;
            }
            #endif
            jobs.Add(job);
        }

        private static List<Job> jobs = new List<Job>();

        private void Update()
        {
            foreach (var job in jobs.ToArray())
            {
                if (!job.DoneOnce)
                    continue;

                job.durationLeft -= Time.deltaTime;
                if (job.durationLeft < 0)
                {
                    jobs.Remove(job);
                }
            }
        }

        private bool isSceneViewJob(Job job)
        {
            return new JobType[] { JobType.Sphere }.Contains(job.typeOfJob);
        }
        
        #if UNITY_EDITOR
        public void _OnSceneView(SceneView view)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            Color initialColor = Handles.color;
            foreach (var job in jobs.ToArray())
            {
                if (CheckJobIfSelected(job)) continue;
                Handles.color = job.color;
                
                job.DoneOnce = true;

                switch (job.typeOfJob)
                {
                    case JobType.Sphere:
                        var position = (Vector3)job.arguments[0];
                        var radius = (float)job.arguments[1];
                        
                        Handles.SphereHandleCap(
                            0,
                            position,
                            transform.rotation * Quaternion.LookRotation(Vector3.right),
                            radius * 2,
                            EventType.Repaint
                        );
                        break;
                }
            }

            Handles.color = initialColor;
        }
        #endif


        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }

            Color initialColor = Handles.color;
            foreach (Job job in jobs.ToArray())
            {
                if (isSceneViewJob(job))
                {
                    continue;
                }

                if (CheckJobIfSelected(job)) continue;

                job.DoneOnce = true;

                Handles.color = job.color;

                if (job.AnimatedColor)
                {
                    float percentLeft = 1 - job.durationLeft / job.duration;
                    Color color = Color.Lerp(job.color, job.EndColor, percentLeft);

                    Handles.color = color;
                }

                // Draw!!
                switch (job.typeOfJob)
                {
                    case JobType.Label:
                        Vector3 position = (Vector3)job.arguments[0];
                        String text = (String)job.arguments[1];

                        GUIStyle style = new GUIStyle();
                        style.normal.textColor = job.color;

                        Handles.Label(position, text, style);

                        break;
                    case JobType.Lines:
                        Vector3[] positions = (Vector3[])job.arguments[0];
                        float width = (float)job.arguments[1];
                        Handles.DrawAAPolyLine(width, positions);
                        break;
                    case JobType.Line:
                        Vector3 start = (Vector3)job.arguments[0];
                        Vector3 end = (Vector3)job.arguments[1];
                        width = (float)job.arguments[2];

                        Handles.DrawAAPolyLine(width, new Vector3[] { start, end });

                        break;

                    case JobType.Circle:
                        position = (Vector3)job.arguments[0];
                        float radius = (float)job.arguments[1];

                        Handles.DrawSolidDisc(position, Vector3.forward, radius);
                        break;
                }
            }
            Handles.color = initialColor;
#endif
        }

        private static bool CheckJobIfSelected(Job job)
        {
            #if UNITY_EDITOR
            // If the job is supposed to only be done while the gameobject is rendered, check for selection here.
            if (job.MustBeSelected != null)
            {
                if (!Selection.gameObjects.Contains(job.MustBeSelected))
                {
                    return true;
                }
            }
            #endif

            return false;
        }
    }
}