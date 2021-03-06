﻿using UnityEngine;
using System.Collections;

public class BallReset : MonoBehaviour
{
    private Vector3 ballInitPosition;

    // Use this for initialization
    void Start()
    {
        ballInitPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            transform.position = ballInitPosition;
        }
    }
}
