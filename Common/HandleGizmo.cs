using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBG.GizmoUtility.Common
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