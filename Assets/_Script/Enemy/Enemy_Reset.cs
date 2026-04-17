using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReset : MonoBehaviour
{
    public float fallThreshold = -100f;
    public Vector3 resetPosition = new Vector3(0f, 0f, 0f);

    void Update()
    {
        if (transform.position.y <= fallThreshold)
        {
            Debug.Log("Enemy fell below threshold. Resetting position.");
            transform.position = resetPosition;
        }
    }

}
