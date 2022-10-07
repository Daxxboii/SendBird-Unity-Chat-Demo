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
    public string ClanName;
    public Texture2D ClanImage;
    public string URL;
    //public string Description;
    public bool ViewGroupChatHistory;

    private List<string> ListOfNames;

    private string ProfileUrl;
    



    private void Awake()
    {
        if (instance == null) instance = this;
       ListOfNames = new List<string>();
       ListOfNames.Add(User);
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

    //Call This Function When a Clan is Created
    public void StartGroup(string ClanName)
    {
        GroupChannel.CreateChannelWithUserIds(ListOfNames,false,ClanName,URL,(GroupChannel groupChannel, SendBirdException e) =>
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
               // Debug.Log(userMessage.Message);
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
