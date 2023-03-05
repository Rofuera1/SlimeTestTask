using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class Player : MonoBehaviour, IDamagable
    {
        protected static Player Instance;

        private float _startHealth => HealthValStat.Value;
        private float _health;
        private bool _isAlive;
        private float _healthRegenTime;

        public bool IsAlive => _isAlive;
        public static bool isAlive => Instance._isAlive;
        public float Health => _health;
        public float StartHealth => _startHealth;
        public float AttackValue => GetCurrentAttackValue();
        public static float AttackValueStatic => GetCurrentAttackValue();

        public UnityEvent OnDie { get; } = new UnityEvent();
        public UnityEvent OnCreate { get; } = new UnityEvent();
        public UnityEvent OnHealthChanged { get; } = new UnityEvent();

        private Stat HealthValStat;
        private Stat HealthRegenSpeedStat;
        private Stat AttackVal;

        private bool isRegeneratingHealth;

        private void Awake()
        {
            Instance = this;
            HealthValStat = Stats.GetStat(Stats.Types.HealthValue);
            HealthRegenSpeedStat = Stats.GetStat(Stats.Types.HealthRegenerationSpeed);
            HealthRegenSpeedStat.OnLevelChange.AddListener(() => { _healthRegenTime = HealthRegenSpeedStat.Value; });

            _health = _startHealth;
        }

        public static void GetDamageStatic(int damageValue) => Instance.GetDamage(damageValue);

        public virtual void GetDamage(int damageValue)
        {
            _health -= damageValue;
            regenerateHealth();

            if (_health <= 0f)
                die();

            OnHealthChanged?.Invoke();
        }

        protected virtual void die()
        {
            _isAlive = false;

            OnDie?.Invoke();
        }

        private void regenerateHealth()
        {
            if (isRegeneratingHealth) return;

            StartCoroutine(regenerateHealthCoroutine());
        }

        private IEnumerator regenerateHealthCoroutine()
        {
            isRegeneratingHealth = true;
            while (_health < _startHealth)
            {
                float t = 0f;
                while(t < _healthRegenTime)
                {
                    t += Time.deltaTime;

                    yield return null;
                }

                _health += _health * 0.05f;
                OnHealthChanged?.Invoke();
                yield return null;
            }
            isRegeneratingHealth = false;
        }


        public static float GetCurrentAttackValue()
        {
            return Instance.AttackVal.Value;
        }
    }
}
