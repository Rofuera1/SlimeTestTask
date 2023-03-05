using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float Speed;

        private Transform targetTR;

        public void Init(Enemy.Enemy target)
        {
            targetTR = target.transform;
        }

        private void FixedUpdate()
        {
            if(targetTR)
            {
                transform.position += (targetTR.position - transform.position).normalized * Time.fixedDeltaTime * Speed;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.gameObject.GetComponent<Enemy.Enemy>())
            {
                collision.transform.gameObject.GetComponent<Enemy.Enemy>().GetDamage(1);// Player.AttackValueStatic);
                Destroy(gameObject);
            }
        }
    }
}
