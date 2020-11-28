using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ShopButton : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] ShopManager shopManager;
    [SerializeField] int skinNumber;
    [SerializeField] int skinCost;

    [SerializeField] int skinEnabled;

    [SerializeField] bool defaultSkin;

    [SerializeField] GameObject openObj;
    [SerializeField] GameObject closeObj;

    [SerializeField] Animator noMoney;
    [SerializeField] Animator newSkin;
 
    void Start()
    {
        if(!defaultSkin)
        {
            noMoney.enabled = false;
            newSkin.enabled = false;
        }
        //PlayerPrefs.DeleteKey("skinEnabled" + skinNumber);

        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        skinEnabled = PlayerPrefs.GetInt("skinEnabled" + skinNumber);

        if (skinEnabled == 1)
        {
            openObj.SetActive(true);
            closeObj.SetActive(false);
        }
    }

    public void Call()
    {
        if(defaultSkin)
        {
            shopManager.ChangeSkin(skinNumber);
        }
        else
        {
            if (skinEnabled != 1)
            {
                if (shopManager.coinValue >= skinCost)
                {
                    shopManager.RemoveCoin(skinCost);
                    shopManager.ChangeSkin(skinNumber);
                    openObj.SetActive(true);
                    closeObj.SetActive(false);
                    StartCoroutine(NewSkin());
                    skinEnabled = 1;
                    PlayerPrefs.SetInt("skinEnabled" + skinNumber, skinEnabled);
                }
                else
                {
                    noMoney.enabled = true;
                    StartCoroutine(HaventMoney());
                }
            }
            else
            {
                shopManager.ChangeSkin(skinNumber);
            }
        }
    }

    public void DefaultButtonDown()
    {
        audioManager.ButtonDown();
        openObj.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
    }
    public void DefaultButtonUp()
    {
        audioManager.ButtonUp();
        openObj.transform.DOScale(Vector3.one, 0.2f);
        Call();
    }

    public void buttonDown()
    {
        audioManager.ButtonDown();
        openObj.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
        closeObj.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.2f);
    }
    public void buttonUp()
    {
        audioManager.ButtonUp();
        openObj.transform.DOScale(Vector3.one, 0.2f);
        closeObj.transform.DOScale(Vector3.one, 0.2f);
        Call();
    }

    IEnumerator HaventMoney()
    {
        audioManager.NoMoney();

        noMoney.SetBool("haventMoney", true);
        yield return new WaitForSeconds(0.15f);
        noMoney.SetBool("haventMoney", false);
        yield return new WaitForSeconds(0.4f);
        noMoney.enabled = false;
    }
    IEnumerator NewSkin()
    {
        audioManager.NewSkin();

        newSkin.enabled = true;
        newSkin.SetBool("newSkin", true);
        yield return new WaitForSeconds(0.4f);
        newSkin.enabled = false;
    }
}
