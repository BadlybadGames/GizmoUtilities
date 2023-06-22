using System.Reflection;
using GizmoUtility.Editor.Settings;
using UnityEngine;

namespace BBG.GizmoUtility.GizmoUtility.Runtime.FieldImplementations
{
    public abstract class BaseFieldImplementation
    {
        public abstract void Handle(FieldInfo field, GameObject go, Component behaviour, GizmoAttribute attribute);
    }
    
    public abstract class BaseTypedFieldImplementation<T>
    {
        public abstract void Handle(T value, GameObject go, Component behaviour, GizmoAttribute attribute);
    }

    public class FloatFieldImplementation : BaseFieldImplementation
    {
        public override void Handle(FieldInfo field, GameObject go, Component behaviour, GizmoAttribute attribute)
        {
            var color = attribute.DefinedCustomColor ? attribute.GetColor : GizmoSettings.sphereDefaultColor;
            Transform trans = go.transform;
            var value = (float)field.GetValue(behaviour);
            Vector3 start = trans.position;
            if (attribute.displayLength)
            {
                Utility.GizmoUtility.Label(start + new Vector3(0, value+0.2f, 0), value.ToString("F1"));
            }

            Utility.GizmoUtility.Sphere(trans.position, value, color: color);
        }
    }

    public class Vector3FieldImplementation : BaseFieldImplementation
    {
        public override void Handle(FieldInfo field, GameObject go, Component behaviour, GizmoAttribute attribute)
        {
            Vector3 value = (Vector3)field.GetValue(behaviour);
            float width = attribute.size < 0 ? 4.5f : attribute.size;
            var color = attribute.DefinedCustomColor ? attribute.GetColor : GizmoSettings.arrowDefaultColor;
            Transform trans = go.transform;
            Vector3 start = trans.position;
            Vector3 end = trans.position + value;
            if (attribute.displayLength)
            {
                var midWay = Vector3.Lerp(start, end, 0.5f);
                Utility.GizmoUtility.Label(midWay + new Vector3(0, 0.5f, 0), value.magnitude.ToString("F1"));
            }
            Utility.GizmoUtility.Arrow(trans.position, value.normalized, value.magnitude, width, color:color);
        }
    }
    public class Vector3ArrayFieldImplementation : BaseFieldImplementation
    {
        public override void Handle(FieldInfo field, GameObject go, Component behaviour, GizmoAttribute attribute)
        {
            Vector3[] value = (Vector3[])field.GetValue(behaviour);
            float width = attribute.size < 0 ? 4.5f : attribute.size;
            Color color = new Color(attribute.r, attribute.g, attribute.b, attribute.a);
            Utility.GizmoUtility.Lines(value, width, color:color);
        }
    }
    
    public class TransformFieldImplementation : BaseFieldImplementation
    {
        public override void Handle(FieldInfo field, GameObject go, Component behaviour, GizmoAttribute attribute)
        {
            Transform value = (Transform)field.GetValue(behaviour);
            if (value == default)
            {
                return;
            }
            float width = attribute.size < 0 ? 4.5f : attribute.size;
            Color color = new Color(attribute.r, attribute.g, attribute.b, attribute.a);
            Transform trans = go.transform;
            Vector3 start = trans.position;
            Vector3 end = value.transform.position;
            var diff = end - start;
            
            if (attribute.displayLength)
            {
                var midWay = Vector3.Lerp(start, end, 0.5f);
                Utility.GizmoUtility.Label(midWay + new Vector3(0, 0.5f, 0), diff.magnitude.ToString("F1"));
            }
            Utility.GizmoUtility.ArrowTo(trans.position, value.position, width, color:color);
        }
    }
}