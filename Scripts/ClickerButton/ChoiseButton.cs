using UnityEngine;
using UnityEngine.UI;

public class ChoiseButton : MonoBehaviour
{
    public Button buttonCopper;
    public Button buttonMelting;
    public Button buttonCoin;

    public void choiseCopper()
    {
        buttonCopper.gameObject.SetActive(true);
        buttonMelting.gameObject.SetActive(false);
        buttonCoin.gameObject.SetActive(false);
    }

    public void choiseMelting()
    {
        buttonMelting.gameObject.SetActive(true);
        buttonCopper.gameObject.SetActive(false);
        buttonCoin.gameObject.SetActive(false);
    }

    public void choiseCoin()
    {
        buttonCoin.gameObject.SetActive(true);
        buttonCopper.gameObject.SetActive(false);
        buttonMelting.gameObject.SetActive(false);
    }
}
