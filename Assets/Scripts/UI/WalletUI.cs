using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletUI : MonoBehaviour
{
    public Text GemsText;
    public Text KeysText;

    private void Awake()
    {
        Wallet.Gems.OnChanged.AddListener(updateGems);
        Wallet.Keys.OnChanged.AddListener(updateKeys);

        updateGems();
        updateKeys();
    }

    private void updateGems()
    {
        GemsText.text = Wallet.Gems.Val.ToString();
    }

    private void updateKeys()
    {
        KeysText.text = Wallet.Keys.Val.ToString();
    }
}
