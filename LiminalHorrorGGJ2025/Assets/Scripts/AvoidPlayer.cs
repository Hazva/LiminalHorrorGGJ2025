using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidPlayer : MonoBehaviour
{
    public GameObject[] pathNodes;
    public GameObject playerObject;
    public float distanceThreshold = 5f;
    public float speed = 1.0f;
    private GameObject _currTargetNode;
    private int _currTargetIndex = 0;
    private Vector3 _monsterDirection;

    void Start()
    {
        if (pathNodes != null && pathNodes.Length > 0)
        {
            UpdateCurrTargetNode(_currTargetIndex);
        }
    }

    void Update()
    {
        if (_currTargetIndex < pathNodes.Length)
        {
            Vector3 playerXZ = new Vector3(playerObject.transform.position.x, 0, playerObject.transform.position.z);
            Vector3 monsterXZ = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            Vector3 playerToMonster = monsterXZ - playerXZ;

            float distance = playerToMonster.magnitude;

            if (distance < distanceThreshold)
            {
                _monsterDirection = (_currTargetNode.transform.position - gameObject.transform.position).normalized;
                gameObject.transform.position += _monsterDirection * speed * Time.deltaTime;

                RotateToFace(_currTargetNode.transform.position);
            }

            if (_currTargetNode != null && Vector3.Distance(gameObject.transform.position, _currTargetNode.transform.position) < 0.5f)
            {
                _currTargetIndex = _currTargetIndex + 1;
                UpdateCurrTargetNode(_currTargetIndex);
            }
        }
    }

    private void UpdateCurrTargetNode(int currTargetNodeIndex)
    {
        if (currTargetNodeIndex >= pathNodes.Length) { return; }
        _currTargetNode = pathNodes[currTargetNodeIndex];
    }

    private void RotateToFace(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - gameObject.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
