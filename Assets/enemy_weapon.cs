using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class enemy_weapon : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject player;
    public Transform pivot;
    public Transform firePoint;
    public float velocidadRotacion = 5f;

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

        Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, angle + 180f);

        pivot.rotation = Quaternion.Lerp(pivot.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
    }


}
