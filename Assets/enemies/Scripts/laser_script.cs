using UnityEngine;

public class laser_script : MonoBehaviour
{

    private float timetillspawn;
    public float starttimetillspawn;

    public GameObject laser;
    public Transform wheretospawn;


    private void Update()
    {
        if (timetillspawn <= 0)
        {
            Instantiate(laser, wheretospawn.position, wheretospawn.rotation);
            timetillspawn = starttimetillspawn;
        }
        else
        {
            timetillspawn -= Time.deltaTime;
        }
    }

}
