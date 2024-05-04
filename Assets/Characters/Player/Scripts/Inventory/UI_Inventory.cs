using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField]
    Transform slotsContainer;
    [SerializeField]
    Transform itemSlotTemplate;
    [SerializeField]
    TextMeshProUGUI goldText;

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        goldText.text = "Gold:" + inventory.GetGold().ToString();

        inventory.onItemListChanged += Iventory_OnItemListChanged;
        inventory.onGoldChanged += Invetory_OnGoldChanged;
        DialogueManager.onSellingItems += Inventory_OnSellingItems;
        RefreshInventoryItems();
    }
    
    private void OnEnable() {
        inventory.onItemListChanged += Iventory_OnItemListChanged;
        inventory.onGoldChanged += Invetory_OnGoldChanged;
        DialogueManager.onSellingItems += Inventory_OnSellingItems;
        RefreshInventoryItems();
    }

    private void OnDisable() {

        inventory.onItemListChanged -= Iventory_OnItemListChanged;
        inventory.onGoldChanged -= Invetory_OnGoldChanged;
        DialogueManager.onSellingItems -= Inventory_OnSellingItems;
        
    }

    private void Inventory_OnSellingItems()
    {
        inventory.mode = Inventory.Mode.Selling;
        gameObject.SetActive(true);
        Debug.Log("gameobejct is: " + gameObject);
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void Invetory_OnGoldChanged(int n)
    {
        goldText.text = "Gold: " + n;
    }

    private void Iventory_OnItemListChanged(object sender, EventArgs e)
    {
       RefreshInventoryItems();
    }

    void RefreshInventoryItems()
    {
        foreach (Transform child in slotsContainer)
        {
            //if(child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransformed = Instantiate(itemSlotTemplate, slotsContainer).GetComponent<RectTransform>();

            itemSlotRectTransformed.GetComponent<Button_UI>().SetItem(item);
            itemSlotRectTransformed.GetComponent<Button_UI>().ui_Inventory = this;

            Image image = itemSlotRectTransformed.Find("Item Image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI costText = itemSlotRectTransformed.Find("Cost").GetComponent<TextMeshProUGUI>();
            costText.text = item.GetPrice().ToString() + " Gold";

            itemSlotRectTransformed.gameObject.SetActive(true);
        }

        int filledInventory = inventory.GetItemList().Count;

        for (int i = filledInventory; i < 10; i++)
        {
            RectTransform itemSlotRectTransformed = Instantiate(itemSlotTemplate, slotsContainer).GetComponent<RectTransform>();


            Transform itemImageTransform = itemSlotRectTransformed.Find("Item Image");

            if (itemImageTransform != null)
            {
                UnityEngine.UI.Image childImage = itemImageTransform.GetComponent<UnityEngine.UI.Image>();
                if (childImage != null)
                {
                    childImage.enabled = false;
                }
                else
                {
                    Debug.LogWarning("Image component not found in the 'Item Image' child of itemSlotTemplate.");
                }
            }
            else
            {
                Debug.LogWarning("Child named 'Item Image' not found in itemSlotTemplate.");
            }

            itemSlotRectTransformed.gameObject.SetActive(true);
        }
    }

    public void UseItem(Item item)
    {
        inventory.UseItem(item);
    }

    public void SellItem(Item item)
    {
        inventory.SellItem(item);
    }


}
