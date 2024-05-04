using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        yellowOutfit,
        blueOutfit,
        leatherHat,
        wizardHat,
        wood,
        potion
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {

            default:
            case ItemType.yellowOutfit: return ItemAssets.Instace.yellowOutfitSprite;
            case ItemType.blueOutfit: return ItemAssets.Instace.blueOutfitSprite;
            case ItemType.leatherHat: return ItemAssets.Instace.leatherHatSprite;
            case ItemType.wizardHat: return ItemAssets.Instace.wizardHatSprite;
            case ItemType.wood: return ItemAssets.Instace.woodSprite;
            case ItemType.potion: return ItemAssets.Instace.potionSprite;
        }
    }
}
