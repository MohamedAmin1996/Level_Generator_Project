using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRooms : MonoBehaviour
{
    public LayerMask whatIsRoom;
    public LevelGeneration levelgen;
    private void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (roomDetection == null && levelgen.stopGeneration == true)
        {
            //Spawn random room
            int rand = Random.Range(0, levelgen.rooms.Length);
            Instantiate(levelgen.rooms[rand], transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
