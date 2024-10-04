using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoPlayer : MonoBehaviour
{
    public GameObject bullet;
    public GameObject canon;
    public Transform Tank;

    public float launchSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateBullet();
        }
    }
    void CreateBullet()
    {
        GameObject shell = Instantiate(bullet, canon.transform.position, canon.transform.rotation);
        shell.GetComponent<Rigidbody>().velocity = launchSpeed * Tank.forward;

    }
}
