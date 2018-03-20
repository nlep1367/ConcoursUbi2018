using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeShopSpawn : MonoBehaviour {

    List<Shop> ShopList;

    GameObject selectedCoffeeShop;

    
    // Use this for initialization
	void Start () {
        ShopList = new List<Shop>();
	}
	
    public void SetShoplist()
    {
        var shopsTemp = GameObject.FindObjectsOfType<Shop>();

        foreach(var shop in shopsTemp)
            ShopList.Add(shop);
    }

    public void ActivateCoffeeShop()
    {
        System.Random rand = new System.Random();

        ShopList[rand.Next(0, ShopList.Count - 1)].isCoffeeShop = true;
    }
}
