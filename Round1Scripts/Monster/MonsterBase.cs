using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 5.0f)]
    public float speedMax = 0.1f;                   //maximum speed of the monster

    [Range(0.01f, 20.0f)]
    public float acce = 10f;                        //accelertion

    [Range(0.01f, 30.0f)]
    public float deathF = 0f;                       //minimum attack volecity

    [Range(0.01f, 30.0f)]
    public float health = 10f;                      //maximum health
    public float curHealth = 10f;                   //current health

    GameManager gameManager;
    Transform enemyTarget;
    Rigidbody rb;

    bool ifAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        enemyTarget = GameObject.Find("XR Rig").transform;
        curHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            Rigidbody rb = collision.collider.gameObject.GetComponent<Rigidbody>();
            Debug.Log(rb.gameObject.name);
            OnKnifeDamage(rb.velocity);
        }
    }

    private void OnKnifeDamage(Vector3 velocity)
    {
        if (velocity.magnitude > deathF)
        {
            Debug.Log(rb.velocity.magnitude);
            Debug.Log(this.name);
            ifAttacked = true;
        }
    }

    private void Move()
    {
        Vector3 posi = transform.position;
        rb.velocity = (enemyTarget.position - posi).normalized * speedMax;

        //Rotation
        transform.LookAt(enemyTarget.position, Vector3.up);
    }

    private void CheckState()
    {
        if (ifAttacked)
        {
            Destroy(this.gameObject);
            gameManager.enemyNow -= 1;
        }
    }
}