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
    public int playerNumber;
    int possible = 1;
    

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
    void GetNextPylon()
    {
        GameObject[] P = GameObject.FindGameObjectsWithTag("PylonParent");
        float posmn = 20f;
        int indc = 0;
        for (int i = 0; i < P.Length; i++)
        {
            if (P[i].transform.position.x > gameObject.transform.position.x)
            {
                if (P[i].transform.position.x < posmn)
                {
                    indc = i;
                    posmn = P[i].transform.position.x;
                }
            }
        }

        GM.YDifference[playerNumber] = gameObject.transform.position.y - P[indc].transform.position.y;
        GM.DistanceUntilNext[playerNumber] = P[indc].transform.position.x - gameObject.transform.position.x;
        //Debug.Log(GM.YDifference[playerNumber] + "            " + GM.DistanceUntilNext[playerNumber]);

    }

    public void Die()
    {
        GM.SetDeath(playerNumber,  gameObject.transform.position.x);
        Destroy(gameObject);
    }

    void Update()
    {
        GetNextPylon();
        Action = GM.Actions[playerNumber];
        //print(playerNumber + "    " + Action);
        Vector3 forward = new Vector3(1f, 0f, 0f);
        transform.position += forward * forwardSpeed * Time.deltaTime;
        if (Action == 1 && possible == 1)
        {
            StartCoroutine(WaitForJump());
            possible = 0;
            Vector2 dir = new Vector2(0, 1);
            rb.velocity = Vector2.zero;
            rb.AddForce(dir * force, ForceMode2D.Impulse);

        }
        AdjustRotation();
        if (gameObject.transform.position.y < -5)
        {
            Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
        {
            Die();
        }
    }
    IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(0.3f);
        possible = 1;
    }

}
