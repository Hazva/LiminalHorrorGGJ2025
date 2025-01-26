using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position;

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(-direction);
                transform.rotation = targetRotation;
            }
        }
    }
}