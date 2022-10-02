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

    public float downForce = 1000f;

    [Header("Boost")]
    public bool boostEnabled;
    public float boostSpeed;
    public float boostCooldown;
    float boostCooldownTime;
    float revUpCooldown;
    float revDownCooldown;
    float idleCooldown;
    float revCooldown;

    [HideInInspector]
    public float boostDuration = 0f;
    public float boostDurationMax;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
		    revDownCooldown = 0.75f;

        Vector3 centerOfMass = rb.centerOfMass;

        rb.centerOfMass = centerOfMass;

        boostDuration = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForceAtPosition(Vector3.down * downForce, rb.worldCenterOfMass);

        //Vector3 vel = rb.velocity;
        //rb.velocity = Vector3.zero;
        
        //rb.velocity = vel;

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

            Vector3 forwardVector = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

            rb.AddForce(forwardVector * vertical * speed * Time.fixedDeltaTime * GameManager.timeScale);
            //rb.MoveRotation(transform.rotation * deltaRotation);

            //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            //rb.MoveRotation(transform.rotation * deltaRotation);

            // Physics based
            if (vertical < 0)
            {
                //rb.AddTorque(transform.up * -horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
                //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * -horizontal * turnSpeed * Time.fixedDeltaTime);
                //rb.MoveRotation(transform.rotation * deltaRotation);
                transform.eulerAngles += Vector3.up * -horizontal;

            }
            else if (vertical > 0)
            {
                //rb.AddTorque(transform.up * horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
                //rb.AddTorque(transform.right * horizontal * turnSpeed * Time.deltaTime * GameManager.timeScale);
                //Quaternion deltaRotation = Quaternion.Euler(Vector3.up * horizontal * turnSpeed * Time.fixedDeltaTime);
                //rb.MoveRotation(transform.rotation * deltaRotation);
                transform.eulerAngles += Vector3.up * horizontal;
            }
        }

        //rb.constraints = RigidbodyConstraints.None;
        //rb.MoveRotation(Quaternion.Euler(0, transform.eulerAngles.y, 0));
        //rb.constraints = (int)RigidbodyConstraints.FreezeRotationX + RigidbodyConstraints.FreezeRotationZ;
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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

      if (Input.GetButtonDown("Vertical")) {
        AudioManager.instance.Stop("EngineIdle");
        AudioManager.instance.Stop("EngineTransDown");
        AudioManager.instance.Play("EngineTransUp", 0.2f);
        revUpCooldown = 2.5f;
        revDownCooldown = 0f;
        revCooldown = 0f;
        idleCooldown = 0f;
      }
      if (Input.GetButtonUp("Vertical")) {
        AudioManager.instance.Stop("EngineRev");
        AudioManager.instance.Stop("EngineTransUp");
        AudioManager.instance.Play("EngineTransDown", 0.2f);
        revDownCooldown = 2.95f;
        revUpCooldown = 0f;
        revCooldown = 0f;
        idleCooldown = 0f;
      }


      if (revDownCooldown > 0)
      {
        revDownCooldown -= Time.deltaTime;

        if (revDownCooldown <= 0) 
        {
          AudioManager.instance.Play("EngineIdle", 0.2f);
          idleCooldown = 6.5f;
        }
      }
      if (revUpCooldown > 0)
      {
        revUpCooldown -= Time.deltaTime;
        if (revUpCooldown <= 0) 
        {
          AudioManager.instance.Play("EngineRev", 0.2f);
          revCooldown = 3.5f;
        }
      }
      if (revCooldown > 0)
      {
        revCooldown -= Time.deltaTime;
        if (revCooldown <= 0) 
        {
          AudioManager.instance.Play("EngineRev", 0.2f);
          revCooldown = 3.5f;
        }
      }
      if (idleCooldown > 0)
      {
        idleCooldown -= Time.deltaTime;
        if (idleCooldown <= 0) 
        {
          AudioManager.instance.Play("EngineIdle", 0.2f);
          idleCooldown = 6.5f;
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
