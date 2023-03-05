using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public StartStats[] StartStats;
    public int GemsStartVal;
    public int KeysStartVal;

    private void Awake()
    {
        Player.Stats.Init(StartStats);
        Wallet.Init(GemsStartVal, KeysStartVal);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
