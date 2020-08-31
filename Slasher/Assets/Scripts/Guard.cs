using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    public FOVDetection fov;
    Transform target;
    public Transform gun;
    public int damage = 10;
    bool shooting;
    public GameObject shot;

    public Transform[] waypoints;
    int currentPoint = 0;
    NavMeshAgent agent;

    public Material[] materials;
    public MeshRenderer cone;

    public Guard[] otherGuards;
    [HideInInspector]
    public Vector3 suspicious;
    bool investigating;
    public float hearingDistance = 10;
    public float detectedBar;
    public enum MovementStates
    {
        Patrol,
        Still,
        Chase,
        Investigate
    }
    public MovementStates state;
    public Material detecionMaterial;
    //WeaponSwitch weaponSwitch;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = fov.player;
        //weaponSwitch = target.GetComponent<WeaponSwitch>();
        detecionMaterial.color = new Color(detecionMaterial.color.r, detecionMaterial.color.g, detecionMaterial.color.b, detectedBar);
    }
    private void Update()
    {
        if (fov.isInFov)
        {
            detectedBar += Time.deltaTime * 2;

            detecionMaterial.color = new Color(detecionMaterial.color.r, detecionMaterial.color.g, detecionMaterial.color.b, detectedBar);
            suspicious = target.position;

            foreach(Guard guard in otherGuards)
            {
                guard.suspicious = target.position;
                if(guard.state == MovementStates.Patrol)
                {
                    guard.state = MovementStates.Investigate;
                }
            }
            if (!shooting && detectedBar >= 1)
            {
                detectedBar = 1;
                StartCoroutine(AimAndShoot());
            }
        }
        else
        {
            if(detectedBar > 0)
            {
                detecionMaterial.color = new Color(detecionMaterial.color.r, detecionMaterial.color.g, detecionMaterial.color.b, detectedBar);
                detectedBar -= Time.deltaTime;
            }
            else
            {
                detectedBar = 0;
            }
            if(Vector3.Distance(transform.position, waypoints[currentPoint].position) < 3)
            {
                currentPoint++;
                if(currentPoint >= waypoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        switch (state)
        {
            case MovementStates.Patrol:
                cone.material = materials[0];
                agent.SetDestination(waypoints[currentPoint].position);
                /*if (Input.GetMouseButtonDown(0) && weaponSwitch.index == 0)
                {
                    if (Vector3.Distance(transform.position, target.position) < hearingDistance)
                    {
                        suspicious = target.position;
                        state = MovementStates.Investigate;
                    }
                }*/
                break;
            case MovementStates.Still:
                cone.material = materials[1];
                agent.SetDestination(transform.position);
                break;
            case MovementStates.Chase:
                cone.material = materials[2];
                agent.SetDestination(target.position);
                break;
            case MovementStates.Investigate:
                cone.material = materials[2];
                agent.SetDestination(suspicious);
                if(Vector3.Distance(transform.position, suspicious) < 3)
                {
                    if(!investigating)
                    StartCoroutine(Investigate());
                }
                break;
        }
    }
    IEnumerator AimAndShoot()
    {
        state = MovementStates.Still;
        shooting = true;
        Vector3 targetPostition = new Vector3(target.position.x,
                           transform.position.y,
                           target.position.z);
        transform.LookAt(targetPostition); //aim at the player
        gun.LookAt(target);
        yield return new WaitForSeconds(0.1f);
        GameObject pistolShot = Instantiate(shot, transform.position, Quaternion.identity);
        Destroy(pistolShot, 3);
        Ray ray = new Ray(gun.position, transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(1);
        shooting = false;
        if (!fov.isInFov)
            StartCoroutine(Chase());
    }
    IEnumerator Chase()
    {
        if (shooting)
            yield return null;
        state = MovementStates.Chase;
        yield return new WaitForSeconds(5);
        if(state == MovementStates.Chase)
        {
            state = MovementStates.Patrol;
        }
    }
    IEnumerator Investigate()
    {
        investigating = true;
        float investigationTime = UnityEngine.Random.Range(0.2f, 5.0f);
        yield return new WaitForSeconds(investigationTime);
        if (state == MovementStates.Investigate)
        {
            investigating = false;
            state = MovementStates.Patrol;
        }
    }
}
