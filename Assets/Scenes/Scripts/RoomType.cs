using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int type;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }

    private void Start()
    {
        Debug.Log($"{name}: {GetComponent<RoomType>().type}");
    }

}
