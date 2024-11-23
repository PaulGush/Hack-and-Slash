using UnityEngine;

namespace Code.Utils
{
    public class MonoInstance : MonoBehaviour
    {
        public static MonoInstance Instance;

        private void Start()
        {
            MonoInstance.Instance = this;
        }
    }
}