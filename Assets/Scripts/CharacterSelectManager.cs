using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class CharacterSelectManager : MonoBehaviour
{
    public string charSelected;
    public GameObject charSelectedObj;

    // Character Info
    [SerializeField] TextMeshProUGUI charinfoDescription;
    [SerializeField] GameObject readyButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject charSelectDisplay;
    [SerializeField] GameObject charSelectedSprite;
    [SerializeField] GameObject charSelectionScreen;

    [SerializeField] GameObject uiOverlay;

    public void SetCharacterInfo(string charDescription)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        charinfoDescription.text = charDescription;
    }
    public void SetChosenCharacter(string charName,Sprite charSprite,GameObject charSelect)
    {
        charSelected = charName;
        charSelectedObj = charSelect;
        ToggleUI(true);
        charSelectedSprite.GetComponent<Image>().sprite = charSprite;
    }
    private void ToggleUI(bool on)
    {
        if (on)
        {
            readyButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(true);
            charSelectDisplay.gameObject.SetActive(true);
            charSelectionScreen.gameObject.SetActive(false);
        }
        else
        {
            readyButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            charSelectDisplay.gameObject.SetActive(false);
            charSelectionScreen.gameObject.SetActive(true);
        }

    }
    public void CreateCharacter()
    {
        Vector3 characterSpawnLoc = new Vector3(0, 0, 0);
        PhotonNetwork.Instantiate(charSelectedObj.name, characterSpawnLoc, Quaternion.identity);
        ToggleUI(false);
        uiOverlay.SetActive(false);
    }
    public void ResetConditions()
    {
        charinfoDescription.text = "hover over a character to see more information";
        charSelected = null;
        ToggleUI(false);
    }
}
