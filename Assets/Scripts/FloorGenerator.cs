using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] GameObject floor;
    [SerializeField] float distance = 75f;

    List<GameObject> clones = new List<GameObject>();
    private bool isDeletingFloors = false;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDeletingFloors)
        {
            GameObject clone = Instantiate(floor);
            clone.transform.position = new Vector3(clone.transform.position.x + distance, clone.transform.position.y, clone.transform.position.z);
            clones.Add(clone);
        }
    }

    public void DeleteFloors()
    {
        isDeletingFloors = true;

        foreach (GameObject clone in clones)
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }
        clones.Clear();

        isDeletingFloors = false;
    }
}
