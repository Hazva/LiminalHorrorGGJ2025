using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = SC_FPSController.Instance.transform.position;
    }
}
