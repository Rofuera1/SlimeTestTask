using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        [SerializeField]
        private float damageCountdown;
        [SerializeField]
        private int _startHealth;
        private float _health;
        private float _speed;
        [SerializeField]
        private float _startSpeed;
        [SerializeField]
        private int _attackValue;
        private bool _isAlive;

        public bool IsAlive => _isAlive;
        public float Health => _health;
        public float StartHealth => _startHealth;
        public float CurrentSpeed => _speed;
        public float AttackValue => _attackValue;

        public UnityEvent OnDie { get; } = new UnityEvent();
        public UnityEvent OnCreate { get; } = new UnityEvent();
        public UnityEvent OnHealthChanged { get; } = new UnityEvent();



        private IEnumerator walk;
        public UnityEvent ReachedPlayer { get; } = new UnityEvent();
        private IEnumerator attack;

        public void Init()
        {
            _health = _startHealth;
            _speed = _startSpeed;
            _isAlive = true;

            startWalking();
            ReachedPlayer.AddListener(startAttacking);

            OnCreate?.Invoke();
        }

        public virtual void GetDamage(float damageValue)
        {
            _health -= damageValue;

            if (_health <= 0f)
                die();

            OnHealthChanged?.Invoke();
        }

        protected virtual void startWalking()
        {
            if (walk != null) StopCoroutine(walk);
            walk = walkCoroutine();
            StartCoroutine(walk);
        }

        private IEnumerator walkCoroutine()
        {
            while(Vector3.Distance(transform.position, Player.Movement.PlayerPosition.position) > 1f)
            {
                yield return new WaitForFixedUpdate();

                transform.position += (Player.Movement.PlayerPosition.position - transform.position).normalized * _speed * Time.fixedDeltaTime;
            }

            ReachedPlayer?.Invoke();
        }

        private void startAttacking()
        {
            stopAttacking();
            attack = attackingCoroutine();
            StartCoroutine(attack);
        }

        private IEnumerator attackingCoroutine()
        {
            while(Player.Player.isAlive)
            {
                yield return new WaitForSeconds(damageCountdown);

                Player.Player.GetDamageStatic(_attackValue);
            }
        }

        private void stopAttacking()
        {
            if (attack != null) StopCoroutine(attack);
        }

        protected virtual void die()
        {
            _isAlive = false;
            stopAttacking();

            OnDie?.Invoke();

            Destroy(gameObject);
        }
    }
}
