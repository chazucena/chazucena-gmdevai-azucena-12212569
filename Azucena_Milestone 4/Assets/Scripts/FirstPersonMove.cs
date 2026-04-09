using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 moveInput;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
