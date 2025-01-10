using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils;

namespace Code.Services
{
    public class ServiceLocator : MonoBehaviour
    {
        private static ServiceLocator _global;
        private static Dictionary<Scene, ServiceLocator> _sceneContainers;
        private static List<GameObject> tempSceneGameObjects;
        
        private readonly ServiceManager m_services = new ServiceManager();

        private const string k_globalServiceLocatorName = "ServiceLocator [Global]";
        private const string k_sceneServiceLocatorName = "ServiceLocator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (_global == this)
            {
                Debug.LogWarning("ServiceLocator::ConfigureAsGlobal: Already configured as global!", this);
            }
            else if (_global != null)
            {
                Debug.LogError("ServiceLocator::ConfigureAsGlobal: Global ServiceLocator already exists!", this);
            }
            else
            {
                _global = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (_sceneContainers.ContainsKey(scene))
            {
                Debug.LogError("ServiceLocator::ConfigureForScene: Scene ServiceLocator already exists!", this);
                return;
            }
            
            _sceneContainers.Add(scene, this);
        }

        public static ServiceLocator Global
        {
            get
            {
                if(_global != null) return _global;
                
                if(FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is { } found)
                {
                    found.BootstrapOnDemand();
                    return _global;
                }
                
                var container = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocator));
                container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
                
                return _global;
            }
        }
        
        public static ServiceLocator For(MonoBehaviour mb)
        {
            return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(mb) ?? Global; 
        }
        
        public static ServiceLocator ForSceneOf(MonoBehaviour mb)
        {
            Scene scene = mb.gameObject.scene;
            
            if (_sceneContainers.TryGetValue(scene, out ServiceLocator container) && container != mb)
            {
                return container;
            }
            
            tempSceneGameObjects.Clear();
            scene.GetRootGameObjects(tempSceneGameObjects);

            foreach (var gameObject in tempSceneGameObjects.Where(gameObject => gameObject.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
            {
                if (gameObject.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != mb)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return Global;
        }
        
        public ServiceLocator Register<T>(T service)
        {
            m_services.Register(service);
            return this;
        }
        
        public ServiceLocator Register<T>(Type type, object service)
        {
            m_services.Register(type, service);
            return this;
        }

        public ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service)) return this;
            
            if (TryGetNextInHierarchy(out ServiceLocator container))
            {
                container.Get(out service);
                return this;
            }

            throw new ArgumentException($"Service of type {typeof(T).FullName} not registered!");
        }
        
        private bool TryGetService<T>(out T service) where T : class
        {
            return m_services.TryGet(out service);
        }

        private bool TryGetNextInHierarchy(out ServiceLocator container)
        {
            if (this == _global)
            {
                container = null;
                return false;
            }
            
            container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
            return container != null;
        }

        private void OnDestroy()
        {
            if (this == _global)
            {
                _global = null;
            }
            else if (_sceneContainers.ContainsValue(this))
            {
                _sceneContainers.Remove(gameObject.scene);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _global = null;
            _sceneContainers = new Dictionary<Scene, ServiceLocator>();
            tempSceneGameObjects = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/ServiceLocator Global")]
        static void AddGlobal()
        {
            var go = new GameObject(k_globalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
        }
        
        [MenuItem("GameObject/ServiceLocator/ServiceLocator Scene")]
        static void AddScene()
        {
            var go = new GameObject(k_sceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
        }
#endif
    }
}