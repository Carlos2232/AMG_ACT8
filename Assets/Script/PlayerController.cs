using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AABBCollider))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -15f;

    private AABBCollider boxCollider;
    private float verticalVelocity;
    private bool isGrounded;

    private void Awake()
    {
        boxCollider = GetComponent<AABBCollider>();
    }

    private void Update()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();
    }

    private void HandleHorizontalMovement()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveInput = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveInput = 1f;

        transform.position += new Vector3(moveInput * moveSpeed * Time.deltaTime, 0f, 0f);
    }

    private void HandleVerticalMovement()
    {
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }

        transform.position += new Vector3(0f, verticalVelocity * Time.deltaTime, 0f);
        isGrounded = false;
    }

    public void HandleCollision(AABBCollider other)
    {
        switch (other.type)
        {
            case ColliderType.Ground:
                if (verticalVelocity <= 0f)
                {
                    float playerHeight = boxCollider.max.y - boxCollider.min.y;
                    transform.position = new Vector3(transform.position.x, other.max.y + playerHeight * 0.5f, transform.position.z);
                    verticalVelocity = 0f;
                    isGrounded = true;
                }
                break;

            case ColliderType.Obstacle:
                float playerWidth = boxCollider.max.x - boxCollider.min.x;
                if (transform.position.x < other.transform.position.x)
                {
                    transform.position = new Vector3(other.min.x - playerWidth * 0.5f, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(other.max.x + playerWidth * 0.5f, transform.position.y, transform.position.z);
                }
                break;

            case ColliderType.FallCollider:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }
}