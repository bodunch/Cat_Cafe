using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform _camera;
    public LayerMask groundMask;

    public float mouseSensitivity = 50f;
    public float jumpHeight = 3f;

    public float horizontal;
    public float vertical;

    public float speed = 6f;

    public float minLookAngle = -60f;
    public float maxLookAngle = 60f;

    private float mouseX;
    private float mouseY;

    private float xRotation = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && CheckIsGrounded())
        {
            Jump();
        }

        if(_rb.velocity.y < 0.734)
        {
            _rb.velocity += Vector3.up * Physics.gravity.y * 5f * Time.fixedDeltaTime;
        }

        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        _rb.velocity = new Vector3(move.x * speed, _rb.velocity.y, move.z * speed);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);
        _camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private bool CheckIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
    }

    private void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(3f * -Physics.gravity.y * jumpHeight);
        _rb.velocity = new Vector3(_rb.velocity.x, jumpVelocity, _rb.velocity.z);
    }
}
