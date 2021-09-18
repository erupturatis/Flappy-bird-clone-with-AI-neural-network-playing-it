using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float force;
    [SerializeField] float forwardSpeed;
    GenesManager GM;
    public int Action;
    int playerNumber;
    void Start()
    {
        GM = GameObject.FindObjectOfType<GenesManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    float min(float a, float b)
    {
        if (a < b)
        {
            return a;
        }
        return b;
    }
    float abs(float y)
    {
        if (y < 0){
            return -y;
        }
        return y;
    }


    void AdjustRotation()
    {
        float y = rb.velocity.y;
        Vector3 rotation = new Vector3(0f, 0f, 90 * min((abs(y) / 10), 0.75f));
        if (y < 0)
        {
            rotation *= -1;
        }

        gameObject.transform.eulerAngles = rotation;
    }
    

    void Update()
    {
        Action = GM.Actions[playerNumber];
        Vector3 forward = new Vector3(1f, 0f, 0f);
        transform.position += forward * forwardSpeed * Time.deltaTime;
        if (Action == 1)
        {
            Vector2 dir = new Vector2(0, 1);
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * force, ForceMode2D.Impulse);

        }
        AdjustRotation();
        if (gameObject.transform.position.y < -50)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

}
