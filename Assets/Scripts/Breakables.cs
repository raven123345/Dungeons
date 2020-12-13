using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{

    public GameObject[] brokenPeaces;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Smash()
    {
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(1);

        //show broken pieces
        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPeaces.Length);
            Instantiate(brokenPeaces[randomPiece], transform.position, transform.rotation);
        }

        //drop items
        if (shouldDropItem)
        {
            float dropChanse = Random.Range(0f, 100f);

            if (dropChanse < itemDropPercent)
            {
                int i = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[i], transform.position, transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0f)
            {
                Smash();
            }
        }
        if (other.tag == "PlayerBullet")
        {
            Smash();
        }
    }
}

