using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class LevelHandler : MonoBehaviour
    {
        protected static LevelHandler Instance;
        public Transform StartPoint;
        public float DistanceBetweenPoint;

        private List<GameObject> backgroundObjects;

        private void Awake()
        {
            Instance = this;
        }

        public static Vector3 GetPointPosition(int pointIndex)
        {
            return Instance.StartPoint.position + Instance.StartPoint.right * pointIndex * Instance.DistanceBetweenPoint;
        }

        public void GetBackground(IEnumerable<GameObject> objects)
        {
            backgroundObjects.AddRange(objects);
        }

        public void EraseBackground()
        {
            for (int i = 0; i < backgroundObjects.Count; i++)
                Destroy(backgroundObjects[i]);
            backgroundObjects.Clear();
        }
    }
}
