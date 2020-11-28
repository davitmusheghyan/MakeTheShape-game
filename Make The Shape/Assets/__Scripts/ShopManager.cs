using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [SerializeField]  Mesh[] skins;
    [SerializeField] MeshFilter player;
    public int currentSkinNumber;
    public int coinValue;

    void Start()
    {
        currentSkinNumber = PlayerPrefs.GetInt("currentSkinNumber");
        player.mesh = skins[currentSkinNumber];
        coinValue = PlayerPrefs.GetInt("coinValue");
        uiManager.coinAmount_txt.text = coinValue.ToString();
    }

    public void ChangeSkin(int skinNumber)
    {
        if (skinNumber != currentSkinNumber)
        {
            currentSkinNumber = skinNumber;
            PlayerPrefs.SetInt("currentSkinNumber", currentSkinNumber);
            player.mesh = skins[currentSkinNumber];
        }
    }

    public void RemoveCoin(int skinCost)
    {
        coinValue -= skinCost;
        PlayerPrefs.SetInt("coinValue", coinValue);
        uiManager.coinAmount_txt.text = coinValue.ToString();
    }

    public void AddCoin()
    {
        coinValue++;
        PlayerPrefs.SetInt("coinValue", coinValue);
        StartCoroutine(uiManager.AddCoin());
    }
}