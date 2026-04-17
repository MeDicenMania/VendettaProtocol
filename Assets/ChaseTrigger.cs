using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    public flying_enemy[] enemyarray;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 

            foreach (flying_enemy enemy in enemyarray)
            {
                enemy.chase = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (flying_enemy enemy in enemyarray)
            {
                enemy.chase = false;
            }
        }
    }

}
