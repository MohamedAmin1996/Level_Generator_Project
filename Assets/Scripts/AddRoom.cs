using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private LevelGeneration templates;

    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<LevelGeneration>();
       
        if (templates.stopGeneration == false)
        {
            templates.allSpawnedRooms.Add(this.gameObject);
        }
    }
}
