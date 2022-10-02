using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;

    public Rigidbody rb;

    public int maxBoxes = 2;

    public float boxThrowStrength = 50f;

    [Header("Boost")]
    public bool boostEnabled;
    public float boostSpeed;
    public float boostCooldown;
    float boostCooldownTime;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.AddForce(transform.forward * vertical * speed * Time.fixedDeltaTime * GameManager.timeScale);

        //print(Time.fixedDeltaTime);
        //print(Time.deltaTime);

        //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * horizontal * turnSpeed * Time.fixedDeltaTime);

        //rb.MoveRotation(transform.rotation * deltaRotation);

        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        // Physics based
        if (vertical != 0)
        {
            rb.AddTorque(transform.up * horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
        }
    }

    private void Update()
    {
        if (boostEnabled)
        {
            if (boostCooldownTime > 0)
            {
                boostCooldownTime -= Time.deltaTime;
            }
            else if (Input.GetButtonDown("Boost"))
            {
                rb.AddForce(transform.forward * boostSpeed);
                boostCooldownTime = boostCooldown;
            }
        }
    }




}
