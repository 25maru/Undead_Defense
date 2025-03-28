using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}
