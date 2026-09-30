using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayLocalVid : MonoBehaviour
{
	VideoPlayer player;
	[SerializeField] string vidname;
	[SerializeField] bool temp = false;
	[SerializeField] float holdToSkipTime = 1f;
	float holdTimer = 0;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		player = GetComponent<VideoPlayer>();

		// Ensure Source is set to URL programmatically
		player.source = VideoSource.Url;

		// Safely combine the streaming assets root path with the file name
		string fullPath = Path.Combine(Application.streamingAssetsPath, vidname);

		player.url = fullPath;
		player.Play();
		player.loopPointReached += OnVideoFinished;
	}

	// Update is called once per frame
	void Update()
	{
		if (temp) return;

		// Skip with Esc/Enter, or by holding a touch or the mouse button (for mobile)
		Keyboard keyboard = Keyboard.current;
		bool keyPressed = keyboard != null && (keyboard.escapeKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame);
		bool held = Pointer.current != null && Pointer.current.press.isPressed;
		holdTimer = held ? holdTimer + Time.deltaTime : 0;

		if (keyPressed || holdTimer >= holdToSkipTime)
		{
			enabled = false;
			player.Stop();
			SceneManager.LoadScene(1);
		}
	}
	void OnVideoFinished(VideoPlayer player)
	{
		if (temp) return;
		SceneManager.LoadScene(1);
	}

	private void OnDestroy()
	{
		player.loopPointReached -= OnVideoFinished;

	}
}
