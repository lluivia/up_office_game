using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;

    private Camera mainCamera;
    private CharacterController characterController;

    private InputAction walkInput;
    private InputAction runInput;

    private float currentSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;
    private float lookAngle = 0f;

    [SerializeField] private float lookSensitivity = 0.1f; // mouse sensini buradan ve unity içinden değiştirebiliriz
    [SerializeField] private float lookAngleLimit = 90f; // dikeyde limit koyduk

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        walkInput = InputSystem.actions.FindAction("Move");
        runInput = InputSystem.actions.FindAction("Sprint");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        Vector2 moveVector = walkInput.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());


        currentSpeed = runInput.IsPressed() ? runSpeed : walkSpeed;
        HandleMovement(moveVector); // oyuncu hareketi için fonksiyonu tanımlama
        HandleLooking(mouseDelta); // kamera kontrolü için fonksiyonu tanımlama
    }

    private void HandleMovement(Vector2 moveVector)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDirection.y;

        Vector2 newSpeed = new Vector2(moveVector.y * currentSpeed, moveVector.x * currentSpeed);

        moveDirection = (forward * newSpeed.x) + (right * newSpeed.y);
        moveDirection.y = oldY;

        characterController.Move(moveDirection * Time.deltaTime);

    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        lookAngle += -mouseDelta.y * lookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -lookAngleLimit, lookAngleLimit);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * lookSensitivity, 0);
    }


}
