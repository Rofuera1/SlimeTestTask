using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementUpgrader : MonoBehaviour
{
    public Player.Stats.Types Type;
    public Text LevelText;
    public Text ValueText;
    public Text CostText;
    public Button UpgradeButton;

    private Player.Stat StatAttached;

    private void Awake()
    {
        StatAttached = Player.Stats.GetStat(Type);
        StatAttached.OnLevelChange.AddListener(updateInfo);
        UpgradeButton.onClick.AddListener(tryToUpgrade);

        updateInfo();
    }

    private void updateInfo()
    {
        LevelText.text = StatAttached.Level.ToString();
        ValueText.text = StatAttached.Value.ToString();
        CostText.text = StatAttached.UpgradeCost.ToString();
    }

    private void tryToUpgrade()
    {
        if(Wallet.Gems.TryToRemove(StatAttached.UpgradeCost))
        {
            StatAttached.UpgradeLevel(1);
        }
    }
}
