using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] ShopManager shopManager;
    [SerializeField] GameManager gameManager;

    public float rotateSpeed;
    [SerializeField] bool coinAdded;
    [SerializeField] GameObject destroyPS;

    bool chechedPlayer;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rotateSpeed = 250;
    }

    void Update()
    {
        if (gameManager.gameOver)
            StartCoroutine(destroyObject());

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    IEnumerator destroyObject()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerCoin") && !coinAdded)
        {
            PlayerHit();
            coinAdded = true;
        }
    }

    void PlayerHit()
    {
        audioManager.Coin();
        shopManager.AddCoin();
        GameObject ps = Instantiate(destroyPS, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        Destroy(ps, 1);
    }
}
