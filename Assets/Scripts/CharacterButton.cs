using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    GameObject charSelectManager;
    [SerializeField] string charName;
    [SerializeField] string charDescription;
    [SerializeField] GameObject charSpawnObj;

    
    // Start is called before the first frame update
    void Start()
    {
        charSelectManager = GameObject.Find("CharSelectManager");
    }
    public void HoverSelection()
    {
        charSelectManager.GetComponent<CharacterSelectManager>().SetCharacterInfo(charDescription);
    } 
    public void SetSelection()
    {
        charSelectManager.GetComponent<CharacterSelectManager>().SetChosenCharacter(charName, gameObject.GetComponent<Image>().sprite,charSpawnObj);
    }
}
