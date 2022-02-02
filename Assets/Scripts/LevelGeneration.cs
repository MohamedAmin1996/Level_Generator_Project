using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;
    /*
     * index 0 --> LR 
     * index 1 --> LRB 
     * index 2 --> LRT 
     * index 3 --> LRBT 
     * 
     * index 4 --> RB
     * index 5 --> LB 
     * index 6 --> RT
     * index 7 --> LT 
     * index 8 --> LTB
     * index 9 --> RTB
     * index 10 --> TB
    */

    private int direction; 
    /*
     * 1 && 2 = Right
     * 3 && 4 = Left
     * 5 = Down
     */

    public float moveAmount;
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public bool stopGeneration;

    public LayerMask room;

    public int downCounter;

    public List<GameObject> allSpawnedRooms;
    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    public GameObject player;


    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;

        if (transform.position == startingPositions[0].position) // Top left Position
        {
            Instantiate(rooms[4], transform.position, Quaternion.identity); // RB
        }
        else if (transform.position == startingPositions[1].position || transform.position == startingPositions[2].position) // Top middle positions
        {
            Instantiate(rooms[1], transform.position, Quaternion.identity); // LRB
        }
        else if (transform.position == startingPositions[3].position) // Top right Position
        {
            Instantiate(rooms[5], transform.position, Quaternion.identity); // LB
        }

        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }

        SpawnPlayerAndBossRoom();
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) // Move to the right
        {
            if (transform.position.x < maxX)
            {
                if(transform.position.y == maxY) // If at the top spawn points
                {
                    if (transform.position.x == maxX - 10) // Top middle right spawn point
                    {
                        downCounter = 0;
                        Vector2 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y);
                        transform.position = newPos;

                        Instantiate(rooms[5], transform.position, Quaternion.identity); // LB 

                        direction = Random.Range(1, 6);
                        if (direction == 3) // If left
                        {
                            direction = 2; // Move right instead
                        }
                        else if (direction == 4) // If left
                        {
                            direction = 5; // Move Down instead
                        }
                    }
                    else
                    {
                        GameObject[] nonTopRooms = { rooms[0], rooms[1] }; // All rooms with no top opening
                        downCounter = 0;
                        Vector2 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y);
                        transform.position = newPos;

                        int randTop = Random.Range(0, nonTopRooms.Length);
                        Instantiate(nonTopRooms[randTop], transform.position, Quaternion.identity);

                        direction = Random.Range(1, 6);
                        
                        if (direction == 3) // If left
                        {
                            direction = 2; // Move right
                        }
                        else if (direction == 4) // If left
                        {
                            direction = 5; // Move Down
                        }
                    }
                }
                else
                {
                    Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                    if (roomDetection.GetComponent<RoomType>().type != 0 &&
                        roomDetection.GetComponent<RoomType>().type != 1 &&
                        roomDetection.GetComponent<RoomType>().type != 2 &&
                        roomDetection.GetComponent<RoomType>().type != 3 &&
                        roomDetection.GetComponent<RoomType>().type != 4 &&
                        roomDetection.GetComponent<RoomType>().type != 6 &&
                        roomDetection.GetComponent<RoomType>().type != 9) // Rooms with right openings will be ignored
                    {
                        if (transform.position.x == minX)
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[9], transform.position, Quaternion.identity); // RTB
                        }
                        else
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[3], transform.position, Quaternion.identity); // LRTB
                        }
                    }

                    downCounter = 0;
                    Vector2 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y);
                    transform.position = newPos;

                    if (transform.position.y == minY)
                    {
                        if (transform.position.x == maxX)
                        {
                            Instantiate(rooms[7], transform.position, Quaternion.identity); // LT
                        }
                        else
                        {
                            Instantiate(rooms[0], transform.position, Quaternion.identity); // LR
                        }
                    }
                    else
                    {
                        if (transform.position.x == maxX)
                        {
                            Instantiate(rooms[8], transform.position, Quaternion.identity); // LTB
                        }
                        else
                        {
                            GameObject[] leftRooms = { rooms[0], rooms[1], rooms[2], rooms[3] }; // LR, LRB, LRT, LRTB
                            int rand = Random.Range(0, leftRooms.Length);
                            Instantiate(rooms[rand], transform.position, Quaternion.identity);
                        }
                    }

                    direction = Random.Range(1, 6);
                    if (direction == 3) // If left
                    {
                        direction = 2; // Move right
                    }
                    else if (direction == 4) // If left
                    {
                        direction = 5; // Move Down
                    }
                }  
            }
            else
            {
                direction = 5; // Move Down
            }
        }
        
        else if (direction == 3 || direction == 4) // Move to the left
        {
            if (transform.position.x > minX)
            {
                if (transform.position.y == maxY) // If at the top spawn points
                {
                    if (transform.position.x == minX + 10) // Top middle left spawn point
                    {
                        downCounter = 0;
                        Vector2 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y);
                        transform.position = newPos;

                        Instantiate(rooms[4], transform.position, Quaternion.identity); // RB 
                        
                        direction = Random.Range(3, 6); // Will not choose right, only left or down 
                    }
                    else
                    {
                        GameObject[] nonTopRooms = { rooms[0], rooms[1] }; // All rooms with no top opening
                        downCounter = 0;
                        Vector2 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y);
                        transform.position = newPos;

                        int randTop = Random.Range(0, nonTopRooms.Length);
                        Instantiate(rooms[randTop], transform.position, Quaternion.identity); 

                        direction = Random.Range(3, 6); // Will not choose right, only left or down 
                    }
                }
                else
                {
                    Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                    if (roomDetection.GetComponent<RoomType>().type != 0 &&
                        roomDetection.GetComponent<RoomType>().type != 1 &&
                        roomDetection.GetComponent<RoomType>().type != 2 &&
                        roomDetection.GetComponent<RoomType>().type != 3 &&
                        roomDetection.GetComponent<RoomType>().type != 5 &&
                        roomDetection.GetComponent<RoomType>().type != 7 &&
                        roomDetection.GetComponent<RoomType>().type != 8) // Rooms with left openings will be ignored
                    {
                        if (transform.position.x == maxX)
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[8], transform.position, Quaternion.identity); // LTB
                        }
                        else
                        {
                            roomDetection.GetComponent<RoomType>().RoomDestruction();
                            Instantiate(rooms[3], transform.position, Quaternion.identity); // LRTB
                        }
                    }

                    downCounter = 0;
                    Vector2 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y);
                    transform.position = newPos;

                    if (transform.position.y == minY)
                    {
                        if (transform.position.x == minX)
                        {
                            Instantiate(rooms[6], transform.position, Quaternion.identity); // RT
                        }
                        else
                        {
                            Instantiate(rooms[0], transform.position, Quaternion.identity); // LR
                        }
                    }
                    else
                    {
                        if (transform.position.x == minX)
                        {
                            Instantiate(rooms[9], transform.position, Quaternion.identity); // RTB
                        }
                        else 
                        {
                            GameObject[] rightRooms = { rooms[0], rooms[1], rooms[2], rooms[3] }; 
                            int rand = Random.Range(0, rightRooms.Length);
                            Instantiate(rooms[rand], transform.position, Quaternion.identity);
                        }
                    }

                    direction = Random.Range(3, 6); // Will not choose right, only left or down 
                }
            }
            else
            {
                direction = 5; // Move Down
            }
        }

        else if (direction == 5) // Move downward
        {
            downCounter++;

            if (transform.position.y > minY)
            {
                // Rooms with bottom opening will be ignored
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if (roomDetection.GetComponent<RoomType>().type != 1 && 
                    roomDetection.GetComponent<RoomType>().type != 3 &&
                    roomDetection.GetComponent<RoomType>().type != 4 &&
                    roomDetection.GetComponent<RoomType>().type != 5 &&
                    roomDetection.GetComponent<RoomType>().type != 8 &&
                    roomDetection.GetComponent<RoomType>().type != 9 &&
                    roomDetection.GetComponent<RoomType>().type != 10)
                {
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity); // LRTB
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        if (transform.position.y == maxY)
                        {
                            Instantiate(rooms[1], transform.position, Quaternion.identity); // LRB
                        }
                        else if (transform.position.y == minY)
                        {
                            Instantiate(rooms[2], transform.position, Quaternion.identity); // LRT
                        }
                        else
                        {
                            int randBottomRoom = Random.Range(1, 4); // LR, LRB, LRT are possible, not LRTB
                            
                            if (randBottomRoom == 2)
                            {
                                randBottomRoom = 1;
                            }
                            
                            Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                        }


                    }
                }

                Vector2 newPos = new Vector3(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                if (transform.position.x == maxX)
                {
                    if (transform.position.y == minY)
                    {
                        Instantiate(rooms[7], transform.position, Quaternion.identity); // LT
                    }
                    else
                    {
                        Instantiate(rooms[10], transform.position, Quaternion.identity); // TB
                    }
                }
                else if (transform.position.x == minX)
                {
                    if (transform.position.y == minY)
                    {
                        Instantiate(rooms[6], transform.position, Quaternion.identity); // RT
                    }
                    else
                    {
                        Instantiate(rooms[10], transform.position, Quaternion.identity); // TB
                    }
                }
                else if (transform.position.y == minY)
                {
                    Instantiate(rooms[2], transform.position, Quaternion.identity); // LRT
                }
                else
                {
                    Instantiate(rooms[3], transform.position, Quaternion.identity); // LRTB
                }

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true; // Stop the level generation
            }
        }
    }

    void SpawnPlayerAndBossRoom()
    {
        for (int i = 0; i < allSpawnedRooms.Count; i++)
        {
            if (allSpawnedRooms[i] == null)
            {
                allSpawnedRooms.RemoveAt(i);
            }
        }
        
        if (waitTime <= 0 && spawnedBoss == false) // WaitTime is how long to wait until all rooms have been generated
        {
            Instantiate(boss, allSpawnedRooms[allSpawnedRooms.Count - 1].transform.position, Quaternion.identity);
            Instantiate(player, allSpawnedRooms[0].transform.position, Quaternion.identity);
            spawnedBoss = true;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
