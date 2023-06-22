using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Utility
{
    public static class testdispatch
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Init()
        {
            InitHandlers();
            List<object> typs = new List<object>()
            {
                2f,
            };

            foreach (object typ in typs)
            {
                TypeInfo f = typ.GetType().GetTypeInfo();
                
                Dispatch(f, typ);
            }
        }

        private static Dictionary<Type, Action> handlers = new Dictionary<Type, Action>();
        private static void InitHandlers()
        {
            
            var implementers = Assembly.GetCallingAssembly().GetTypes().Where(t => t.GetCustomAttribute<GizmoTypeHandler>() != null)
                .ToArray();

            foreach (Type type in implementers)
            {
                Debug.Log("Test type: " + type.FullName);
                Debug.Log("a: " + type.IsGenericTypeDefinition);
                Debug.Log("b: " + type.GenericTypeArguments.Length);
                Debug.Log("c: " + type.GetGenericArguments().Length);
                Debug.Log(type.ContainsGenericParameters);
                Debug.Log(type.DeclaringType);
                //var typ = type.GenericTypeArguments[0];
                //var ctr = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null,
                    //CallingConventions.HasThis, Type.EmptyTypes, null);

                //var instance = ctr.Invoke(default) as C;
                //handlers[typ] = instance;
                //Debug.Log("Implemented for type: " + typ.FullName);
            }
        }

        private static void Dispatch(TypeInfo t, object value)
        {
            //handlers[t.GetType()].Apply(value);
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class GizmoTypeHandler : Attribute
        {
            public Type type;
        }


        [GizmoTypeHandler()]
        public abstract class C<T>
        {
            public abstract void Apply(T val);
        }

        [GizmoTypeHandler()]
        public class ConcreteA : C<float>
        {
            public override void Apply(float val)
            {
                Debug.Log("Hello from concrete, value: " + val);
            }
        }
    }
}