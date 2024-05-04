using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_UI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    
    private Item item;
    public UI_Inventory ui_Inventory;

    [Header("Use Button")]
    [SerializeField] GameObject useButton;
    [Header("Sell Button")]
    [SerializeField] GameObject sellButton;
    [Header("Price text")]
    [SerializeField] GameObject priceText;


    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void UseItem()
    {
        ui_Inventory.UseItem(item);

        GameObject.Find("Use Button").SetActive(false);
    }

    public void SellItem()
    {
        ui_Inventory.SellItem(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ui_Inventory == null){
            useButton.SetActive(true);
            return;
        }

        if(ui_Inventory.GetInventory().mode == Inventory.Mode.Browsing)
        {
            useButton.SetActive(true);
        }
        else if(ui_Inventory.GetInventory().mode == Inventory.Mode.Selling)
        {
            sellButton.SetActive(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ui_Inventory == null) return;

        if(ui_Inventory.GetInventory().mode == Inventory.Mode.Selling)
        {
            priceText.SetActive(true);
        }
        else
        {
            priceText.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(priceText.activeSelf)
        {
            priceText.SetActive(false);
        }
    }
}
