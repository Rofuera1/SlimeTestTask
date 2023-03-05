using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Player
{
    public class Movement : MonoBehaviour
    {
        public static Transform PlayerPosition;

        public Player Player;
        public Transform PlayerParent;
        public float Speed;

        [HideInInspector]
        public UnityEvent OnFinishedMoving = new UnityEvent();

        private IEnumerator movingCoroutine;

        private void Awake()
        {
            PlayerPosition = PlayerParent;
            Init();
        }

        public void Init()
        {
            Game.OnEndWave.AddListener(startMovementToNextPoint);

            Player.OnDie.AddListener(stopMoving);
        }

        private void startMovementToNextPoint()
        {
            Debug.Log("Started moving to next point");

            stopMoving();
            movingCoroutine = move(Level.LevelHandler.GetPointPosition(Game.CurrentWave + 1));
            StartCoroutine(movingCoroutine);
        }

        private void stopMoving()
        {
            if (movingCoroutine != null) StopCoroutine(movingCoroutine);
        }

        private IEnumerator move(Vector3 toWhere)
        {
            Debug.Log("Started moving");
            while(Vector2.Distance(PlayerParent.position, toWhere) > 1f)
            {
                yield return null;
                Debug.Log("Moving");
                PlayerParent.position += (toWhere - PlayerParent.position).normalized * Time.deltaTime * Speed;
            }

            Debug.Log("Finished moving");
            OnFinishedMoving?.Invoke();
        }
    }
}