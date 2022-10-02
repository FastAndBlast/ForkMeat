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

    [HideInInspector]
    public float boostDuration = 0f;
    public float boostDurationMax;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();

        Vector3 centerOfMass = rb.centerOfMass;

        rb.centerOfMass = centerOfMass;

        boostDuration = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (boostDuration > 0)
        {
            if (boostDuration < boostDurationMax - 0.15f && rb.velocity.magnitude < 0.25f)
            {
                Forklift.instance.ExplodeBoxes();
            }
            rb.AddForce(transform.forward * boostSpeed * Time.fixedDeltaTime * GameManager.timeScale);
            boostDuration -= Time.deltaTime;
        }
        else
        {
            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            rb.AddForce(transform.forward * vertical * speed * Time.fixedDeltaTime * GameManager.timeScale);
            //rb.MoveRotation(transform.rotation * deltaRotation);

            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //rb.MoveRotation(transform.rotation * deltaRotation);

            // Physics based
            if (vertical < 0)
            {
                Quaternion deltaRotation = Quaternion.Euler(Vector3.up * -horizontal * turnSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(transform.rotation * deltaRotation);
            }
            else if (vertical > 0)
            {
                //rb.AddTorque(transform.up * horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
                //rb.AddTorque(transform.right * horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
                Quaternion deltaRotation = Quaternion.Euler(Vector3.up * horizontal * turnSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(transform.rotation * deltaRotation);
            }
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
                //rb.AddForce(transform.forward * boostSpeed);
                //ADD DASH
                boostDuration = boostDurationMax;
                boostCooldownTime = boostCooldown;
            }
        }
    }

    public void UpgradeBoxMax()
    {
        if (maxBoxes == 2)
        {
            maxBoxes = 4;
            transform.Find("Forklift").Find("Stem2").gameObject.SetActive(true);
        }
        else if (maxBoxes == 4)
        {
            maxBoxes = 6;
            transform.Find("Forklift").Find("Stem2").Find("Stem3").gameObject.SetActive(true);
        }
    }


}
