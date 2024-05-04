using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{

    private Inventory inventory;

 

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

    }

    public void SellYellowOutfit()
    {
        Debug.Log("selling yellow");
        if(inventory.GetGold() >= 50)
        {
            
            inventory.AddItem(new Item{itemType = Item.ItemType.yellowOutfit,amount = 1});
            inventory.RemoveGold(50);
        }
        
    }
    public void SellBlueOutfit()
    {
       if(inventory.GetGold() >= 50)
        {
            inventory.AddItem(new Item{itemType = Item.ItemType.blueOutfit,amount = 1});
            inventory.RemoveGold(50);
        }
    }
    public void SellLeatherHat()
    {
        if(inventory.GetGold() >= 20)
        {
            inventory.AddItem(new Item{itemType = Item.ItemType.leatherHat,amount = 1});
            inventory.RemoveGold(20);
        }
    }
    public void SellWizardHat()
    {
        if(inventory.GetGold() >= 20)
        {
            inventory.AddItem(new Item{itemType = Item.ItemType.wizardHat,amount = 1});
            inventory.RemoveGold(20);
        }
    }
}
