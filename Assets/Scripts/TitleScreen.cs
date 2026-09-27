using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TitleScreen : MonoBehaviour
{
    VideoPlayer vidplayer;

	private void Awake()
	{
		vidplayer = GetComponent<VideoPlayer>();
	}

	private void Start()
	{
		vidplayer.loopPointReached += OnVideoFinished;
	}
	void OnVideoFinished(VideoPlayer player)
	{
		SceneManager.LoadScene(1);
	}

	private void OnDestroy()
	{
		vidplayer.loopPointReached -= OnVideoFinished;
	}
}
