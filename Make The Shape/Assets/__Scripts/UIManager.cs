using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] AudioManager audioManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] Player player;

    [SerializeField] Canvas menu_cnvs;
    [SerializeField] Canvas game_cnvs;
    [SerializeField] Canvas score_cnvs;
    [SerializeField] Canvas gameOver_cnvs;

    public bool newBestAnimPlayed;

    
    public TextMeshProUGUI coinAmount_txt;
    [SerializeField] GameObject coin;

    [Space]
    [Header("Menu Canvas")]
    //>>>>>Menu<<<<<\\
    [SerializeField] GameObject settingsObj;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject shopObj;
    [SerializeField] Image transitionPanel_img;
    [SerializeField] Image settings_img;
    [SerializeField] Image[] volume_img;
    [SerializeField] Image[] vibration_img;
    [SerializeField] TextMeshProUGUI start_txt;
    public TextMeshProUGUI bestScore_txt;
    bool settingsWindowON;
    [SerializeField] float panelTime;
    [SerializeField] Image shop_img;
    [SerializeField] ScrollRect shopRect;
    [SerializeField] RectTransform shopWindow;
    [SerializeField] RectTransform shopExitButton;
    [SerializeField] Image[] shopExitButtonIcons;

    [Space]
    [Header("Game Canvas")]
    //>>>>>Game<<<<<\\
    [SerializeField] Animator newBestAnim;
    public TextMeshProUGUI gameScore_txt;
    [SerializeField] Image[] pauseSprites;
    [SerializeField] RectTransform pause;
    [SerializeField] Image pausePanel;
    [SerializeField] TextMeshProUGUI continue_txt;

    [Space]
    [Header("GameOver Canvas")]
    //>>>>>GameOver<<<<<\\
    [SerializeField] Image gameOverPanel;
    [SerializeField] Image homeButton;
    [SerializeField] Image homeButtonIcon;
    [SerializeField] TextMeshProUGUI home_txt;
    [SerializeField] TextMeshProUGUI tryAgain_txt;
    [SerializeField] TextMeshProUGUI newBest_txt;
    [SerializeField] TextMeshProUGUI currentScore_txt;

    void Start()
    {
        if (PlayerPrefs.GetInt("volumeStatus") == 0)
        {
            volume_img[0].enabled = true;
            volume_img[1].enabled = false;
        }
        else
        {
            volume_img[0].enabled = false;
            volume_img[1].enabled = true;
        }
        if (PlayerPrefs.GetInt("vibrationStatus") == 0)
        {
            vibration_img[0].enabled = true;
            vibration_img[1].enabled = false;
        }
        else
        {
            vibration_img[0].enabled = false;
            vibration_img[1].enabled = true;
        }
    }

    public void newBest()
    {
        if(!newBestAnimPlayed)
        {
            newBestAnim.SetBool("newBest", true);
            newBestAnimPlayed = true;
        }
    }

    public IEnumerator newLevel()
    {
        gameScore_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f);

        yield return new WaitForSeconds(0.15f);

        gameScore_txt.text = gameManager.gameScore.ToString();
        gameScore_txt.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f);

        yield return new WaitForSeconds(0.15f);

        gameScore_txt.rectTransform.DOScale(Vector3.one, 0.15f);
    }

    public void StartButtonDown()
    {
        audioManager.ButtonDown();
        start_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        start_txt.DOColor(new Color(start_txt.color.r, start_txt.color.g, start_txt.color.b, 0.55f), 0.2f);
    }
    public void StartButtonUp()
    {
        audioManager.ButtonUp();
        start_txt.rectTransform.DOScale(Vector3.one, 0.2f);
        start_txt.DOColor(new Color(start_txt.color.r, start_txt.color.g, start_txt.color.b, 1), 0.2f);
        StartCoroutine(Transition(false, true, false));
        StartCoroutine(gameManager.GameStart());
        newBestAnimPlayed = false;
    }

    public void SettingsButtonDown()
    {
        audioManager.ButtonDown();
        settings_img.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        settings_img.DOColor(new Color(settings_img.color.r, settings_img.color.g, settings_img.color.b, 0.55f), 0.2f);
    }
    public void SettingsButtonUp()
    {
        audioManager.ButtonUp();

        settings_img.rectTransform.DOScale(Vector3.one, 0.2f);
        settings_img.DOColor(new Color(settings_img.color.r, settings_img.color.g, settings_img.color.b, 1), 0.2f);

        if (!settingsWindowON)
        {
            settings_img.rectTransform.DORotate(new Vector3(0, 0, 180), 0.5f);
            volume_img[0].rectTransform.DOScale(Vector3.one, 0.5f);
            volume_img[1].rectTransform.DOScale(Vector3.one, 0.5f);
            vibration_img[0].rectTransform.DOScale(Vector3.one, 0.5f);
            vibration_img[1].rectTransform.DOScale(Vector3.one, 0.5f);

            StartCoroutine(resetSettingsRotation());

            settingsWindowON = true;
        }
        else
        {
            settings_img.rectTransform.DORotate(new Vector3(0, 0, -180), 0.5f);
            volume_img[0].rectTransform.DOScale(Vector3.zero, 0.5f);
            volume_img[1].rectTransform.DOScale(Vector3.zero, 0.5f);
            vibration_img[0].rectTransform.DOScale(Vector3.zero, 0.5f);
            vibration_img[1].rectTransform.DOScale(Vector3.zero, 0.5f);

            StartCoroutine(resetSettingsRotation());
            settingsWindowON = false;
        }
    }
    IEnumerator resetSettingsRotation()
    {
        yield return new WaitForSeconds(0.5f);
        settings_img.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void ShopButtonDown()
    {
        audioManager.ButtonDown();
        shop_img.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        shop_img.DOColor(new Color(shop_img.color.r, shop_img.color.g, shop_img.color.b, 0.55f), 0.2f);
    }
    public void ShopButtonUp()
    {
        audioManager.ButtonUp();
        shop_img.rectTransform.DOScale(Vector3.one, 0.2f);
        shop_img.DOColor(new Color(shop_img.color.r, shop_img.color.g, shop_img.color.b, 1), 0.2f);
        shopRect.enabled = true;
        settingsObj.SetActive(false);
        startObj.SetActive(false);
        shopObj.SetActive(false);
    }

    public void ShopExitButtonDown()
    {
        audioManager.ButtonDown();
        shopExitButton.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        for (int i = 0; i < shopExitButtonIcons.Length; i++)
        {
            shopExitButtonIcons[i].DOColor(new Color(shopExitButtonIcons[i].color.r, shopExitButtonIcons[i].color.g, shopExitButtonIcons[i].color.b, 0.55f), 0.2f);
        }
    }
    public void ShopExitButtonUp()
    {
        audioManager.ButtonUp();
        shopExitButton.DOScale(Vector3.one, 0.2f);
        for (int i = 0; i < shopExitButtonIcons.Length; i++)
        {
            shopExitButtonIcons[i].DOColor(new Color(shopExitButtonIcons[i].color.r, shopExitButtonIcons[i].color.g, shopExitButtonIcons[i].color.b, 1), 0.2f);
        }
        shopRect.enabled = false;
        shopWindow.DOLocalMoveY(-1000, 0.5f);
        settingsObj.SetActive(true);
        startObj.SetActive(true);
        shopObj.SetActive(true);
    }

    public void VolumeButtonDown()
    {
        audioManager.ButtonDown();
        volume_img[0].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        volume_img[0].DOColor(new Color(volume_img[0].color.r, volume_img[0].color.g, volume_img[0].color.b, 0.55f), 0.2f);
        volume_img[1].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        volume_img[1].DOColor(new Color(volume_img[1].color.r, volume_img[1].color.g, volume_img[1].color.b, 0.55f), 0.2f);
    }
    public void VolumeButtonUp()
    {
        audioManager.AudioOnOff();
        audioManager.ButtonUp();

        if (audioManager.audioStatus == 0)
        {
            volume_img[0].enabled = true;
            volume_img[1].enabled = false;
            volume_img[0].rectTransform.DOScale(Vector3.one, 0.2f);
            volume_img[0].DOColor(new Color(volume_img[0].color.r, volume_img[0].color.g, volume_img[0].color.b, 1), 0.2f);
        }
        else
        {
            volume_img[0].enabled = false;
            volume_img[1].enabled = true;
            volume_img[1].rectTransform.DOScale(Vector3.one, 0.2f);
            volume_img[1].DOColor(new Color(volume_img[1].color.r, volume_img[1].color.g, volume_img[1].color.b, 1), 0.2f);
        }
    }
    public void VibrationButtonDown()
    {
        audioManager.ButtonDown();
        vibration_img[0].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        vibration_img[0].DOColor(new Color(vibration_img[0].color.r, vibration_img[0].color.g, vibration_img[0].color.b, 0.55f), 0.2f);
        vibration_img[1].rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        vibration_img[1].DOColor(new Color(vibration_img[1].color.r, vibration_img[1].color.g, vibration_img[1].color.b, 0.55f), 0.2f);
    }
    public void VibrationButtonUp()
    {
        audioManager.ButtonUp();
        audioManager.VibrationOnOff();

        if (audioManager.vibrationStatus == 0)
        {
            vibration_img[0].enabled = true;
            vibration_img[1].enabled = false;
            vibration_img[0].rectTransform.DOScale(Vector3.one, 0.2f);
            vibration_img[0].DOColor(new Color(vibration_img[0].color.r, vibration_img[0].color.g, vibration_img[0].color.b, 1), 0.2f);
        }
        else
        {
            vibration_img[0].enabled = false;
            vibration_img[1].enabled = true;
            vibration_img[1].rectTransform.DOScale(Vector3.one, 0.2f);
            vibration_img[1].DOColor(new Color(vibration_img[1].color.r, vibration_img[1].color.g, vibration_img[1].color.b, 1), 0.2f);
        }
    }

    public void PauseButtonDown()
    {
        player.input = false;
        audioManager.ButtonDown();
        pause.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        for (int i = 0; i < pauseSprites.Length; i++)
        {
            pauseSprites[i].DOColor(new Color(pauseSprites[i].color.r, pauseSprites[i].color.g, pauseSprites[i].color.b, 0.55f), 0.2f);
        }
    }
    public void PauseButtonUp()
    {
        audioManager.ButtonUp();
        pause.DOScale(Vector3.one, 0.2f);
        for (int i = 0; i < pauseSprites.Length; i++)
        {
            pauseSprites[i].DOColor(new Color(pauseSprites[i].color.r, pauseSprites[i].color.g, pauseSprites[i].color.b, 1), 0.2f);
        }

        if(!gameManager.gameOver)
        {
            pause.gameObject.SetActive(false);

            gameManager.pause = true;

            pausePanel.gameObject.SetActive(true);
            pausePanel.DOColor(new Color(pausePanel.color.r, pausePanel.color.g, pausePanel.color.b, 0.5f), 1f);
            continue_txt.enabled = true;
            continue_txt.DOColor(new Color(continue_txt.color.r, continue_txt.color.g, continue_txt.color.b, 1), 1f);
        }

        for (int i = 0; i < levelManager.sceneCoins.Count; i++)
        {
            levelManager.sceneCoins[i].GetComponent<Coin>().rotateSpeed = 0;
        }
    }
    public void continueGameButtonDown()
    {
        audioManager.ButtonDown();
        continue_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        continue_txt.DOColor(new Color(continue_txt.color.r, continue_txt.color.g, continue_txt.color.b, 0.55f), 0.2f);
    }
    public void continueGameButtonUp()
    {
        player.input = true;
        audioManager.ButtonUp();
        continue_txt.rectTransform.DOScale(Vector3.one, 0.2f);
        continue_txt.DOColor(new Color(continue_txt.color.r, continue_txt.color.g, continue_txt.color.b, 1), 0.2f);
        pausePanel.DOColor(new Color(pausePanel.color.r, pausePanel.color.g, pausePanel.color.b, 0), 0.5f);
        gameManager.pause = false;
        pause.gameObject.SetActive(true);
        StartCoroutine(continueGame());

        for (int i = 0; i < levelManager.sceneCoins.Count; i++)
        {
            levelManager.sceneCoins[i].GetComponent<Coin>().rotateSpeed = 250;
        }
    }
    IEnumerator continueGame()
    {
        continue_txt.enabled = false;
        yield return new WaitForSeconds(0.5f);
        pausePanel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        newBestAnim.SetBool("newBest", false);
        game_cnvs.enabled = false;
        score_cnvs.enabled = false;
        gameOver_cnvs.enabled = true;
        coin.SetActive(false);

        gameOverPanel.DOColor(new Color(gameOverPanel.color.r, gameOverPanel.color.g, gameOverPanel.color.b, 0.5f), 0.5f);
        StartCoroutine(gameOverCanvas());

        currentScore_txt.text = gameManager.gameScore.ToString();
        if (newBestAnimPlayed)
            newBest_txt.text = "new best";
        else
            newBest_txt.text = "best " + gameManager.hightScore.ToString();
    }
    IEnumerator gameOverCanvas()
    {
        yield return new WaitForSeconds(0.4f);
        homeButton.enabled = true;
        homeButtonIcon.DOColor(new Color(homeButtonIcon.color.r, homeButtonIcon.color.g, homeButtonIcon.color.b, 1), 0.2f);
        home_txt.DOColor(new Color(home_txt.color.r, home_txt.color.g, home_txt.color.b, 1), 0.2f);
        tryAgain_txt.transform.parent.gameObject.SetActive(true);
        tryAgain_txt.DOColor(new Color(tryAgain_txt.color.r, tryAgain_txt.color.g, tryAgain_txt.color.b, 1), 0.2f);
        newBest_txt.DOColor(new Color(newBest_txt.color.r, newBest_txt.color.g, newBest_txt.color.b, 1), 0.2f);
        currentScore_txt.DOColor(new Color(currentScore_txt.color.r, currentScore_txt.color.g, currentScore_txt.color.b, 1), 0.2f);
    }

    public void homeButtonDown()
    {
        audioManager.ButtonDown();
        homeButton.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        homeButtonIcon.DOColor(new Color(homeButtonIcon.color.r, homeButtonIcon.color.g, homeButtonIcon.color.b, 0.55f), 0.2f);
        home_txt.DOColor(new Color(home_txt.color.r, home_txt.color.g, home_txt.color.b, 0.55f), 0.2f);
    }
    public void homeButtonUp()
    {
        audioManager.ButtonUp();
        gameScore_txt.text = "0";
        bestScore_txt.text = gameManager.hightScore.ToString();
        StartCoroutine(Transition(true, false, false));
        StartCoroutine(levelManager.resetAll());
        newBestAnimPlayed = false;
    }

    public void TryAgainButtonDown()
    {
        audioManager.ButtonDown();
        tryAgain_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        tryAgain_txt.DOColor(new Color(tryAgain_txt.color.r, tryAgain_txt.color.g, tryAgain_txt.color.b, 0.55f), 0.2f);
    }
    public void TryAgainButtonUp()
    {
        audioManager.ButtonUp();
        gameScore_txt.text = "0";
        tryAgain_txt.rectTransform.DOScale(Vector3.one, 0.2f);
        tryAgain_txt.DOColor(new Color(tryAgain_txt.color.r, tryAgain_txt.color.g, tryAgain_txt.color.b, 1), 0.2f);
        StartCoroutine(Transition(false, true, false));
        StartCoroutine(gameManager.GameStart());
        StartCoroutine(levelManager.resetAll());
        newBestAnimPlayed = false;
    }

    public IEnumerator AddCoin()
    {
        coinAmount_txt.rectTransform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f);

        yield return new WaitForSeconds(0.15f);

        coinAmount_txt.text = shopManager.coinValue.ToString();
        coinAmount_txt.rectTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.15f);

        yield return new WaitForSeconds(0.15f);

        coinAmount_txt.rectTransform.DOScale(Vector3.one, 0.15f);        
    }
    public IEnumerator Transition(bool menu, bool game, bool gameOver)
    {
        transitionPanel_img.raycastTarget = true;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 1), panelTime);
        yield return new WaitForSeconds(panelTime);
        menu_cnvs.enabled = menu;
        game_cnvs.enabled = game;
        score_cnvs.enabled = game;
        coin.SetActive(true);
        gameOver_cnvs.enabled = gameOver;
        transitionPanel_img.raycastTarget = false;
        transitionPanel_img.DOColor(new Color(transitionPanel_img.color.r, transitionPanel_img.color.g, transitionPanel_img.color.b, 0), panelTime);
    }
}
