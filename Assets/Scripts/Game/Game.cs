using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    public static bool IsActive { get; private set; }
    public static int CurrentLevel { get; private set; }
    public static int CurrentWave { get; private set; }

    public static UnityEvent OnGameStart = new UnityEvent();
    public static UnityEvent OnStartNextWave = new UnityEvent();
    public static UnityEvent OnNextWaveStarted = new UnityEvent();
    public static UnityEvent OnEndWave = new UnityEvent();
    public static UnityEvent OnEndGame = new UnityEvent();

    public Player.Movement Mover;

    private IEnumerator checkerCor;

    private void Awake()
    {
        checkerCor = checkForWin();
        Mover.OnFinishedMoving.AddListener(startNewWave);
    }

    public void StartGame()
    {
        IsActive = true;
        OnGameStart?.Invoke();

        StartCoroutine(checkerCor);

        startNewWave();
    }

    private void startNewWave()
    {
        OnNextWaveStarted?.Invoke();
    }

    private IEnumerator checkForWin()
    {
        while(IsActive)
        {
            yield return null;

            if (Enemies.EnemiesLeftToSpawnOnWave <= 0 && Enemies.EnemiesOnScene <= 0)
                wonWave();
        }
    }

    private void wonWave()
    {
        StopCoroutine(checkerCor);

        if(Enemies.EnemiesLeftToSpawnOnLevel <= 0)
        {
            wonGame();
            Debug.Log("Game is won-2");
            return;
        }

        Debug.Log("Game is not won");
        OnEndWave?.Invoke();
    }

    private void wonGame()
    {
        Debug.Log("Game is won");
        OnEndGame?.Invoke();
        IsActive = false;
    }
}
