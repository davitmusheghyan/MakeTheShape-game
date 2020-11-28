using UnityEngine;
using DG.Tweening;

public class Level : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioManager audioManager;

    [SerializeField] GameObject wallEffect;
    public GameObject wall;
    public bool playerHit;
    [SerializeField] bool callOneTime;
    bool windEffect;
    [SerializeField] float moveSpeed;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        moveSpeed = LevelManager.levelMoveSpeed;
        callOneTime = true;
    }

    void Update()
    {
        if(!gameManager.pause)
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        if(transform.position.x >= -14 && !windEffect)
        {
            audioManager.NewLevel();
            windEffect = true;
        }
        if (transform.position.x >= 0)
        {
            if (callOneTime)
            {
                if (!gameManager.gameOver)
                {
                    gameManager.newLevel = true;

                    GameObject effect = Instantiate(wallEffect, new Vector3(0, 2.5f), Quaternion.identity);
                    effect.transform.DOScale(new Vector3(1.15f, 6.85f, 10.3f), 0.55f);
                    effect.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 1, 1, 0), 0.55f);
                    Destroy(effect, 0.5f);

                    if(LevelManager.levelMoveSpeed < 60)
                        LevelManager.levelMoveSpeed += 0.5f;
                }

                Destroy(wall);
                callOneTime = false;
            }
        }

        if (transform.position.x >= 15)
            Destroy(gameObject);
    }
}
