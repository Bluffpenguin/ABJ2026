using UnityEngine;
using UnityEngine.InputSystem;

public class SaucerBeam : MonoBehaviour
{
    PlayerController player = null;
	InputAction input_A;
	Rigidbody2D playerRigidbody = null;
    [SerializeField] float attractSpeed = 2;
	[SerializeField] float launchSpeed = 5;

	 

	private void Awake()
	{
		input_A = InputSystem.actions.FindAction("A");
	}
	

    // Update is called once per frame
    void Update()
    {
		if (player == null) return;

        if (input_A.WasPressedThisFrame())
		{
			playerRigidbody.gravityScale = 1;
			player.FreedFromSaucer();
			playerRigidbody.AddForce(Vector2.right * launchSpeed, ForceMode2D.Impulse);
			
			
			player = null;
			playerRigidbody = null;
		}
    }

	private void FixedUpdate()
	{
        if (player == null) return;

		Debug.Log("Bring up");
        playerRigidbody.AddForce(Vector2.up * attractSpeed, ForceMode2D.Force);
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (player != null) return;

        if (collision.CompareTag("Player"))
        {
			player = collision.GetComponent<PlayerController>();
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            player.CaughtBySaucer();
			playerRigidbody.gravityScale = 0;
		}
		
	}
}
