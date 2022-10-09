using MyBox;
using SendBird;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static SendBirdManager;


public class SendBirdManager : MonoBehaviour
{
    public static SendBirdManager instance;

    public int MessageLoadLimit;

    public delegate void OnConnected();
    public static OnConnected onConnected;

    [Foldout("Chat Settings",true)]
    public bool ReadReceipts;
    public bool IsTypingNotification;

    [Foldout("Private Chat",true)]
   // public string UserOnOtherEnd;
    public bool ViewPrivateChatHistory;
   

    [Foldout("Group Chat",true)]
    public string URL;
    public bool ViewGroupChatHistory;

   
    private string ProfileUrl;

    GroupChannel currentClan;
    



    private void Awake()
    {
        if (instance == null) instance = this;

      

       InitializeSendBird();
      // ConnectToServer();
      // SetHandlers();

    }

    #region Initialize
    public void InitializeSendBird()
    {
        SendBirdClient.SetupUnityDispatcher(gameObject);
        StartCoroutine(SendBirdClient.StartUnityDispatcher);

        //Replace AppID wiht your own @JP
        SendBirdClient.Init("9F8F1088-CF75-45A3-B70C-159F4C8ECF6B");
    }

    public void ConnectToServer(string Username)
    {
        SendBirdClient.Connect(Username, (User user, SendBirdException e) =>
        {
        ProfileUrl = user.ProfileUrl;
       
            if (e != null)
            {
                Debug.LogError(e);
                return; // Handle error.
            }
            else
            {
                onConnected.Invoke();
                SetHandlers();
                Debug.Log("Connected");

                SendBirdClient.UpdateCurrentUserInfo(Username, ProfileUrl, (SendBirdException e) => 
                { 
                    if(e != null)
                    {
                        Debug.LogError(e);
                    }
                });
            }
        });

    }

    private void SetHandlers()
    {
        SendBirdClient.ChannelHandler channelHandler = new SendBirdClient.ChannelHandler();

        channelHandler.OnMessageReceived = (BaseChannel baseChannel, BaseMessage baseMessage) => {
            MenuChatManager.instance.spawnChat(baseMessage.GetSender().Nickname, baseMessage.Message);
        };
        SendBirdClient.AddChannelHandler("UniqueID", channelHandler);
    }

     #endregion

    void AddPersonToClan(string Name)
    {
        List<string> Names = new List<string>();
        Names.Add(Name);

        currentClan.InviteWithUserIds(Names, (SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
            }
        });
    }

    void KickPersonFromClan(string UserID)
    {

        currentClan.BanUserWithUserId(UserID,"trolling",1000000, (SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
            }
        });
    }

   

    public void LeaveClan()
    {
        currentClan.Leave((SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
            }
        });
    }

    

    //Call This Function When a Clan is Created
    public void CreateClan(List<string> UserIDS)
    {
        GroupChannel.CreateChannelWithUserIds(UserIDS, true,(GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
                Debug.LogError(e);
            }
            else
            {
                Debug.Log("Group Created");
                currentClan = groupChannel;
                URL = groupChannel.Url;
                AdminEnterClan();
            }
        });
    }

    #region Testing
    public void AdminEnterClan()
    {
        GroupChannel.GetChannel(URL, (GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null) Debug.LogError(e);
            else Debug.Log("Entered Successfully");
        });
    }

    public void EnterClanWithURL(string ClanURL)
    {
        GroupChannel.GetChannel(ClanURL, (GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null) Debug.LogError(e);
            else
            {
                Debug.Log("Entered Successfully");
                currentClan = groupChannel;

                PreviousMessageListQuery mListQuery = groupChannel.CreatePreviousMessageListQuery();
                mListQuery.Load(MessageLoadLimit, true, (List<BaseMessage> messages, SendBirdException e) =>
                {

                    if (e != null) { Debug.Log(e); }
                    else
                    {
                        messages.Reverse();
                        foreach(BaseMessage message in messages)
                        {
                            MenuChatManager.instance.spawnChat(message.GetSender().Nickname, message.Message);
                        }
                    }
                
                });
            }
        });
    }
    #endregion


    public new void SendMessage(string Message)
    {
        currentClan.SendUserMessage(Message, "Message", (UserMessage userMessage, SendBirdException e) =>
        {
            if (e != null) Debug.Log(e);
            else
            {
                MenuChatManager.instance.spawnChat(userMessage.GetSender().Nickname, userMessage.Message);
                Debug.Log("Message Sent");
            }
        });
    }

    private void OnApplicationQuit()
    {
        SendBirdClient.RemoveChannelHandler("Ligma");
        SendBirdClient.Disconnect(() =>{ });
    }
}


