using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayLocalVid : MonoBehaviour
{
	VideoPlayer player;
	[SerializeField] string vidname;
	[SerializeField] bool temp = false;
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
