using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource background;
    public AudioSource[] button;
    [SerializeField] AudioSource playerMovement;
    [SerializeField] AudioSource coin;
    [SerializeField] AudioSource newSkin;
    [SerializeField] AudioSource noMoney;
    [SerializeField] AudioSource newLevel;
    [SerializeField] AudioSource gameOver;

    public int audioStatus;
    public int vibrationStatus;

    void Start()
    {
        audioStatus = PlayerPrefs.GetInt("volumeStatus");
        vibrationStatus = PlayerPrefs.GetInt("vibrationStatus");
        if (audioStatus == 0)
            background.volume = 0.5f;
        else
            background.volume = 0;
    }

    public void PlayerMove()
    {
        if (audioStatus == 0)
        {
            playerMovement.Play();
        }
    }

    public void Coin()
    {
        if (audioStatus == 0)
        {
            coin.Play();
        }
    }

    public void NewLevel()
    {
        if (audioStatus == 0)
        {
            newLevel.Play();
        }
    }

    public void NewSkin()
    {
        if (audioStatus == 0)
        {
            newSkin.Play();
        }
    }
    public void NoMoney()
    {
        if (audioStatus == 0)
        {
            noMoney.Play();
        }
    }

    public void GameOver()
    {
        if(audioStatus == 0)
        {
            gameOver.Play();
        }
    }

    public void AudioOnOff()
    {
        if(audioStatus == 0)
        {
            background.volume = 0f;
            audioStatus = 1;
            PlayerPrefs.SetInt("volumeStatus", audioStatus);
        } else
        {
            background.volume = 0.5f;
            audioStatus = 0;
            PlayerPrefs.SetInt("volumeStatus", audioStatus);
        }
    }
    public void VibrationOnOff()
    {
        if (vibrationStatus == 0)
        {
            vibrationStatus = 1;
            PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);
        }
        else
        {
            vibrationStatus = 0;
            PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);
        }
    }

    public void DoVibration()
    {
        if (vibrationStatus == 0)
        {
            Handheld.Vibrate();
        }
    }

    public void ButtonDown()
    {
        if(audioStatus == 0)
        {
            button[0].Play();
        }
    }
    public void ButtonUp()
    {
        if (audioStatus == 0)
        {
            button[1].Play();
        }
    }
}
