using UnityEngine;

public class destroyer : MonoBehaviour
{

    public float timeTillDestroy;

    void Update()
    {
        Destroy (gameObject, timeTillDestroy);
    }

}
