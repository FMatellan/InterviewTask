using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Variable")]
    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float collisionOffset = 0.05f;
    [SerializeField]
    ContactFilter2D movementFilter;

    Vector2 movementInput;
    Animator baseAnimator;
    [Header("Animators")]
    [SerializeField]
    Animator outfitAnimator,hatAnimator;
    [Header("Inventory")]
    [SerializeField]
    GameObject inventoryObj;
    [SerializeField] GameObject storeObj;

    [Header("Animator Controllers")]
    [SerializeField] AnimatorOverrideController yellowAnimator,blueAnimator,leatherAnimator,wizardAnimator;

    private Inventory inventory;

    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public static event Action Interacted;


    private void Awake() {
        inventory = new Inventory(UseItem);
        inventoryObj.GetComponent<UI_Inventory>().SetInventory(inventory);
        storeObj.GetComponent<StoreManager>().SetInventory(inventory);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseAnimator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool movementSuccess = TryMove(movementInput);

            if (!movementSuccess )
            {
                movementSuccess = TryMove(new Vector2(movementInput.x, 0));
            }
            if (!movementSuccess )
            {
                movementSuccess = TryMove(new Vector2(0, movementInput.y));
            }
        }
    }

    private void UseItem(Item item)
    {
        
        EquipClothes(item);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, movementSpeed * Time.fixedDeltaTime + collisionOffset);

            baseAnimator.SetFloat("Horizontal", direction.x);
            baseAnimator.SetFloat("Vertical", direction.y);
            outfitAnimator.SetFloat("Horizontal", direction.x);
            outfitAnimator.SetFloat("Vertical", direction.y);
            hatAnimator.SetFloat("Horizontal", direction.x);
            hatAnimator.SetFloat("Vertical", direction.y);
            

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime);
        
                return true;
            }
            else
            {
                return false;
            }
                
        }
        else
        return false;

    }

    void OnMove(InputValue inputValue)
    {
        if(DialogueManager.GetInstance().dialogueIsPlaying || inventoryObj.activeSelf ) return;

        movementInput = inputValue.Get<Vector2>();
        baseAnimator.SetFloat("Speed", movementInput.sqrMagnitude);
        outfitAnimator.SetFloat("Speed", movementInput.sqrMagnitude);
        hatAnimator.SetFloat("Speed", movementInput.sqrMagnitude);

    }

    void OnInteract(InputValue inputValue)
    {
       Interacted?.Invoke();
    }

    void OnInventory(InputValue inputValue)
    {
        if(inventoryObj.activeSelf )
        {
            inventoryObj.SetActive(false);
        }
        else
        {
            inventoryObj.SetActive(true);
            inventory.mode = Inventory.Mode.Browsing;
        }
        
    }

    void EquipClothes(Item item)
    {
        switch (item.itemType)
        {
            default: return;
            case Item.ItemType.yellowOutfit: outfitAnimator.runtimeAnimatorController = yellowAnimator;
            inventory.RemoveItem(new Item{itemType = Item.ItemType.yellowOutfit, amount = 1});
            break;
            case Item.ItemType.blueOutfit: outfitAnimator.runtimeAnimatorController = blueAnimator;
            inventory.RemoveItem(new Item{itemType = Item.ItemType.blueOutfit, amount = 1});
            break;
            case Item.ItemType.leatherHat: hatAnimator.runtimeAnimatorController = leatherAnimator;
            inventory.RemoveItem(new Item{itemType = Item.ItemType.leatherHat, amount = 1});
            break;
            case Item.ItemType.wizardHat: hatAnimator.runtimeAnimatorController = wizardAnimator;
            inventory.RemoveItem(new Item{itemType = Item.ItemType.wizardHat, amount = 1});
            break;
        }
    }
    
}
