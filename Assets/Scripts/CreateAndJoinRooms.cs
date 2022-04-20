using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{

    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField nameInput;
    public TextMeshProUGUI nameInputPretext;
    public TextMeshProUGUI setNameButtonText;
    public TextMeshProUGUI playerName;

    static public int playerType;

    public bool nameSetupActive;

    private void Start()
    {
        playerType = 0;
        nameInput.characterLimit = 16;
        joinInput.characterLimit = 16;
        createInput.characterLimit = 16;
        playerName.text = "Guest";
    }
    public void SetName()
    {
        // left off here setting up the name thing
        if (nameSetupActive == true)
        {
            nameSetupActive = false;
            playerName.text = nameInput.text;
            setNameButtonText.text = ("Set Name");
            nameInput.gameObject.SetActive(false);
            playerName.gameObject.SetActive(true);
            return;
        }
        if (nameSetupActive == false)
        {
            nameSetupActive = true;
            setNameButtonText.text = ("Ok");
            nameInput.gameObject.SetActive(true);
            playerName.gameObject.SetActive(false);
            return;
        }
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = (playerName.text);
        PhotonNetwork.LoadLevel("GameArea1");
    }
}
