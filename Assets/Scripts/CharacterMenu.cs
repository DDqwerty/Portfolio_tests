using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text levelText, hitpointText, coinsText, upgradeCostText, xpText;

    private int currentCharecterSelection;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    private void Start()
    {
        currentCharecterSelection = GameManager.instance.player.currentcharacter;
        OnSelectionChanged();
    }

    public void OnArrowClick(bool arrow)
    {
        if (arrow)
        {
            currentCharecterSelection++;

            if (currentCharecterSelection == GameManager.instance.playerSprites.Count)
                currentCharecterSelection = 0;
            OnSelectionChanged();
        }
        else
        {
            currentCharecterSelection--;

            if (currentCharecterSelection < 0)
                currentCharecterSelection = GameManager.instance.playerSprites.Count-1;
            OnSelectionChanged();
        }
    }
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharecterSelection];
        GameManager.instance.player.SwapSprite(currentCharecterSelection);
    }

    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    public void UpdateMenu()
    {
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponSprites.Count-1)
            upgradeCostText.text = "MAX";

        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();

        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoints.ToString() + " / " + GameManager.instance.player.maxhitPoints.ToString();
        coinsText.text = GameManager.instance.coins.ToString();

        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (currentLevel == GameManager.instance.xpTable.Count-1)
        {
            xpText.text = GameManager.instance.exp.ToString() + " Всего XP";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int preLevelXp = GameManager.instance.GetXpToLevel(currentLevel-1);
            int currentLevelXp = GameManager.instance.GetXpToLevel(currentLevel);

            int diff = currentLevelXp - preLevelXp;
            int currentXpIntoLevel = GameManager.instance.exp - preLevelXp ;

            float completionRatio = (float)currentXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currentXpIntoLevel.ToString() + " / " + diff.ToString();
        }
    }
}
