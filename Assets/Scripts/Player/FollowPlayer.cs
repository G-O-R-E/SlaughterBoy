using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.GetChild(0).position = transform.GetChild(1).position;
    }
}
