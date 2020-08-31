
using UnityEngine;

public class BodyCollapse : MonoBehaviour
{
    public Rigidbody bodie;
    // Start is called before the first frame update
    void Start()
    {
        bodie.AddForce(transform.right * -5, ForceMode.Impulse);
    }
}
