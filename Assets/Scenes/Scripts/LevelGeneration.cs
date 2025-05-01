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
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity);

        direction = Random.Range(1, 5);
        timeBtwRoom = startTimeBtwRoom;
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

                int rand = Random.Range(0, 4);
                if (transform.position.x == maxX)
                {
                    rand = 1;
                }

                Instantiate(rooms[rand], transform.position, Quaternion.identity);
                


                direction = Random.Range(1, 6);
                //Dont allow move left after move right
                if (direction == 3)
                {
                    direction = 2;
                }
                if (direction == 4)
                {
                    direction = 5;
                }
            }
            //If at right limit, go down
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
                //Move left or down
                int rand = Random.Range(0, 4);

                if (transform.position.x == minX)
                {
                    rand = 1;
                }

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
                
                
                //Instantiate(rooms[3], transform.position, Quaternion.identity);
                //if it doesn't have a bottom opening
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    // If we go down twice in a row, must spawn room with all 4 openings
                    if (downCounter >= 2)
                    {
                        int randTB = Random.Range(3, 5);
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[randTB], transform.position, Quaternion.identity);
                        
                    }
                    else
                    {
                        //destroy room
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int[] validTopRooms = { 1, 3 };
                        int randIndex = Random.Range(0, validTopRooms.Length);
                        int chosenRoom = validTopRooms[randIndex];
                        Instantiate(rooms[1], transform.position, Quaternion.identity);
                        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPaused = !isPaused;
        }


        // Only proceed if not paused
        if (isPaused) return;

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
