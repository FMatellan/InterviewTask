using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
   public static ItemAssets Instace {get; private set;}
   

   private void Awake() {
    Instace = this;
   }

   public Sprite yellowOutfitSprite;
   public Sprite blueOutfitSprite;
   public Sprite leatherHatSprite;
   public Sprite wizardHatSprite;
   public Sprite woodSprite;
   public Sprite potionSprite;

   
}
