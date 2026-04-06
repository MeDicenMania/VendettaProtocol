using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class enemy_weapon : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject player;
    public Transform pivot;
    public Transform firePoint;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateGun(player.transform.position);
    }


    private void RotateGun(Vector3 lookPoint)
    {

        Vector3 distanceVector = lookPoint - pivot.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        pivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        Debug.Log(lookPoint);
    }


}
