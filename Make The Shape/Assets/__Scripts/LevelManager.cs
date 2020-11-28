using System.Collections.Generic;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] Player player;

    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject[] walls;

    [SerializeField] GameObject currentWall;
    public GameObject currentLevel;

    [SerializeField] Transform levelSpawnPosition, wallSpawnPosition;

    [SerializeField] GameObject[] playerObjects;

    [SerializeField] float scaleTime, scaleWait;
    public static float levelMoveSpeed;

    [Space]
    [SerializeField] Camera myCamera;
    [SerializeField] ColorPallets[] colors;
    [SerializeField] Material[] materials;
    [SerializeField] float colorChangeTime;
    [SerializeField] int palletNumber;
    [SerializeField] int updatePallet;
    [SerializeField] int backgroundNumber;
    [SerializeField] bool stop;

    [SerializeField] int lastLevel;

    [SerializeField] ParticleSystem[] backgroundPS;

    [SerializeField] GameObject coin;
    [SerializeField] List<Transform> coinSpawnPositions = new List<Transform>();
    public List<GameObject> sceneCoins = new List<GameObject>();


    void Start()
    {
        levelMoveSpeed = 40f;

        palletNumber = Random.Range(0, colors.Length);
        backgroundNumber = Random.Range(0, backgrounds.Length);
        updatePallet = -1;

        myCamera = Camera.main;

        StartColors();
        StartBackground();
    }

    void Update()
    {
        if (updatePallet == 10)
        {
            UpdatePallet();
            updatePallet = 0;
        }


        if (backgroundNumber == backgrounds.Length)
            backgroundNumber = 0;

        if (palletNumber == colors.Length)
            palletNumber = 0;

        if (gameManager.pause)
        {
            for (int i = 0; i < backgroundPS.Length; i++)
            {
                backgroundPS[i].Pause();
            }
        }
        else
        {
            for (int i = 0; i < backgroundPS.Length; i++)
            {
                backgroundPS[i].Play();
            }
        }

        RenderSettings.fogColor = myCamera.backgroundColor;
    }

    public void SpawnLevel()
    {
        coinSpawnPositions = new List<Transform>(new Transform[0]);

        for (int i = 0; i < sceneCoins.Count; i++)
        {
            Destroy(sceneCoins[i]);
        }

        sceneCoins = new List<GameObject>(new GameObject[0]);

        player.input = false;
        StartCoroutine(wait());
        StartCoroutine(Scaling());

        int randomNumber = Random.Range(0, levels.Length);
        if(randomNumber == lastLevel)
            randomNumber = Random.Range(0, levels.Length);

        currentLevel = Instantiate(levels[randomNumber], levelSpawnPosition.position, Quaternion.identity);
        currentWall = Instantiate(walls[randomNumber], wallSpawnPosition.position, Quaternion.identity);

        currentLevel.GetComponent<Level>().wall = currentWall;

        for (int i = 0; i < playerObjects.Length; i++)
        {
            playerObjects[i].transform.position = currentWall.transform.GetChild(0).transform.position;
            playerObjects[0].transform.rotation = Quaternion.identity;
            playerObjects[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }

        for (int i = 0; i < currentWall.transform.GetChild(2).childCount; i++)
        {
            coinSpawnPositions.Add(currentWall.transform.GetChild(2).GetChild(i));
        }

        int randomCoinsQuantity = Random.Range(0, coinSpawnPositions.Count);

        for (int i = 0; i < randomCoinsQuantity; i++)
        {
            int randomPosition = Random.Range(0, coinSpawnPositions.Count);

            sceneCoins.Add(Instantiate(coin, coinSpawnPositions[randomPosition].position, Quaternion.identity));
            coinSpawnPositions.RemoveAt(randomPosition);
        }

        updatePallet++;        
    }
    IEnumerator Scaling()
    {
        playerObjects[0].transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        playerObjects[0].transform.DOScale(Vector3.one, scaleTime);

        yield return new WaitForSeconds(scaleWait);

        for (int i = 0; i < currentWall.transform.GetChild(1).childCount; i++)
        {
            currentWall.transform.GetChild(1).transform.GetChild(i).DOScale(new Vector3(1.1f, 1.1f, 1.1f), scaleTime);
        }
        playerObjects[0].transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), scaleTime);

        yield return new WaitForSeconds(scaleWait);
        for (int i = 0; i < currentWall.transform.GetChild(1).childCount; i++)
        {
            currentWall.transform.GetChild(1).transform.GetChild(i).DOScale(Vector3.one, scaleTime);
        }
        playerObjects[0].transform.DOScale(Vector3.one, scaleTime);
        playerObjects[0].transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.2f);
        player.input = true;
    }

    public IEnumerator resetAll()
    {
        levelMoveSpeed = 40f;
        updatePallet = -1;
        playerObjects[0].transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < playerObjects.Length; i++)
        {
            playerObjects[i].transform.position = Vector3.zero;
        }
        yield return new WaitForSeconds(0.05f);
        playerObjects[0].transform.GetChild(0).gameObject.SetActive(true);
    }

    void StartColors()
    {
        materials[0].color = colors[palletNumber].groundMain;
        materials[1].color = colors[palletNumber].groundLine;
        RenderSettings.fogColor = myCamera.backgroundColor = colors[palletNumber].backgroundAndFog;
        palletNumber++;
    }
    void UpdatePallet()
    {
        materials[0].DOColor(colors[palletNumber].groundMain, colorChangeTime);
        materials[1].DOColor(colors[palletNumber].groundLine, colorChangeTime);
        myCamera.DOColor(colors[palletNumber].backgroundAndFog, colorChangeTime);
        palletNumber++;
    }

    void StartBackground()
    {
        GameObject background = Instantiate(backgrounds[backgroundNumber], backgrounds[backgroundNumber].transform.position, Quaternion.identity);
        Destroy(background, 40);
        backgroundNumber++;
        StartCoroutine(NewBackground());
    }

    IEnumerator NewBackground()
    {
        while(!stop)
        {
            yield return new WaitForSeconds(30);
            GameObject background = Instantiate(backgrounds[backgroundNumber], backgrounds[backgroundNumber].transform.position, Quaternion.identity);
            Destroy(background, 40);
            backgroundNumber++;
        }
    }
}

[System.Serializable]
class ColorPallets
{
    public string palletName;
    public Color groundMain, groundLine, backgroundAndFog;
}