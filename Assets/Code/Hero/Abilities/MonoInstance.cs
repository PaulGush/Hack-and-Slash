using UnityEngine;

namespace Code.Hero.Abilities
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