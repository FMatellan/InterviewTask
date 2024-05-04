using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler onItemListChanged;
    public event Action<int> onGoldChanged;
    private List<Item> items = new List<Item>();
    private Action<Item> useItemAction;
    private int gold;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        gold = 100;
        AddItem(new Item{itemType = Item.ItemType.wood,amount = 1});
        AddItem(new Item{itemType = Item.ItemType.potion,amount = 1});
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        onItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<Item> GetItemList()
    {
        return items;
    }

    public int GetGold()
    {
        return gold;
    }

    public int AddGold(int n)
    {
        gold+=n;
        onGoldChanged?.Invoke(gold);
        return gold;
    }

    public int RemoveGold(int n)
    {
        gold-=n;
        onGoldChanged?.Invoke(gold);
        return gold;
    }
}
