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

        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        switchAction.Disable();
        moveAction.Disable();
    }
}
