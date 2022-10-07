using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.UI;

public class MenuChatManager : MonoBehaviour
{
    [Foldout("Menu", true)]
    [SerializeField] private GameObject ChatArea;
    [SerializeField] private GameObject ChatPrefab;
    private TextMeshProUGUI ChatText;
    private InputField ClanName;
    private List<GameObject> ChatPrefabs;


    private void Start()
    {
        
    }

    void CreateClan(string ClanName)
    {
        SendBirdManager.instance.StartGroup(ClanName);
    }

    void EnterClan()
    {

    }




}
