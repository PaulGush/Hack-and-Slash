using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DependencyInjection
{
    public class Injector : MonoBehaviour
    {
        const BindingFlags k_bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        
        readonly Dictionary<Type, object> registry = new Dictionary<Type, object>();

        private void Awake()
        {

            var providers = FindMonoBehaviours().OfType<IDependencyProvider>();

            foreach (var provider in providers)
            {
                RegisterProvider(provider);
            }

            var injectables = FindMonoBehaviours().Where(IsInjectable);
            foreach (var injectable in injectables)
            {
                Inject(injectable);
            }
        }

        void Inject(object instance)
        {
            var type = instance.GetType();
            var injectableFields = type.GetFields(k_bindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception($"Failed to Inject {fieldType.Name} into {type.Name}");
                }
                
                injectableField.SetValue(instance, resolvedInstance);
                Debug.Log($"Injected {fieldType.Name} into {type.Name}.");
            }
        }

        object Resolve(Type type)
        {
            registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        static bool IsInjectable(MonoBehaviour obj)
        {
            var members = obj.GetType().GetMembers(k_bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        private void RegisterProvider(IDependencyProvider provider)
        {
            var methods = provider.GetType().GetMethods(k_bindingFlags);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute)))
                {
                    continue;
                }
                
                var returnType = method.ReturnType;
                var providedInstance = method.Invoke(provider, null);

                if (providedInstance != null)
                {
                    registry.Add(returnType, providedInstance);
                    Debug.Log($"Registered {returnType.Name} from {provider.GetType().Name}.");
                }
                else
                {
                    throw new Exception($"Provider {provider.GetType().Name} returned null for {returnType.Name}.");
                }
            }
        }

        static MonoBehaviour[] FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}