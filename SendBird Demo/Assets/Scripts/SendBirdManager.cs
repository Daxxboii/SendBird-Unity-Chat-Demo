using MyBox;
using SendBird;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SendBirdManager : MonoBehaviour
{

    public string User;

    [Foldout("Chat Settings",true)]
    public bool ReadReceipts;
    public bool IsTypingNotification;

    [Foldout("Private Chat",true)]
    public string UserOnOtherEnd;
    public bool ViewPrivateChatHistory;
   

    [Foldout("Group Chat",true)]
    public string ClanName;
    public Texture2D ClanImage;
    public string URL;
    public string Description;
    public List<SendBird.User> ClanMembers;
    public bool ViewGroupChatHistory;

    private List<string> ListOfNames;



    private void Awake()
    {
        ListOfNames = new List<string>();
        ListOfNames.Add("River Testing");
        ListOfNames.Add("Daxx");
        InitializeSendBird();
        ConnectToServer();
        SetHandlers();
    }

    #region Initialize
    private void InitializeSendBird()
    {
        SendBirdClient.SetupUnityDispatcher(gameObject);
        StartCoroutine(SendBirdClient.StartUnityDispatcher);

        //Replace AppID wiht your own @JP
        SendBirdClient.Init("9F8F1088-CF75-45A3-B70C-159F4C8ECF6B");
    }

    private void ConnectToServer()
    {
        SendBirdClient.Connect(User, (User user, SendBirdException e) =>
        {
            if (e != null)
            {
                return; // Handle error.
            }
            else
            {
                Debug.Log("Connected");
            }
        });
    }

    private void SetHandlers()
    {
        SendBirdClient.ChannelHandler channelHandler = new SendBirdClient.ChannelHandler();

        channelHandler.OnMessageReceived = (BaseChannel baseChannel, BaseMessage baseMessage) => { Debug.Log(baseMessage); };
        SendBirdClient.AddChannelHandler("UniqueID", channelHandler);
    }

     #endregion


    public void StartGroup()
    {
        GroupChannel.CreateChannelWithUserIds(ListOfNames, false, (GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
                Debug.LogError(e);
            }
            else
            {
                Debug.Log("Group Created");
            }
            string CustomType = groupChannel.CustomType;
            string ChanneUrl = groupChannel.Url;

            groupChannel.SendUserMessage("hello", "Message", (UserMessage userMessage, SendBirdException e) =>
            {
                Debug.Log(userMessage.Message);
            });
            
        });
    }

    public void EnterGroup()
    {
        GroupChannel.GetChannel(URL, (GroupChannel groupChannel, SendBirdException e) =>
        {
           
        });
    }

    public void UpdateGroup()
    {
        

    }

    private void OnApplicationQuit()
    {
        SendBirdClient.RemoveChannelHandler("Ligma");
        SendBirdClient.Disconnect(() =>{ });
    }





}
