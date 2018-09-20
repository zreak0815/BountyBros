using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGroundScroller : MonoBehaviour
{

    public float scrollSpeed;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        playerPos = new Vector3(playerPos.x, 0, 0);
        
        transform.position = startPosition - (scrollSpeed * playerPos / 10);
    }
}
