using System;

namespace BBG.GizmoUtilities.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class HandleGizmoAttribute : Attribute
    {
    
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonHandle : Attribute
    {
        
    }
}