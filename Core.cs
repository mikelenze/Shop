using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public enum State
{
    Male, Female, Random
}

public enum SpriteState
{
    Adult, Baby
}

public enum MedicineType
{
    Pills,Vials,Scapel,Saline,Tablets,Lotion,Needles
}

[System.Serializable]
public class AllViruses
{
    public string virusName;
    public string category;
    public string virusInfo;

    public int virusVersion = 1;
    public int virusLevel;

    public Sprite virusPicture;

    public int experience;
    public int playerDamageLow;
    public int playerDamageHigh;
    public int NpcDamageLow;
    public int NpcDamageHigh;

    public float minAge;
    public float maxAge;

    public List<string> virusMessages = new List<string>();
    public List<string> neededMedicine = new List<string>();

    public SpriteState sprite;
    public State gender;

    public bool canSpawn;
    public bool wasIdentified;

    [HideInInspector]
    public string[] neededTest;
    [HideInInspector]
    public int finalAge = -1;
    [HideInInspector]
    public string virusAge;
    [HideInInspector]
    public Color virusColor;
    [HideInInspector]
    public Sprite[] pretestIcons;
    [HideInInspector]
    public TestName[] testData;
}

[System.Serializable]
public class VirusCategory
{
    public string category;
    public AllViruses[] virusInThisCategory;
}

[System.Serializable]
public class ItemDB
{
    public string itemListName;
    public List<Items> items = new List<Items>();
}

[System.Serializable]
public class Items
{
    public string itemName;
    public string itemClass;
    public string itemInfo;
    public int itemLevel;
    public int itemQuantity;
    public MedicineType type;
    public Color itemColor = new Color(1, 1, 1, 1);
    public Sprite imageBG;
    public Sprite imageBGColored;
    public List<string> playerAttackInfo = new List<string>();
    public int damage;
    public int price;
}

[System.Serializable]
public class AllTestNames
{
    public string testName;
    public string testDescription;
}

[System.Serializable]
public class TestName
{
    public string testName;
    public Sprite testPicture;
    public int activatingTimer = 0;
}

[System.Serializable]
public class CurrentBoughtTest
{
    public string virusName;
    public string enemyNamePrefab;
    public List<string> testNames = new List<string>();
}

public class Core : MonoBehaviour
{
    public static Core instance;

    public TextAsset adenovirus;
    public TextAsset bacteria;
    public TextAsset fungus;
    public TextAsset HIV;
    public TextAsset parasites;
    public TextAsset protazoa;

    public TextAsset testFungus;
    public TextAsset testBacteria;
    public TextAsset testAdenovirus;
    public TextAsset testHIV;
    public TextAsset testParasites;
    public TextAsset testProtazoa;

    public Text playerLevelUI;
    public Text playerGoldUI;
    public Text playerPointUI;
    public Text playerRubyUI;
    public Text playerNameUI;

    private string[] adenovirusNames;
    private string[] bacteriaNames;
    private string[] fungusNames;
    private string[] HIVNames;
    private string[] parasitesNames;
    private string[] protazoaNames;

    public List<string> testFungusNames = new List<string>();
    public List<string> testBacteriaNames = new List<string>();
    public List<string> testHIVNames = new List<string>();
    public List<string> testParasitesNames = new List<string>();
    public List<string> testProtazoaNames = new List<string>();
    public List<string> testAdenovirusNames = new List<string>();

    [HideInInspector]
    public List<VirusCategory> virusByCategory = new List<VirusCategory>();
    //[HideInInspector]
    public List<AllViruses> virusDatabaseFromCSV = new List<AllViruses>();
    [HideInInspector]
    public List<string> virusCategoryList = new List<string>();
    [HideInInspector]
    public int virusCount;
    public List<ItemDB> itemDatabase = new List<ItemDB>();
    public AllTestNames[] testNameDatabase;

    public float health = 100;
    public int money = 0;
    public int rubyHearts = 0;
    public int playerLevel = 1;
    public int experience = 0;
    public float currentExp = 0;
    public int[] levelExp;
    public float currentLevelExp = 0;
    public int inventorySlotsGainPerLevel = 1;
    public string playerName = "";
    public GameObject character;
    public bool inCombat = false;
    public bool invFull = false;
    public GameObject levelUpUI;
    public GameObject levelUpItemPrefab;
    public Transform levelUpGrid;
    public Transform levelUpBaseCotent;
    public string[] gameOverMessages;
    private Inventory playerInventory;
    private SaveLoad saveScript;
    public List<string> defeatedViruses = new List<string>();
    [HideInInspector]
    public List<string> defeatedVirusesSN = new List<string>();
    [HideInInspector]
    public List<Sprite> defeatedVirusesSprites = new List<Sprite>();
    public int testIconTimer = 5;
    //public int minFluidDamage = 1;
    //public int maxFluidDamage = 3;
    public GameObject canvasObj;
    public GameObject mainCam;
    public GameObject spawnSystemUI;

    [HideInInspector]
    public List<string> unlockedViruses = new List<string>();

    public bool unlockBacteria = true;

    public bool unlockFungus = true;

    public bool unlockHIV = true;

    public bool unlockAdenovirus = true;

    public bool unlockParasites = true;

    public bool unlockProtazoa = true;

    public bool inMap = false;

    public Sprite medicineTemplateBG;
    public Sprite medicineTemplateColor;

    public Texture FXAttack;

    public Text XPText;

    public Text selectionNameTxt;
    public GameObject renameObj;

    [HideInInspector]
    public List<CurrentBoughtTest> currentBoughtTest = new List<CurrentBoughtTest>();

    [HideInInspector]
    public int nextLevelExp;
    [HideInInspector]
    public bool inDialogue = false;
    [HideInInspector]
    public bool clearSave;

    public int enemyDefeatedCount = 0;
    public GameObject unlockBuildingPanel;

    //public ETCJoystick joystickController;

    public GameObject invetoryUI;
    public GameObject optionsUI;
    public GameObject shopUI;
    public GameObject shopHardCurrencyUI;

    public GameObject alertMainMenu;

    public List<string> enemiesNames = new List<string>();

    public GameObject expContainer;
    public Image expBar;

    public GameObject newLocalMapPrefabMsg;

    public bool disableTestPhase;

    [HideInInspector]
    public string charName;

    public GameObject optionsButtonUIContainer;
    public GameObject inventoryButtonUIContainer;
    public GameObject leaderboardButtonUIContainer;
    public GameObject playerStatsUIContainer;

    [HideInInspector]
    public Transform storyPosition1;
    [HideInInspector]
    public Transform storyPosition2;

    public Image mapDoctors;

    [HideInInspector]
    public bool hudPlayerOn;

    public string externalMapScene;
    public string internalMapScene;

    public GameObject itemNameShopUI;
    public GameObject itemInfoUI;
    public GameObject priceUI;
    public GameObject priceBG;
    public GameObject buyButtonUI;
    public GameObject gridShop;
    public GameObject imageBGUI;
    public GameObject imageColorUI;
    public GameObject baseContent;
    public GameObject fullMessageUI;

    public GameObject interiorMap;
    public bool canGoToInitialStory;
    

    // THE CORE SCRIPT IS THE MAIN SCRIPT IN THE WHOLE GAME, IF YOU WANT TO ACCESS AND MODIFY DATA (money, points, xp, etc) YOU WILL HAVE TO MANIPULATE THEM ONLY HERE
    // DON'T FORGET TO USE saveScript.Save() OR saveScript.Load() TO KEEP THOSES DATA CHANGED

    void Awake()
    {
        // Load all the viruses and test names from .txt files
        instance = this;

        DontDestroyOnLoad(transform.gameObject);

        adenovirusNames = adenovirus.text.Split("\n"[0]);
        bacteriaNames = bacteria.text.Split("\n"[0]);
        fungusNames = fungus.text.Split("\n"[0]);
        HIVNames = HIV.text.Split("\n"[0]);
        parasitesNames = parasites.text.Split("\n"[0]);
        protazoaNames = protazoa.text.Split("\n"[0]);
        playerInventory = gameObject.GetComponent<Inventory>();

        string[] tmp1 = testFungus.text.Split("\n"[0]);
        for (int i = 0; i < tmp1.Length; i++)
        {
            testFungusNames.Add(tmp1[i]);
        }
        string[] tmp2 = testAdenovirus.text.Split("\n"[0]);
        for (int i = 0; i < tmp2.Length; i++)
        {
            testAdenovirusNames.Add(tmp2[i]);
        }
        string[] tmp3 = testBacteria.text.Split("\n"[0]);
        for (int i = 0; i < tmp3.Length; i++)
        {
            testBacteriaNames.Add(tmp3[i]);
        }
        string[] tmp4 = testProtazoa.text.Split("\n"[0]);
        for (int i = 0; i < tmp4.Length; i++)
        {
            testProtazoaNames.Add(tmp4[i]);
        }
        string[] tmp5 = testParasites.text.Split("\n"[0]);
        for (int i = 0; i < tmp5.Length; i++)
        {
            testParasitesNames.Add(tmp5[i]);
        }

        if (gameObject.name != "Core")
        {
            gameObject.name = "Core";
        }

        if (PlayerPrefs.HasKey("USERNAME"))
            playerName = PlayerPrefs.GetString("USERNAME");

        inCombat = false;
        playerNameUI.text = "";
        playerNameUI.text = playerName;
    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == externalMapScene)
        {
            playerNameUI.text = playerName;
            Transform tmp = GameObject.Find("SpawnPoint").transform;
            character.transform.parent = null;
            character.transform.position = new Vector3(tmp.position.x, tmp.position.y, 0);
            character.SetActive(true);
            character.transform.parent = null;
            charName = character.transform.name;
            mainCam.transform.position = character.transform.position;
            mainCam.SetActive(true);
            mainCam.SendMessage("Start", SendMessageOptions.RequireReceiver);
            playerLevelUI.text = "" + playerLevel;
            playerGoldUI.text = money.ToString();
            playerRubyUI.text = rubyHearts.ToString();
            inMap = true;

            interiorMap = GameObject.Find("GridInterior Kos");

            /*Color32 newColor = new Color32(28, 167, 189, 255);
            mainCam.transform.GetComponent<Camera>().backgroundColor = newColor;*/

            shopUI.SetActive(false);
            shopHardCurrencyUI.SetActive(false);

            SceneController.instance.mainCurrentScene = "ExternalMap";
            DisableUI();
        }

        if (level == 18)
        {
            Transform tmp = GameObject.Find("SpawnPoint").transform;
            Debug.Log(tmp.transform.position);
        }

        else if (SceneManager.GetActiveScene().name == "MainMenuGame")
        {
            Shop.instance.shopReady = true;

            PlayerControllerOnOff(false);
            alertMainMenu.SetActive(false);
            canvasObj.SetActive(false);

            Color32 newColor = new Color32(68, 68, 68, 5);
            mainCam.transform.GetComponent<Camera>().backgroundColor = newColor;

            Button startbtn = GameObject.Find("StartBtn").GetComponent<Button>();
            startbtn.onClick.AddListener(LoadCharacterSelection);
        }

        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            SceneManager.LoadScene("CharacterSelection");
        }

        else if (SceneManager.GetActiveScene().name == "ExternalMap")
        {
            if (!GameBalancing.instance.noShowIntro)
            {
                IntroController.instance.StartIntro();
            }
            else
            {
                StartGameInScene(true);
                IntroController.instance.gameObject.SetActive(false);
            }
        }
    }

    public void StartGameInScene(bool canStartStory)
    {
        EnableUI();
        MusicController.instance.StartAmbientMusic();

        StartCoroutine(StartGameInSceneRoutine(canStartStory));

#if UNITY_ANDROID || UNITY_IOS
        UnityAdsManager.instance.inGameScene = true;
#endif
    }

    public IEnumerator StartGameInSceneRoutine(bool canStartStory)
    {
        yield return new WaitForSeconds(0.5f);
        PlayerController.instance.canMove = false;

        if (canStartStory)
        {
            InitialStory.instance.currentStory = "Me";
            NewDialogueSystem.instance.InstantiateVariableDialogue("Wait, what? Is there really an epidemic going on?", 3f, NewDialogueSystem.WhoCall.Story);
        }
        else
        {
            canGoToInitialStory = true;
        }

        yield return new WaitUntil(() => canGoToInitialStory);
        PlayerController.instance.canMove = true;

        if (canStartStory)
        {
            InitialStory.instance.CallInitStory1();
        }
        else
        {
            InitialStory.instance.SkipStory();
        }
    }

    AsyncOperation ao;
    IEnumerator LoadInternalLevel()
    {
        yield return new WaitForSeconds(1);

        ao = SceneManager.LoadSceneAsync(20);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            if (ao.progress == 0.9f)
            {
                yield return new WaitUntil(() => PlayerController.instance.inInteriorMap == true);
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    void Start()
    {
        saveScript = gameObject.GetComponent<SaveLoad>();

        for (int i = 0; i < virusByCategory.Count; i++)
        {
            virusCount += virusByCategory[i].virusInThisCategory.Length;
            virusCategoryList.Add(virusByCategory[i].category);

            for (int i2 = 0; i2 < virusByCategory[i].virusInThisCategory.Length; i2++)
            {
                //virusDatabaseFromCSV.Add(virusByCategory[i].virusInThisCategory[i2]);
                enemiesNames.Add(virusByCategory[i].virusInThisCategory[i2].virusName);
            }
        }

        ResetPlayerPrefsMessages();

        PlayerPrefs.DeleteAll();
    }

    public void UnlockVirusCategory(string type)
    {
        if (type == "HIV")
        {
            unlockHIV = !unlockHIV;
        }
        if (type == "Bacteria")
        {
            unlockBacteria = !unlockBacteria;
        }
        if (type == "Adenovirus")
        {
            unlockAdenovirus = !unlockAdenovirus;
        }
        if (type == "Parasites")
        {
            unlockParasites = !unlockParasites;
        }
        if (type == "Fungus")
        {
            unlockFungus = !unlockFungus;
        }
        if (type == "Protazoa")
        {
            unlockProtazoa = !unlockProtazoa;
        }

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == externalMapScene)
        {
            //float expNeed = ((float)experience) / ((float)levelExp[playerLevel]);
            //Debug.Log("expBar: " + expNeed + " || Current Exp: " + currentExp + " || Need Exp: " + currentLevelExp + " || Level: " + playerLevel);

            float expNeed = (currentExp / currentLevelExp);

            if (expNeed == 0)
            {
                expBar.fillAmount = 0.01f;
            }
            else
            {
                expBar.fillAmount = expNeed;
            }
        }
    }

    private int oldLevel;
    private int levelReach;

    public void UpdateStats(int moneyInt, int expInt, bool isLoad, int rubyReward = 0)
    {
        // The main function who updates the player stats (called from multiples locations)
        //Debug.Log("MoneyInt: " + moneyInt + " ExpInt: " + expInt + " isLoad: " + isLoad + " rubyReward: " + rubyReward);

        currentLevelExp = levelExp[playerLevel];

        if (moneyInt != 0 && !isLoad)
        {
            money += moneyInt;
            playerGoldUI.text = money.ToString();
        }
        if (moneyInt != 0 && isLoad)
        {
            money = moneyInt;
            playerGoldUI.text = money.ToString();
        }
        if (expInt != 0 && !isLoad)
        {
            experience += expInt;
            currentExp += expInt;
        }
        if (expInt != 0 && isLoad)
        {
            experience = expInt;
            currentExp = expInt;
        }
        if (rubyReward != 0 && !isLoad)
        {
            rubyHearts += rubyReward;
            playerRubyUI.text = rubyHearts.ToString();
        }
        if (rubyReward != 0 && isLoad)
        {
            rubyHearts = rubyReward;
            playerRubyUI.text = rubyHearts.ToString();
        }

        oldLevel = playerLevel;
        levelReach = playerLevel;

        StartCoroutine("CheckLevel");
    }

    IEnumerator CheckLevel()
    {
        yield return new WaitForSeconds(0.1f);

        if (currentExp >= currentLevelExp)
        {
            //Debug.Log("Subiu de lvl!!");
            levelReach++;
            currentLevelExp = levelExp[playerLevel + 1];
            SpawnSystem.instance.CallSpawm();
        }

        yield return new WaitForSeconds(0.1f);

        if (!NewDialogueSystem.instance.dialogueOn)
        {
            if (oldLevel < levelReach)
            {
                playerLevelUI.text = "" + levelReach.ToString();
                playerLevel = levelReach;
                levelUpUI.SetActive(true);
                levelUpUI.GetComponentInChildren<ParticleSystem>().Play();
                playerInventory.invSlots += inventorySlotsGainPerLevel;

                currentExp = 0;

                NewDialogueSystem.instance.DestroyBox();

                for (int i = 0; i < GameBalancing.instance.levelUpUnlock[levelReach - 2].itemSprite.Count; i++)
                {
                    GameObject obj = Instantiate(levelUpItemPrefab, transform.position, transform.rotation);
                    obj.name = "Item" + (i + 1);

                    obj.transform.GetComponent<Image>().sprite = GameBalancing.instance.levelUpUnlock[levelReach - 2].itemSprite[i];

                    obj.transform.SetParent(levelUpGrid);
                    obj.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    levelUpGrid.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 24, 0);

                    obj.transform.GetChild(0).transform.GetComponent<Text>().text = GameBalancing.instance.levelUpUnlock[levelReach - 2].itemText[i];
                }

                levelUpGrid.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(190 * GameBalancing.instance.levelUpUnlock[levelReach - 2].itemSprite.Count, 138);
                levelUpBaseCotent.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(190 * GameBalancing.instance.levelUpUnlock[levelReach - 2].itemSprite.Count, 138);

                transform.GetComponent<BattleManager>().CloseRemoteInventory();
                transform.GetComponent<BattleManager>().canOpenRemoteInv = false;

                Animator animator = character.gameObject.GetComponent<Animator>();

                if (levelReach >= GameBalancing.instance.firstSpriteChangeLvl && levelReach < GameBalancing.instance.secondSpriteChangeLvl)
                {
                    if (character.transform.name == "Man1")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.maleDoc1b;
                    }
                    else if (character.transform.name == "Man2")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.maleDoc2b;
                    }
                    else if (character.transform.name == "Woman1")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.femaleDoc1b;
                    }
                    else if (character.transform.name == "Woman2")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.femaleDoc2b;
                    }
                }
                else if (levelReach > GameBalancing.instance.secondSpriteChangeLvl)
                {
                    if (character.transform.name == "Man1")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.maleDoc1c;
                    }
                    else if (character.transform.name == "Man2")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.maleDoc2c;
                    }
                    else if (character.transform.name == "Woman1")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.femaleDoc1c;
                    }
                    else if (character.transform.name == "Woman2")
                    {
                        animator.runtimeAnimatorController = GameBalancing.instance.femaleDoc2c;
                    }
                }

                PlayerController.instance.ChangeAnimationArts();
            }
        }

#if UNITY_ANDROID || UNITY_IOS
        /*FirebaseManager fbManager = GameObject.Find("GameManagers").GetComponent<ApplicationManager>().GetFirebaseManager;
        if (fbManager != null && fbManager.User != null)
        {
            fbManager.User.level = playerLevel;
            fbManager.User.xp = experience;
            fbManager.saveUser();
        }*/
#endif
        saveScript.SaveGame();
    }

    public void HideLevelUp()
    {
        foreach (Transform child in levelUpGrid)
        {
            Destroy(child.GetComponent<Image>());
            Destroy(child.gameObject);
        }
    }

    public void RemoveRuby(int value)
    {
        rubyHearts -= value;
    }

    public void openSpawnSystem()
    {
        if (!inCombat)
        {
            spawnSystemUI.SetActive(true);
        }
    }

    public void UpdateName()
    {
        if (selectionNameTxt.text.Length > 1)
        {
#if UNITY_ANDROID || UNITY_IOS
           // GameObject.Find("GameManagers").GetComponent<ApplicationManager>().GetFirebaseManager.changeUserName(selectionNameTxt.text);
#endif
            PlayerPrefs.SetString("USERNAME", selectionNameTxt.text);
            playerNameUI.text = selectionNameTxt.text;
            renameObj.SetActive(false);
        }
        if (selectionNameTxt.text.Length <= 1)
        {
            //TTN.NotifyManager.Add("Your name is too short !", 1, 3, TTN.Corner.BottomRight);
            NewDialogueSystem.instance.InstantiateVariableDialogue("Your name is too short !", 2f, NewDialogueSystem.WhoCall.Variable);
        }

        PlayerControllerOnOff(true);
    }

    public void OpenRenameOption()
    {
        renameObj.SetActive(true);
    }

    // THIS FUNCTION IS CHECKING IF ANY ACHIEVEMENT IS DONE

    public void CheckAchievement(string tmp)
    {
        if (tmp == "defeated")
        {
            // Add your conditions here to get the achievement :  use the variable "enemyDefeatedCount" (int) to check how many enemies are actually killed by the player

            if (enemyDefeatedCount >= 5 && PlayerPrefs.GetInt("EnemyDefeated5") == 0)
            {
                //HelperScript.AchievementUnlockedHelper ("5 enemies defeated");
                //PlayerPrefs.SetInt ("EnemyDefeated5", 1);
            }
            if (enemyDefeatedCount >= 20 && PlayerPrefs.GetInt("EnemyDefeated20") == 0)
            {
                //HelperScript.AchievementUnlockedHelper ("20 enemies defeated");
                //PlayerPrefs.SetInt ("EnemyDefeated20", 1);
            }
            if (enemyDefeatedCount >= 50 && PlayerPrefs.GetInt("EnemyDefeated50") == 0)
            {
                //HelperScript.AchievementUnlockedHelper ("50 enemies defeated");
                //PlayerPrefs.SetInt ("EnemyDefeated50", 1);
            }
            if (enemyDefeatedCount >= 100 && PlayerPrefs.GetInt("EnemyDefeated100") == 0)
            {
                //HelperScript.AchievementUnlockedHelper ("100 enemies defeated");
                //PlayerPrefs.SetInt ("EnemyDefeated100", 1);
            }
        }
        // here, I will add the other achievement types later...
    }

    public void LoadCharacterSelection()
    {
        //Application.LoadLevel("CharacterSelection");
        SceneManager.LoadScene("CharacterSelection");
    }

    public void PlayerControllerOnOff(bool value)
    {
        //Debug.Log("PlayerControllerOnOff");
        if(value == false)
        {
            hudPlayerOn = false;
        }
        else
        {
            hudPlayerOn = true;
        }
    }

    public void OpenInventoryInBattle()
    {
        Inventory.instance.OpenInventory();
        transform.GetComponent<BattleManager>().CloseRemoteInventory();
    }

    public void CloseInventoryInBattle()
    {
        if (transform.GetComponent<BattleManager>().diagnosisDone)
        {
            transform.GetComponent<BattleManager>().OpenRemoteInventory();
        }
    }

    public void AlertMainMenu()
    {
        invetoryUI.SetActive(false);
        shopUI.SetActive(false);
        optionsUI.SetActive(false);

        alertMainMenu.SetActive(true);
        PlayerControllerOnOff(false);
    }

    public void CloseAlertMainMenu()
    {
        alertMainMenu.SetActive(false);
        PlayerControllerOnOff(true);
    }

    public void MainMenu()
    {
        saveScript.ClearSave();
        Shop.instance.ClearShop();
        SceneManager.LoadScene("MainMenuGame");
    }

    private bool canLocalMsg = true;
    private GameObject newLocaMapMsgProvisore;

    public IEnumerator NewLocalMapMsg(string msg)
    {
        CancelInvoke("DestroyNewLocalMapMsg");

        if (canLocalMsg)
        {
            canLocalMsg = false;

            GameObject newLocalMapMsg = Instantiate(newLocalMapPrefabMsg, transform.position, transform.rotation);

            newLocalMapMsg.transform.SetParent(GetComponent<Core>().canvasObj.transform);

            newLocaMapMsgProvisore = newLocalMapMsg;

            newLocalMapMsg.GetComponent<Text>().text = msg;

            newLocalMapMsg.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 280, 0);
            newLocalMapMsg.transform.GetComponent<RectTransform>().localScale = new Vector3(.3f, .3f, .3f);

            newLocalMapMsg.transform.DOScale(new Vector3(1, 1, 1), .8f);

            Debug.Log("msg: " + msg);

            Invoke("DestroyNewLocalMapMsg", GameBalancing.instance.timeLocalMsg);
        }
        else
        {
            Destroy(newLocaMapMsgProvisore);

            yield return new WaitForSeconds(0.3f);

            GameObject newLocalMapMsg = Instantiate(newLocalMapPrefabMsg, transform.position, transform.rotation);

            newLocalMapMsg.transform.SetParent(GetComponent<Core>().canvasObj.transform);

            newLocaMapMsgProvisore = newLocalMapMsg;

            newLocalMapMsg.GetComponent<Text>().text = msg;

            newLocalMapMsg.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 280, 0);
            newLocalMapMsg.transform.GetComponent<RectTransform>().localScale = new Vector3(.3f, .3f, .3f);

            newLocalMapMsg.transform.DOScale(new Vector3(1, 1, 1), .8f);

            Invoke("DestroyNewLocalMapMsg", GameBalancing.instance.timeLocalMsg);
        }
    }

    public void DestroyNewLocalMapMsg()
    {
        Destroy(newLocaMapMsgProvisore);
        canLocalMsg = true;
    }

    public void ClearPlayerInfo()
    {
        PlayerPrefs.SetInt("playerLevel", 1);
        PlayerPrefs.SetInt("experience", 0);
        PlayerPrefs.SetInt("money", 3000);

        health = 100;
        money = 3000;
        experience = 0;
        rubyHearts = 0;
        playerLevel = 1;

        XPText.text = "EXP " + 0 + "/" + nextLevelExp;
    }

    public void ClearEnemies()
    {
        defeatedViruses.Clear();
        defeatedVirusesSN.Clear();
        defeatedVirusesSprites.Clear();
    }

    public void ResetPlayerPrefsMessages()
    {
        PlayerPrefs.SetInt("Message1", 0);
        PlayerPrefs.SetInt("Message2", 0);
    }

    public void ExitGame()
    {
        //Leave the game

        saveScript.SaveGame();
        Application.Quit();
    }

    public void DisableUI()
    {
        optionsButtonUIContainer.SetActive(false);
        leaderboardButtonUIContainer.SetActive(false);
        inventoryButtonUIContainer.SetActive(false);
        playerStatsUIContainer.SetActive(false);
    }

    private bool isFirstTime;
    public void EnableUI()
    {
        optionsButtonUIContainer.SetActive(true);
        leaderboardButtonUIContainer.SetActive(true);
        inventoryButtonUIContainer.SetActive(true);
        playerStatsUIContainer.SetActive(true);

        if (!isFirstTime)
        {
          Inventory.instance.AddItem("Penicillin", 0, 5);
          isFirstTime = true;
        }
    }

    public GameObject skipAnimationBtn;
    public Image blackScreen;

    public void SkipAnimation(int from)
    {
        StartCoroutine(EndAnimationRoutine());
    }

    IEnumerator EndAnimationRoutine()
    {
        blackScreen.DOFade(1f, 1f);

        yield return new WaitForSeconds(1.0f);
        blackScreen.DOFade(0f, 1f);
        StartGameInScene(false);
        skipAnimationBtn.GetComponent<Button>().enabled = false;
        skipAnimationBtn.GetComponent<Image>().enabled = false;

        StopAllCoroutines();
        canGoToInitialStory = true;
        StopCoroutine(StartGameInSceneRoutine(false));

        NewDialogueSystem.instance.StopAnimate();
        PlayerController.instance.canMove = true;

        NewDialogueSystem.instance.FakeDestroyBox();
        InitialStory.instance.StopInitStory();
    }
}
