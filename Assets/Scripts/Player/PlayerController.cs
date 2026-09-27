using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [Header("Input Actions")]
    InputAction input_A, input_B;
    bool performedJump = false;

    [Header("Movement")]
    [SerializeField] float moveForce = 5f;
    [SerializeField] Vector2 moveDirection = Vector2.right;
    [SerializeField] Vector2 jumpForce = new Vector2(3, 10);
    [SerializeField] Vector2 jumpDirection = Vector2.up;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    enum MovementState
    {
        Free,
        Frozen,
        StuckInSaucer
    }
    MovementState currentMovementState = MovementState.Free;

    [Header("Ground Detection")]
    [SerializeField] bool grounded;
    [SerializeField] float groundDetectionLength = 1.0f;
    [SerializeField] LayerMask groundMask;
   

    [Header("Costs")]
    [SerializeField] int walkCost = 1;
    [SerializeField] int baseJumpCost = 5;
    int currentJumpCost;
    [SerializeField] int currentJumpCount = 0; // Amount of jumps the player has performed before landing
    [SerializeField] float walkCostInterval = 0.25f;
    [SerializeField] float damageInvulnerabilityPeriod = 0.5f;
    float intervalProgress = 0;
    float invulnerabilityProgress = 0;
    bool invulnerable = false;

	Rigidbody2D rb;
    Animator anim;

    [Header("SFX")]
    [SerializeField] AudioManager.SFXClip jumpSFX;

    [Header("For Monitoring")]
    [SerializeField] Vector2 linearVelocity;
    [SerializeField] float angularVelocity;

	private void Awake()
	{
        if (Instance != null) Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        input_A = InputSystem.actions.FindAction("A");
        input_B = InputSystem.actions.FindAction("B");
        
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        currentJumpCost = baseJumpCost;
	}

    // Update is called once per frame
    void Update()
    {
        if (input_A.WasPressedThisFrame()) performedJump = true;

        if (input_A.WasReleasedThisFrame() && rb.linearVelocity.y > 0) rb.linearVelocityY *= jumpCutMultiplier; 

	}

	private void FixedUpdate()
	{
        switch(currentMovementState)
        {
            case MovementState.Free:
				CheckForGround();
				PerformMovement();
				HandleJump();
				HandleInvulnerability();
				break;
            case MovementState.Frozen:
                break;
            case MovementState.StuckInSaucer:
                break;
        }
 

        

        linearVelocity = rb.linearVelocity;
        angularVelocity = rb.angularVelocity;

        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -10f, 10f);
	}

    void PerformMovement()
    {
		if (grounded)
		{
            
			rb.AddForce(moveDirection * moveForce, ForceMode2D.Force);

            // TODO: Align with animation instead of an interval
            intervalProgress += Time.fixedDeltaTime;
            if (intervalProgress >= walkCostInterval)
            {
				MoneyManager.Instance.SubtractMoney(walkCost);
                intervalProgress = 0;
			}
            
		}
        else rb.AddForce(moveDirection * moveForce / 5, ForceMode2D.Force); // Slight force to get past obstacles

	}

    void HandleJump()
    {
        if (performedJump)
        {
            if (!grounded) rb.linearVelocityY = 0;
			rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
			currentJumpCount++;
            performedJump = false;

            currentJumpCost *= 2;
            MoneyManager.Instance.SubtractMoney(currentJumpCost);

            AudioManager.Instance.PlaySFX(jumpSFX);
		}

        
    }

    void CheckForGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, groundDetectionLength, groundMask))
        {
            // Player is touching the ground
            if (!grounded)
            {
                currentJumpCount = 0;
                currentJumpCost = baseJumpCost;
            }

            grounded = true;
            anim.SetBool("IsGrounded", true);
            
        }
        else
        {
            // Player is not touching the ground
            grounded = false;
            anim.SetBool("IsGrounded", false);
            intervalProgress = 0;

        }
    }

    void HandleInvulnerability()
    {
        if (!invulnerable) return;

        invulnerabilityProgress += Time.fixedDeltaTime;

        if (invulnerabilityProgress >= damageInvulnerabilityPeriod)
        {
            invulnerable = false;
            invulnerabilityProgress = 0;
        }
    }


    public void ResetPlayer(Vector2 startPosition)
    {
		rb.linearVelocity = Vector2.zero;
		rb.angularVelocity = 0f;
        intervalProgress = 0;
        invulnerabilityProgress = 0;
        currentJumpCount = 0;
        currentJumpCost = baseJumpCost;
        invulnerable = false;

        transform.position = startPosition;
	}

	#region Damage Methods
    public void DamageWithForce(float force, Transform deliver, int cost)
    {
        if (invulnerable) return;

        Vector2 direction = transform.position - deliver.position;

		// If player is in the air, invert x direction to keep the player moving forward
		if (direction.y > 0.6f) direction.x *= -1f;
        else
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        rb.AddForce(force * direction, ForceMode2D.Impulse);
        MoneyManager.Instance.SubtractMoney(cost);
        invulnerable = true;
    }

    public void ApplyForce(float force, Transform deliverer)
    {
		Vector2 direction = transform.position - deliverer.position;
		rb.linearVelocity = Vector2.zero;
		rb.angularVelocity = 0f;
		rb.AddForce(force * direction, ForceMode2D.Impulse);
	}

    public void CaughtBySaucer()
    {
        currentMovementState = MovementState.StuckInSaucer;
		rb.linearVelocity = Vector2.zero;
		rb.angularVelocity = 0f;
	}

    public void FreedFromSaucer()
    {
        currentMovementState = MovementState.Free;
    }
	#endregion

	private void OnDrawGizmosSelected()
	{
        Gizmos.DrawSphere(transform.position + new Vector3(0, -groundDetectionLength), 0.2f);
	}
}
