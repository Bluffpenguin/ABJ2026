using UnityEngine;

public class DisableWEBGL : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer) gameObject.SetActive(false);
    }

    
}
