using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public GameObject deadBody;
    public void TakeDamage(int damage)
    {
        if(damage < health)
        {
            health -= damage;
        }
        else
        {
            health = 0;
            Instantiate(deadBody, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
