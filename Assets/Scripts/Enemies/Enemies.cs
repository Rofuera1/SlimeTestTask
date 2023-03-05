using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Enemy;

public class Enemies : MonoBehaviour
{
    public Human BasicEnemyPrefab;
    public Boss BossEnemyPrefab;
    [Space]
    public int EnemiesOnFirstLevel;
    public int EnemiesPerLevelUpgrade;
    [Space]
    public float SpawnTimeDelay;
    public const int WavesOnLevel = 3;

    public static List<Enemy.Enemy> EnemiesList { get; private set; }
    public static int EnemiesLeftToSpawnOnLevel { get; private set; }
    public static int EnemiesLeftToSpawnOnWave { get; private set; }
    private int EnemiesPerWave;
    public static int EnemiesOnScene => EnemiesList.Count;

    public static UnityEvent EnemySpawned = new UnityEvent();
    public static UnityEvent EnemyKilled = new UnityEvent();

    private IEnumerator spawnCoroutine;

    private void Awake()
    {
        Game.OnGameStart.AddListener(startLevelSpawning);
        Game.OnNextWaveStarted.AddListener(startWaveSpawning);
    }

    private void startLevelSpawning()
    {
        EnemiesLeftToSpawnOnLevel = EnemiesOnFirstLevel + EnemiesPerLevelUpgrade * Game.CurrentLevel;
        EnemiesPerWave = EnemiesLeftToSpawnOnLevel / 3;

        EnemiesList = new List<Enemy.Enemy>();
    }

    private void startWaveSpawning()
    {
        EnemiesLeftToSpawnOnWave = EnemiesPerWave;

        stopSpawning();
        spawnCoroutine = spawning();
        StartCoroutine(spawnCoroutine);
    }

    private void stopSpawning()
    {
        if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
    }

    private IEnumerator spawning()
    {
        while(EnemiesLeftToSpawnOnWave > 0)
        {
            spawnEnemy();

            yield return new WaitForSecondsRealtime(SpawnTimeDelay);
        }
    }

    private void spawnEnemy()
    {
        EnemiesLeftToSpawnOnWave--;
        EnemiesLeftToSpawnOnLevel--;

        Vector3 point = Level.LevelHandler.GetPointPosition(Game.CurrentWave + 1);
        Enemy.Enemy en = Instantiate(BasicEnemyPrefab, point, Quaternion.identity);
        en.Init();
        en.OnDie.AddListener(() => EnemyKilledCallback(en));

        EnemiesList.Add(en);

        EnemySpawned?.Invoke();
    }

    public void EnemyKilledCallback(Enemy.Enemy en)
    {
        if(EnemiesList.Contains(en))
        {
            EnemyKilled?.Invoke();
            EnemiesList.Remove(en);

            if (en is Human)
                Wallet.Gems.Add(Wallet.GemsPerKilledEnemy);
        }
    }
}
