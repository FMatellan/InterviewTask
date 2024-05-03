using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float movementSpeed = 1f;
    [SerializeField]
    float collisionOffset = 0.05f;
    [SerializeField]
    ContactFilter2D movementFilter;

    Vector2 movementInput;
    Animator baseAnimator;
    [SerializeField]
    Animator outfitAnimator,hatAnimator;

    Rigidbody2D rb;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public static event Action Interacted;


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
        if(DialogueManager.GetInstance().dialogueIsPlaying) return;

        movementInput = inputValue.Get<Vector2>();
        baseAnimator.SetFloat("Speed", movementInput.sqrMagnitude);
        outfitAnimator.SetFloat("Speed", movementInput.sqrMagnitude);
        hatAnimator.SetFloat("Speed", movementInput.sqrMagnitude);

    }

    void OnInteract(InputValue inputValue)
    {
       Interacted?.Invoke();
    }

    
}
