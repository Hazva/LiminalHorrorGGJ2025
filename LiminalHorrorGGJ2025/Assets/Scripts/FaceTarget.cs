using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    void Update()
    {
        Vector3 targetPosition = SC_FPSController.Instance.transform.position;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            transform.rotation = targetRotation;
        }
    }
}