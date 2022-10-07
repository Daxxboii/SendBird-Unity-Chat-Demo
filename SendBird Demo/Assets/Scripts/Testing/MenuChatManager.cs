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

    [SerializeField]private TMP_InputField ChatText;


    private List<GameObject> ChatPrefabs;


    public void CreateClan(string ClanName)
    {
        SendBirdManager.instance.CreateClan(ClanName);
    }

    public void EnterClan()
    {
        SendBirdManager.instance.EnterClan();
    }


    public void SendMessage()
    {
        string Message = ChatText.text;
        SendBirdManager.instance.SendMessage(Message);
    }

    public void Leave()
    {
        SendBirdManager.instance.LeaveClan();
    }





}
