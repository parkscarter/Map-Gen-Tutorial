using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public Transform[] startingPositions;   //Top Row of level
    public GameObject[] rooms;  // 0 -> LR, 1 -> LRB, 2 -> LRT, 3 -> LRBT

    private int direction;
    //1 or 2 indicate right, 3 or 4 indicate left, 5 indicates down
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.5f;

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;

    public LayerMask room;

    private int downCounter;

    // Start is called before the first frame update
    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) //Move right
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);


                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 2;
                }
                if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) //Move Left
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 5) //Move Down
        {
            downCounter++;
            if (transform.position.y > minY)
            {
                //Detect what type of room this is
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                //if it doesn't have a bottom opening
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    // If we go down twice in a row, must spawn room with all 4 openings
                    if (downCounter >= 2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        //destroy room
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        //Create a new room with a bottom opening
                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                    
                }

                //move down and create room with top opening
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                
            }
            else
            {
                stopGeneration = true;
            }

            
        }
    }

    // Update is called once per frame
    void Update()
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

    }
}
