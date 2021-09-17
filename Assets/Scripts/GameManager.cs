using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {

        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(hp);
            Destroy(menu);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Ресурсы
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public Animator deathmenu;
    public Animator startmenu;
    public GameObject hp;
    public GameObject hud;
    public GameObject menu;

    // Логика
    public int coins;
    public int exp;

    // Старт
    //private int ng_tag = 0;

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public bool TryUpgradeWeapon()
    {
        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        
        if (coins >=  weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (exp >= add)
        {
            add += xpTable[r];
            r++;

            if (r == (xpTable.Count - 1))
                return r;
        }

        return r;
    }

    public int GetXpToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXp(int xp)
    {
        int currentLevel = GetCurrentLevel();
        exp += xp;
        while (currentLevel < GetCurrentLevel())
        {
            player.OnLevelUp();
            currentLevel++;
            ShowText("Level Up!", 50, Color.yellow,player.transform.position, Vector3.up, 1.0f);
        }
        ShowText("+" + xp + " xp", 30, Color.magenta, player.transform.position+Vector3.down/10, Vector3.up, 1.0f);
    }

    public void OnHPChange()
    {
        float ratio = (float)player.hitPoints / (float)player.maxhitPoints;
        // 0.18 и 0.75 - границы сердца (0.75 - 0.18 = )
        hp.GetComponent<Image>().fillAmount = 0.18f + 0.57f*ratio;
    }

    public void Respawn()
    {
        deathmenu.SetTrigger("Hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
        player.Respawn();
    }

    public void NewGame()
    {
        weapon.weaponLevel = 0;
        startmenu.SetTrigger("Hide");
        player.Respawn();
    }



    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += coins.ToString() + "|"; 
        s += exp.ToString() + "|";
        s += weapon.weaponLevel.ToString() + "|";
        s += player.maxhitPoints.ToString() + "|";
        s += player.hitPoints.ToString() + "|";
        s += player.currentcharacter.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("SaveState", s);
    }

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("Spawn").transform.position;
        //if (ng_tag == 0)
        //    startmenu.SetTrigger("Show");
        //ng_tag++;
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data=PlayerPrefs.GetString("SaveState").Split('|');

        player.transform.position = GameObject.Find("Spawn").transform.position;

        coins = int.Parse(data[1]);
        exp = int.Parse(data[2]);
        weapon.weaponLevel = int.Parse(data[3]);
        weapon.SetWeaponLevel(int.Parse(data[3]));
        player.maxhitPoints = int.Parse(data[4]);
        player.hitPoints = int.Parse(data[5]);
        player.currentcharacter = int.Parse(data[6]);

        OnHPChange();
    }

}
