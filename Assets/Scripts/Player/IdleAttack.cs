using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class IdleAttack : MonoBehaviour
    {
        public Bullet BulletPrefab;
        public Transform Nozzle;

        private int startTimeValue;
        private int currentTimeValue;
        private float currentTimeFromShoot => startTimeValue * (1f / Mathf.Sqrt(currentTimeValue));

        private Stat AttackSpeedStat;

        private IEnumerator shootingCoroutine;

        private void Awake()
        {
            AttackSpeedStat = Stats.GetStat(Stats.Types.AttackSpeed);

            AttackSpeedStat.OnLevelChange.AddListener(updateStatistics);
            Game.OnGameStart.AddListener(startShooting);

            loadStatistic();
        }

        private void loadStatistic()
        {
            currentTimeValue = AttackSpeedStat.Value;
            startTimeValue = AttackSpeedStat.StartValue;
        }

        private void updateStatistics()
        {
            currentTimeValue = AttackSpeedStat.Value;
        }

        private void startShooting()
        {
            stopShooting();
            shootingCoroutine = shooting();
            StartCoroutine(shootingCoroutine);
        }

        private IEnumerator shooting()
        {
            while(true)
            {
                float t = 0f;

                while (t < currentTimeFromShoot)
                {
                    t += Time.deltaTime;
                    yield return null;
                }

                yield return null;

                while (Enemies.EnemiesOnScene == 0)
                    yield return null;

                shoot();
            }
        }

        private void stopShooting()
        {
            if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
        }

        private Enemy.Enemy getClosestEnemy()
        {
            Enemy.Enemy closestEnemy = Enemies.EnemiesList[0];
            float closestDist = Vector2.Distance(Nozzle.position, closestEnemy.transform.position);

            foreach(Enemy.Enemy en in Enemies.EnemiesList)
            {
                float thisDist = Vector2.Distance(Nozzle.position, en.transform.position);
                if(thisDist < closestDist)
                {
                    closestDist = thisDist;
                    closestEnemy = en;
                }
            }

            return closestEnemy;
        }

        private void shoot()
        {
            Instantiate(BulletPrefab, Nozzle.position, Quaternion.identity).Init(getClosestEnemy());
        }
    }
}