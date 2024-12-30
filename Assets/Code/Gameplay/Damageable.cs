using System;
using UnityEngine;

namespace Code.Gameplay
{
    public abstract class Damageable : MonoBehaviour
    {
        public abstract event Action OnDamaged;
        public abstract event Action OnDeath;
        
        public abstract int TakeDamage(int damage);
    }
}
