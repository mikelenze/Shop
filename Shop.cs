using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject buttonUI;

    public bool isMainShop;

    public Text itemNameUI;
    public Text itemInfoUI;
    public Text priceUI;
    public Button buyButton;
    public Image pictureBG;
    public Image pictureColor;
    public GameObject shopUI;
    public Transform gridShop;
    private Core coreScript;
    private Transform canvasObj;
    private RectTransform panelUI;
    public string currentObjectName;
    private int currentPrice;
    private Inventory inventoryObj;
    public bool shopReady = false;
    public GameObject fullTextUI;
    private GameObject priceBG;

    public Button closeShopUI;

    public bool canShowAll;

    int indexListItem = 0;

    public string shopName = "NPC Shop";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        coreScript = GameObject.Find("Core").GetComponent<Core>();
        invScript = GameObject.Find("Core").GetComponent<Inventory>();

        gameObject.name = gameObject.name+"("+shopName+")";

        //Start with 5 pinicilin
        //inventoryObj.AddItem("Penicillin", 0);
        //Invoke("BuyStartItems", 1.0f);
    }

    void BuyStartItems()
    {
        ItemBought("Penicillin", 5);
    }

    public IEnumerator FindInGame()
    {
        shopUI = coreScript.shopUI;
        /*itemNameUI = GameObject.Find("itemNameShopUI").GetComponent<Text>();
        itemInfoUI = GameObject.Find("itemInfoUI").GetComponent<Text>();
        priceUI = GameObject.Find("priceUI").GetComponent<Text>();
        buyButton = GameObject.Find("BuyButtonUI").GetComponent<Button>();
        gridShop = GameObject.Find("GridShop").GetComponent<RectTransform>();
        pictureBG = GameObject.Find("ImageBGUI").GetComponent<Image>();
        pictureColor = GameObject.Find("ImageColorUI").GetComponent<Image>();
        coreScript = GameObject.Find("Core").GetComponent<Core>();
        panelUI = GameObject.Find("BaseContent").GetComponent<RectTransform>();
        fullTextUI = GameObject.Find("FullMessageUI");*/

        itemNameUI = coreScript.itemNameShopUI.GetComponent<Text>();
        itemInfoUI = coreScript.itemInfoUI.GetComponent<Text>();
        priceUI = coreScript.priceUI.GetComponent<Text>();
        buyButton = coreScript.buyButtonUI.GetComponent<Button>();
        gridShop = coreScript.gridShop.GetComponent<RectTransform>();
        pictureBG = coreScript.imageBGUI.GetComponent<Image>();
        pictureColor = coreScript.imageColorUI.GetComponent<Image>();
        panelUI = coreScript.baseContent.GetComponent<RectTransform>();
        fullTextUI = coreScript.fullMessageUI;
        priceBG = coreScript.priceBG;

        closeShopUI = shopUI.transform.Find("ExitButtonUI").GetComponent<Button>();
        inventoryObj = coreScript.gameObject.GetComponent<Inventory>();
        
        buyButton.onClick.RemoveAllListeners();
        closeShopUI.onClick.RemoveAllListeners();

        yield return new WaitForSeconds(0.5f);

        buyButton.onClick.AddListener(() => buy(currentObjectName));
        closeShopUI.onClick.AddListener(CloseShop);
    }


    void InitializeShop()
    {
        //Debug.Log("InitializeShop");
        if (!isMainShop)
        {
            return;
        }

        StartCoroutine(FindInGame());

        //GET THE CORRESPONDING CORE ITEMS LIST
        for (int i = 0; i < coreScript.itemDatabase.Count; i++)
        {
            if (coreScript.itemDatabase[i].itemListName == gameObject.name)
            {
                indexListItem = i;
            }
        }

        for (int i = 0; i < coreScript.itemDatabase[indexListItem].items.Count; i++)
        {
            if (!MainShop.instance.shopListNames.Contains(coreScript.itemDatabase[indexListItem].items[i].itemName))
            {
                //Debug.Log("!!!!!!! " + coreScript.itemDatabase[indexListItem].items[i].itemName);
                GameObject obj = Instantiate(buttonUI.gameObject, transform.position, transform.rotation);
                obj.name = coreScript.itemDatabase[indexListItem].items[i].itemName;

                MainShop.instance.curItems.Add(obj.gameObject);
                canvasObj = GameObject.Find("GameplayCanvas").transform;

                obj.GetComponent<ItemName>().itemLevel = coreScript.itemDatabase[indexListItem].items[i].itemLevel;

                obj.GetComponentInChildren<Text>().text = coreScript.itemDatabase[indexListItem].items[i].itemName;
                obj.GetComponentInChildren<Text>().transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponentInChildren<Text>().fontSize = 20;
                obj.GetComponentInChildren<Text>().resizeTextForBestFit = false;

                int wordCount = coreScript.itemDatabase[indexListItem].items[i].itemName.Split(' ').Length;

                if (wordCount <= 1)
                {
                    obj.GetComponentInChildren<Text>().lineSpacing = 1f;
                    obj.transform.Find("nameItem").transform.localPosition = new Vector3(0, -65f, 0);
                    obj.transform.Find("nameItem").GetComponent<RectTransform>().sizeDelta = new Vector2(130, 24);
                }
                else
                {
                    obj.GetComponentInChildren<Text>().lineSpacing = .72f;
                    obj.transform.Find("nameItem").transform.localPosition = new Vector3(0, -70f, 0);
                    obj.transform.Find("nameItem").GetComponent<RectTransform>().sizeDelta = new Vector2(130, 40);
                }

                foreach (Transform child in obj.transform)
                {
                    if (child.gameObject.name == "ImageBG")
                    {
                        if (coreScript.itemDatabase[indexListItem].items[i].imageBG != null)
                        {
                            child.gameObject.GetComponent<Image>().sprite = coreScript.itemDatabase[indexListItem].items[i].imageBG;
                        }
                        else
                        {
                            if (coreScript.itemDatabase[indexListItem].items[i].type == MedicineType.Pills)
                            {
                                child.gameObject.GetComponent<Image>().sprite = GameBalancing.instance.phillBGSprite;
                                child.gameObject.GetComponent<Image>().raycastTarget = false;
                                child.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);
                                child.gameObject.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
                            }
                        }
                    }
                    if (child.gameObject.name == "ImageColor")
                    {
                        if (coreScript.itemDatabase[indexListItem].items[i].imageBGColored != null)
                        {
                            child.gameObject.GetComponent<Image>().sprite = coreScript.itemDatabase[indexListItem].items[i].imageBGColored;
                        }
                        else
                        {
                            if (coreScript.itemDatabase[indexListItem].items[i].type == MedicineType.Pills)
                            {
                                child.gameObject.GetComponent<Image>().sprite = GameBalancing.instance.phillColorSprite;
                                child.gameObject.GetComponent<Image>().raycastTarget = false;
                                child.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);
                                child.gameObject.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
                            }
                        }

                        child.gameObject.GetComponent<Image>().color = coreScript.itemDatabase[indexListItem].items[i].itemColor;
                    }
                }

                obj.transform.SetParent(gridShop);
                obj.transform.position = panelUI.transform.position;

                MainShop.instance.shopList.Add(obj);
                MainShop.instance.shopListNames.Add(coreScript.itemDatabase[indexListItem].items[i].itemName);
            }
        }

        panelUI.sizeDelta = new Vector2(100, panelUI.sizeDelta.y + (MainShop.instance.curItems.Count * 10));
        panelUI.anchoredPosition = new Vector2(panelUI.anchoredPosition.x, panelUI.anchoredPosition.y - (MainShop.instance.curItems.Count * 30));

        for (int i3 = 0; i3 < MainShop.instance.curItems.Count; i3++)
        {
            string tmp;
            MainShop.instance.curItems[i3].GetComponent<ItemName>().getName();
            tmp = MainShop.instance.curItems[i3].GetComponent<ItemName>().nameObj;
            MainShop.instance.curItems[i3].GetComponent<Button>().onClick.AddListener(() => showItem(tmp));
        }

        shopUI.SetActive(false);
    }

    public GameObject bgShopItem;

    private Inventory invScript;

    public void OpenShop()
    {
        //Debug.Log("OpenShop: " + MainShop.instance.shopList.Count);
        StartCoroutine(FindInGame());
        shopUI.SetActive(true);
        
        for (int i = 0; i < MainShop.instance.shopList.Count; i++)
        {
            GameObject bg = Instantiate(bgShopItem, transform.position, transform.rotation);
            bg.transform.SetParent(MainShop.instance.shopList[i].transform);
            bg.transform.localScale = new Vector3(0.42f, 0.34f, 0.34f);
            bg.transform.localPosition = new Vector3(0f, 0f, 0f);
            bg.transform.SetAsFirstSibling();

            //Debug.Log("OpenShop " + i);

            MainShop.instance.shopList[i].transform.localScale = new Vector3(1, 1, 1);
            MainShop.instance.shopList[i].GetComponent<Image>().enabled = false;
            MainShop.instance.shopList[i].GetComponent<Button>().enabled = false;
        }

        for (int i2 = 0; i2 < MainShop.instance.curItems.Count; i2++)
        {
            string tmp;
            MainShop.instance.curItems[i2].GetComponent<ItemName>().getName();
            tmp = MainShop.instance.curItems[i2].GetComponent<ItemName>().nameObj;

            if (MainShop.instance.curItems[i2].GetComponent<ItemName>().itemLevel > coreScript.playerLevel && !canShowAll)
            {
                Color color1 = MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color;
                color1.a = 0.5f;
                MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color = color1;

                Color color2 = MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color;
                color2.a = 0.5f;
                MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color = color2;

                MainShop.instance.curItems[i2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => showItem(tmp));
            }
            else
            {
                Color color1 = MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color;
                color1.a = 1f;
                MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color = color1;

                Color color2 = MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color;
                color2.a = 1f;
                MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color = color2;

                MainShop.instance.curItems[i2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => showItem(tmp));
            }

            MainShop.instance.curItems[i2].transform.localScale = new Vector3(1f, 1f, 1f);
            //Debug.Log("curItems[i2].transform: " + MainShop.instance.curItems[i2].transform.name);
        }

        if (coreScript.invFull)
        {
            Debug.Log("1 invFull: " + coreScript.invFull);
            fullTextUI.SetActive(true);
        }
        if (!coreScript.invFull)
        {
            //Debug.Log("2 invFull: " + coreScript.invFull);

            fullTextUI = shopUI.transform.Find("FullMessageUI").gameObject;
            fullTextUI.SetActive(false);
        }

        coreScript.PlayerControllerOnOff(false);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        coreScript.PlayerControllerOnOff(true);
    }

    public void ClearShop()
    {
        for (int i = 0; i < coreScript.itemDatabase[indexListItem].items.Count; i++)
        {
            Destroy(MainShop.instance.shopList[i]);
            MainShop.instance.shopListNames.Clear();
        }
    }

    void Update()
    {
        if (coreScript.inCombat == true)
        {
            //shopUI.SetActive(false);
        }

        if (coreScript.canvasObj.activeSelf == true && shopReady == false && MainShop.instance.curItems.Count <= 0)
        {
            if (isMainShop)
            {
                //Core.instance.playerStatsUIContainer.SetActive(true);
                InitializeShop();
            }
        }
    }

    private int currentQuant;

    public void showItem(string nameObj)
    {
        //print("Item shown: " + nameObj);

        for (int i = 0; i < coreScript.itemDatabase[indexListItem].items.Count; i++)
        {
            if (coreScript.itemDatabase[indexListItem].items[i].itemName == nameObj)
            {
                if (coreScript.itemDatabase[indexListItem].items[i].itemLevel <= coreScript.playerLevel)
                {
                    buyButton.gameObject.SetActive(true);
                    priceUI.gameObject.SetActive(true);
                    priceBG.gameObject.SetActive(true);

                    currentObjectName = nameObj;
                    priceUI.text = "$:" + coreScript.itemDatabase[indexListItem].items[i].price;
                    currentPrice = coreScript.itemDatabase[indexListItem].items[i].price;
                    itemInfoUI.text = coreScript.itemDatabase[indexListItem].items[i].itemInfo;
                    itemNameUI.text = coreScript.itemDatabase[indexListItem].items[i].itemName;

                    currentQuant = coreScript.itemDatabase[indexListItem].items[i].itemQuantity;

                    if (coreScript.itemDatabase[indexListItem].items[i].type == MedicineType.Pills)
                    {
                        pictureBG.sprite = GameBalancing.instance.phillBGSprite;
                        pictureColor.sprite = GameBalancing.instance.phillColorSprite;
                        pictureColor.color = coreScript.itemDatabase[indexListItem].items[i].itemColor;

                        pictureBG.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);
                        pictureColor.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);

                        pictureBG.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                        pictureColor.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                    }
                }
                else
                {
                    if (!canShowAll)
                    {
                        itemInfoUI.text = "Can be used at level " + coreScript.itemDatabase[indexListItem].items[i].itemLevel + "!";
                        itemNameUI.text = coreScript.itemDatabase[indexListItem].items[i].itemName;

                        buyButton.gameObject.SetActive(false);
                        priceUI.gameObject.SetActive(false);
                        priceBG.gameObject.SetActive(false);

                        if (coreScript.itemDatabase[indexListItem].items[i].type == MedicineType.Pills)
                        {
                            pictureBG.sprite = GameBalancing.instance.phillBGSprite;
                            pictureColor.sprite = GameBalancing.instance.phillColorSprite;
                            pictureColor.color = coreScript.itemDatabase[indexListItem].items[i].itemColor;

                            pictureBG.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);
                            pictureColor.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);

                            pictureBG.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                            pictureColor.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                        }
                    }
                    else
                    {
                        buyButton.gameObject.SetActive(true);
                        priceUI.gameObject.SetActive(true);
                        priceBG.gameObject.SetActive(true);

                        currentObjectName = nameObj;
                        priceUI.text = "$:" + coreScript.itemDatabase[indexListItem].items[i].price;
                        currentPrice = coreScript.itemDatabase[indexListItem].items[i].price;
                        itemInfoUI.text = coreScript.itemDatabase[indexListItem].items[i].itemInfo;
                        itemNameUI.text = coreScript.itemDatabase[indexListItem].items[i].itemName;

                        currentQuant = coreScript.itemDatabase[indexListItem].items[i].itemQuantity;

                        if (coreScript.itemDatabase[indexListItem].items[i].type == MedicineType.Pills)
                        {
                            pictureBG.sprite = GameBalancing.instance.phillBGSprite;
                            pictureColor.sprite = GameBalancing.instance.phillColorSprite;
                            pictureColor.color = coreScript.itemDatabase[indexListItem].items[i].itemColor;

                            pictureBG.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);
                            pictureColor.gameObject.transform.localPosition = new Vector3(0f, 2f, 0);

                            pictureBG.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                            pictureColor.gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                        }
                    }
                }
            }
            
        }
    }

    public void buy(string nameObj)
    {
        //Debug.Log("Try buy: " + nameObj);

        bool ok = true;

        if (coreScript.money >= currentPrice && nameObj != null)
        {
            if (inventoryObj.curItemDatabase.Count > 0)
            {
                for (int i2 = 0; i2 < inventoryObj.curItemDatabase.Count; i2++)
                {
                    if (inventoryObj.curItemDatabase[i2].item.name == nameObj)
                    {
                        ok = false;
                        if (inventoryObj.curItemDatabase[i2].item.GetComponent<ItemName>().amount + 1 <= inventoryObj.maximumStack)
                        {
                            coreScript.UpdateStats(-currentPrice, 0, false);
                            inventoryObj.curItemDatabase[i2].item.GetComponent<ItemName>().updateAmount(1);
                            inventoryObj.curItemDatabase[i2].quantity += currentQuant;
                            Debug.Log("currentQuant: " + currentQuant + " / Current: " + inventoryObj.curItemDatabase[i2].quantity);
                            NewDialogueSystem.instance.InstantiateVariableDialogue("Bought " + nameObj + " (x"+ currentQuant + ")", 2f, NewDialogueSystem.WhoCall.Variable);
                        }
                    }
                }
            }
        }

        if (ok == true && coreScript.money >= currentPrice && nameObj != null && coreScript.invFull == false)
        {
            coreScript.UpdateStats(-currentPrice, 0, false);
            ItemBought(nameObj, currentQuant);
            NewDialogueSystem.instance.InstantiateVariableDialogue("Bought " + nameObj + " (x" + currentQuant + ")", 2f, NewDialogueSystem.WhoCall.Variable);
        }

        if (ok == true && coreScript.money >= currentPrice && nameObj != null && coreScript.invFull == true)
        {
            NewDialogueSystem.instance.InstantiateVariableDialogue("No more room for a new item !", 2f, NewDialogueSystem.WhoCall.Variable);
        }

        if (coreScript.money < currentPrice && coreScript.invFull == false)
        {
            NewDialogueSystem.instance.InstantiateVariableDialogue("Not enought money !", 2f, NewDialogueSystem.WhoCall.Variable);
        }

    }

    void ItemBought(string nameObj, int quant)
    {
        //print("Bought " + quant + "x: " + nameObj);

        if (quant > 1)
        {
            if (inventoryObj.curItemDatabase.Find(item => item.itemName == nameObj) != null)
            {
                CurItems currentItem = new CurItems();
                currentItem.itemName = nameObj;

                inventoryObj.curItemDatabase[inventoryObj.curItemDatabase.IndexOf(currentItem)].item.GetComponent<ItemName>().updateAmount(quant);

                Debug.Log(inventoryObj.curItemDatabase[inventoryObj.curItemDatabase.IndexOf(currentItem)].itemName);
            }
            else
            {
                inventoryObj.AddItem(nameObj, currentPrice, quant);
            }
        }
        else
        {
            inventoryObj.AddItem(nameObj, currentPrice, quant);

            //inventoryObj.curItemDatabase[0].item.GetComponent<ItemName>().updateAmount(1);
            //inventoryObj.curItemDatabase[0].quantity++;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OpenShop();

            Core.instance.DestroyNewLocalMapMsg();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CloseShop();
        }
    }

    public void ShowAllMedicines()
    {
        if (!canShowAll)
        {
            canShowAll = true;

            for (int i2 = 0; i2 < MainShop.instance.curItems.Count; i2++)
            {
                string tmp;
                MainShop.instance.curItems[i2].GetComponent<ItemName>().getName();
                tmp = MainShop.instance.curItems[i2].GetComponent<ItemName>().nameObj;

                if (MainShop.instance.curItems[i2].GetComponent<ItemName>().itemLevel > coreScript.playerLevel)
                {
                    Color color1 = MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color;
                    color1.a = 1f;
                    MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color = color1;

                    Color color2 = MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color;
                    color2.a = 1f;
                    MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color = color2;

                    MainShop.instance.curItems[i2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => showItem(tmp));
                }
            }

            Color btnColor = Inventory.instance.showAllMedicinesBtn.GetComponent<Image>().color;
            btnColor.a = .5f;

            Inventory.instance.showAllMedicinesBtn.GetComponent<Image>().color = btnColor;
            Inventory.instance.showAllMedicinesBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Disable all medicines!";
        }
        else
        {
            canShowAll = false;

            for (int i2 = 0; i2 < MainShop.instance.curItems.Count; i2++)
            {
                string tmp;
                MainShop.instance.curItems[i2].GetComponent<ItemName>().getName();
                tmp = MainShop.instance.curItems[i2].GetComponent<ItemName>().nameObj;

                if (MainShop.instance.curItems[i2].GetComponent<ItemName>().itemLevel > coreScript.playerLevel)
                {
                    Color color1 = MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color;
                    color1.a = 0.5f;
                    MainShop.instance.curItems[i2].transform.GetChild(1).GetComponent<Image>().color = color1;

                    Color color2 = MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color;
                    color2.a = 0.5f;
                    MainShop.instance.curItems[i2].transform.GetChild(2).GetComponent<Image>().color = color2;

                    MainShop.instance.curItems[i2].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => showItem(tmp));
                }
            }

            Color btnColor = Inventory.instance.showAllMedicinesBtn.GetComponent<Image>().color;
            btnColor.a = 1f;

            Inventory.instance.showAllMedicinesBtn.GetComponent<Image>().color = btnColor;
            Inventory.instance.showAllMedicinesBtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enable all medicines!";
        }
    }
}
