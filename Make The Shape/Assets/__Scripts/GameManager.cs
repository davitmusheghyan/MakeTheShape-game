using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] UIManager uiManager;

    [SerializeField] GameObject player;
    [SerializeField] GameObject playerDestroy_FX;

    public bool gameStart;
    public bool newLevel;
    public bool gameOver;
    public bool pause;

    public int gameScore;
    public int hightScore;

    void Start()
    {
        hightScore = PlayerPrefs.GetInt("hightScore");
        uiManager.bestScore_txt.text = hightScore.ToString();
        gameScore = 0;
    }

    void Update()
    {
        if (newLevel && !gameOver)
        {
            gameScore++;
            StartCoroutine(uiManager.newLevel());
            levelManager.SpawnLevel();
            checkNewBest();
            newLevel = false;
        }
    }

    public IEnumerator GameStart()
    {
        yield return new WaitForSeconds(0.35f);
        gameScore = 0;
        gameStart = true;
        gameOver = false;
        player.GetComponent<Player>().hitWall = false;
        levelManager.SpawnLevel();
    }

    public IEnumerator GameOver(GameObject hitBlock)
    {
        newLevel = false;
        checkNewBest();
        gameOver = true;
        gameStart = false;
        Time.timeScale = 0.1f;
        hitBlock.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 0.31f, 0.31f), 0.01f);
        yield return new WaitForSeconds(0.015f);
        hitBlock.GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.01f);
        yield return new WaitForSeconds(0.015f);
        hitBlock.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 0.31f, 0.31f), 0.01f);
        yield return new WaitForSeconds(0.015f);
        hitBlock.GetComponent<MeshRenderer>().material.DOColor(Color.white, 0.01f);
        yield return new WaitForSeconds(0.015f);
        hitBlock.GetComponent<MeshRenderer>().material.DOColor(new Color(1, 0.31f, 0.31f), 0.01f);

           
        yield return new WaitForSeconds(0.04f);
        Time.timeScale = 1;

        audioManager.DoVibration();
        audioManager.GameOver();
        GameObject playerPS = Instantiate(playerDestroy_FX, player.transform.position, Quaternion.identity);
        Destroy(playerPS, 2f);
        player.transform.GetChild(0).gameObject.SetActive(false);
        uiManager.GameOver();
    }
    void checkNewBest()
    {
        if (gameScore > hightScore)
        {
            uiManager.newBest();
            hightScore = gameScore;
            PlayerPrefs.SetInt("hightScore", hightScore);
        }
     }
}
