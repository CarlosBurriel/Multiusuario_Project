using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;                   //Movement speed
    public float rotationSpeed = 100.0f;  //Turn speed
    public Transform orientacion;
    public Vector3 moveDirection;
    public Rigidbody PlayerRB;

    public GameObject puntacanon;

    

    public int vidasPlayer;               //Lives


   


    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        float VerticalInput = Input.GetAxisRaw("Vertical");
        moveDirection = orientacion.forward * VerticalInput * Time.deltaTime;
        PlayerRB.AddForce(moveDirection.normalized * speed * 2);

        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);



        if (vidasPlayer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Proyectil")
        {
            vidasPlayer -= 1;
        }



    }
    private void OnTriggerEnter(Collider other)
    {

       
    }

}
