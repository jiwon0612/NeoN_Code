using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Work.Jiwon.Code.Player;

namespace Work.JW.Code.Core.Dependencies
{
    [DefaultExecutionOrder(-10)]
    public class Injecter : MonoBehaviour
    {
        private const BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private readonly Dictionary<Type, object> _registry = new Dictionary<Type, object>();

        private void Awake()
        {
            IEnumerable<IDependencyProvider> providers = FindMonoBehaviours().OfType<IDependencyProvider>();

            foreach (var pro in providers)
            {
                RegisterProvider(pro);
            }

            IEnumerable<MonoBehaviour> injectables = FindMonoBehaviours().Where(IsInjectable);

            foreach (var mono in injectables)
            {
                Inject(mono);
            }
        }

        private void Inject(MonoBehaviour mono)
        {
            Type type = mono.GetType();

            IEnumerable<FieldInfo> fields = type.GetFields(_bindingFlags);
            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(InjectAttribute)))
                {
                    object instance = _registry[field.FieldType];
                    field.SetValue(mono, instance);
                }
            }

            IEnumerable<MethodInfo> methods = type.GetMethods(_bindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));
            
            foreach (var method in methods)
            {
                Type[] requireParams = method.GetParameters().Select(p => p.ParameterType).ToArray();
                object[] paramValues = requireParams.Select(ResolveType).ToArray();

                method.Invoke(mono, paramValues);
            }
        }

        private object ResolveType(Type type)
        {
            _registry.TryGetValue(type, out object instance);
            return instance;
        }

        private bool IsInjectable(MonoBehaviour mono)
        {
            MemberInfo[] members = mono.GetType().GetMembers(_bindingFlags);

            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider pro)
        {
            if (Attribute.IsDefined(pro.GetType(), typeof(ProvideAttribute)))
            {
                _registry.Add(pro.GetType(), pro);
                return;
            }

            MethodInfo[] methods = pro.GetType().GetMethods(_bindingFlags);
            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                Type returnType = method.ReturnType;
                object returnInstance = method.Invoke(pro, null);
                Debug.Assert(returnInstance != null, $"Provide method return type is void {method.Name}");

                _registry.Add(returnType, returnInstance);
            }
        }

        private IEnumerable<MonoBehaviour> FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        }
    }
}