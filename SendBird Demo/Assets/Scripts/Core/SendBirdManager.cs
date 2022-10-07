using MyBox;
using SendBird;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class SendBirdManager : MonoBehaviour
{

    public string User;
    public static SendBirdManager instance;

    [Foldout("Chat Settings",true)]
    public bool ReadReceipts;
    public bool IsTypingNotification;

    [Foldout("Private Chat",true)]
    public string UserOnOtherEnd;
    public bool ViewPrivateChatHistory;
   

    [Foldout("Group Chat",true)]
    public string URL;
    public bool ViewGroupChatHistory;

    private List<string> ListOfPeopleInClan;

    private string ProfileUrl;

    GroupChannel currentClan;
    



    private void Awake()
    {
        if (instance == null) instance = this;

       ListOfPeopleInClan = new List<string>();
       ListOfPeopleInClan.Add(User);


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
        ProfileUrl = user.ProfileUrl;
       
            if (e != null)
            {
                Debug.LogError(e);
                return; // Handle error.
            }
            else
            {
                Debug.Log("Connected");

                SendBirdClient.UpdateCurrentUserInfo(User, ProfileUrl, (SendBirdException e) => 
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
            Debug.Log(baseMessage.UserId);
            Debug.Log(baseMessage.Message);
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
    public void CreateClan(string ClanName)
    {
        GroupChannel.CreateChannelWithUserIds(ListOfPeopleInClan,false,(GroupChannel groupChannel, SendBirdException e) =>
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
                EnterClan();
            }
        });
    }

    #region Testing
    public void EnterClan()
    {
        GroupChannel.GetChannel(URL, (GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null) Debug.LogError(e);
            else Debug.Log("Entered Successfully");
        });
    }
    #endregion


    public new void SendMessage(string Message)
    {
        currentClan.SendUserMessage(Message, "Message", (UserMessage userMessage, SendBirdException e) =>
        {
            if (e != null) Debug.Log(e);
        });
    }

    private void OnApplicationQuit()
    {
        SendBirdClient.RemoveChannelHandler("Ligma");
        SendBirdClient.Disconnect(() =>{ });
    }
}


