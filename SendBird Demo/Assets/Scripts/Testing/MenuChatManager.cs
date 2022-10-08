using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.UI;

public class MenuChatManager : MonoBehaviour
{
    [Foldout("Menu", true)]
    [SerializeField] private GameObject ChatPanel;
    [SerializeField] private GameObject UserNamePanel;

    [SerializeField] private GameObject ChatArea;
    [SerializeField] private GameObject ChatPrefab;

    [SerializeField]private TMP_InputField ChatText;
    [SerializeField] private TMP_InputField UserName;

    [SerializeField] private string DefaultClanURL;


    private List<GameObject> ChatPrefabs;

    public List<string> ClanMembers;

    private void Awake()
    {
        SendBirdManager.onConnected += Enter;
    }
    public void CreateClan(string ClanName)
    {
       // SendBirdManager.instance.CreateClan(ClanName);
    }

    public void Connect()
    {
        SendBirdManager.instance.ConnectToServer(UserName.text);

    }

    void Enter()
    {
        SendBirdManager.instance.EnterClanWithURL(DefaultClanURL);
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

    public void CreateClan()
    {
        SendBirdManager.instance.CreateClan(ClanMembers);
    }







}
