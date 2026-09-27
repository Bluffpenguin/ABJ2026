using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] GameObject floor;
    [SerializeField] float distance = 75f;

    List<GameObject> clones = new List<GameObject>();
    bool isDeletingFloors = false;
    int floorNum;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(clones);
        if (collision.CompareTag("Player") && !isDeletingFloors)
        {
            floorNum = floorNum + 1;
            GameObject clone = Instantiate(floor);
            clone.transform.position = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
            GameManager.Instance.AddFloorToList(clone);
        }
    }
}
