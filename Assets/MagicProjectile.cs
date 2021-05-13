using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public Camera cam;
    public GameObject projectile;
    public Transform lFirePoint, rFirePoint;
    public float projectileSpeed = 30f;
    public float fireRate = 4f;
    public float arcRange = 1f;
    public float damage = 10f;
   

    private Vector3 destination;
    private bool left;
    private float timeToFire;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1/fireRate;
            ShootProjectile();
        }
    }
    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

       if( Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);
            EnemyAi enemy = hit.transform.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;

        }
        else
        {
            destination = ray.GetPoint(1000);

        }
        if (left)
        {
            left = false;
            InstantiateProjectile(lFirePoint);
        }
        else
        {
            left = true;
            InstantiateProjectile(rFirePoint);
        }
    }

    void InstantiateProjectile(Transform firePoint)
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;

        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 2));
    }

    
}
