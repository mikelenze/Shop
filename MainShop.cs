using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainShop : MonoBehaviour {

    public static MainShop instance;

    public List<GameObject> shopList = new List<GameObject>();
    public List<string> shopListNames = new List<string>();
    public List<GameObject> curItems = new List<GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        instance = this;
    }
}
