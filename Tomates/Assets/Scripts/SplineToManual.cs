using UnityEngine;
using UnityEngine.InputSystem;

public class SplineToManual : MonoBehaviour
{
    public MonoBehaviour splineAnimate;
    public float moveSpeed = 5f;

    private bool manualControl = false;
    private Rigidbody rb;
    private Camera mainCamera;

    private InputAction switchAction;
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        mainCamera = Camera.main;

        switchAction = new InputAction("SwitchControl", binding: "<Keyboard>/space");
        switchAction.performed += ctx => EnableManualControl();
        switchAction.Enable();

        moveAction = new InputAction("Move", binding: "<Keyboard>/w");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.Enable();
    }

    void Update()
    {
        if (manualControl)
        {
            MoveWithWASD();
        }
    }

    void EnableManualControl()
    {
        manualControl = true;
        if (splineAnimate != null)
            splineAnimate.enabled = false;
    }

    void MoveWithWASD()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        float rotationInput = input.x;
        float rotationSpeed = 120f;

        if (rotationInput != 0)
        {
            transform.Rotate(0f, rotationInput * rotationSpeed * Time.deltaTime, 0f);
        }

        float forwardInput = input.y;

        if (forwardInput != 0)
        {
            Vector3 moveDir = transform.forward * forwardInput * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveDir);
        }
    }

    private void OnDestroy()
    {
        switchAction.Disable();
        moveAction.Disable();
    }
}