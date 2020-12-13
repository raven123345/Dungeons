using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorkenPieces : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Vector3 moveDir;

    public float deceleration = 5f;

    public float pieceLifeTime = 3f;

    public SpriteRenderer spr;
    public float fadeSpeed = 2.5f;


    void Start()
    {
        moveDir.x = Random.Range(-moveSpeed, moveSpeed);
        moveDir.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDir * Time.deltaTime;

        moveDir = Vector3.Lerp(moveDir, Vector3.zero, deceleration * Time.deltaTime);


        pieceLifeTime -= Time.deltaTime;

        if (pieceLifeTime < 0f)
        {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, Mathf.MoveTowards(spr.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (spr.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
