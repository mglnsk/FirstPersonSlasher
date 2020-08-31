using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float startDelay = 1;
    float delay;
    Animator anim;
    public int damage = 40;
    public Transform check;
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetTrigger("attack");
                delay = startDelay;
                StartCoroutine(WaitWithDamage());
            }
        }
    }
    IEnumerator WaitWithDamage()
    {
        yield return new WaitForSeconds(0.4f);
        Collider[] colliders = Physics.OverlapSphere(check.position, range);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
}
