using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_UI : MonoBehaviour
{
    private Item item;
    public UI_Inventory ui_Inventory;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void UseItem()
    {
        ui_Inventory.UseItem(item);

        GameObject.Find("Use Button").SetActive(false);
    }
}
