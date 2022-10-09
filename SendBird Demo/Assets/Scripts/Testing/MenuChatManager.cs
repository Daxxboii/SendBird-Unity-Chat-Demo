using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.UI;
using System;

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

    public static MenuChatManager instance;


    private List<GameObject> ChatPrefabs;

    public List<string> ClanMembers;

    private void Awake()
    {
        if (instance == null) instance = this;
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
        if ((ChatText.text).Trim().Length == 0)
        {
            Debug.Log("Message is empty!");
        }
        else
        {
            string Message = ChatText.text;
            ChatText.text = "";
            SendBirdManager.instance.SendMessage(Message);
        }
    }

    public void Leave()
    {
        SendBirdManager.instance.LeaveClan();
    }

    public void CreateClan()
    {
        SendBirdManager.instance.CreateClan(ClanMembers);
    }

    public void spawnChat(string User, string Message)
    {
        var Chat = Instantiate(ChatPrefab);
        Chat.GetComponentInChildren<TextMeshProUGUI>().text = User + ":" + Message;
        Chat.transform.SetParent(ChatArea.transform);
        ChatArea.GetComponent<RectTransform>().sizeDelta = new Vector2(ChatArea.GetComponent<RectTransform>().sizeDelta.x, ChatArea.GetComponent<RectTransform>().sizeDelta.y+30);
    }







}
