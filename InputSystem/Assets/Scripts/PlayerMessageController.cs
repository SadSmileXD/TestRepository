using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMessageController : MonoBehaviour
{
    Vector2 moveInput;
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log($"{moveInput.x} : {moveInput.y}");
    }
}
