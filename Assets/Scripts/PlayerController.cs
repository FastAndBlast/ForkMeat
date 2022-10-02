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
	float revUpCooldown;
	float revDownCooldown;
	float idleCooldown;
	float revCooldown;

    public static PlayerController instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		revDownCooldown = 0.75f;
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
}
