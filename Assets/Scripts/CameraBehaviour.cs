using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    Transform player;
    LevelGeneration levelgen;

    private void Start()
    {
        levelgen = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<LevelGeneration>();
    }


    private void LateUpdate()
    {
        if (levelgen.waitTime + 0.5f <= 0) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 newPos = new Vector3(player.position.x, player.position.y, -10f);
            transform.position = newPos;
        }
    }
}
