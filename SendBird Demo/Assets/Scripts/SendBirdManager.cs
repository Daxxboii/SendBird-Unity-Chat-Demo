using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SendBird;
using UnityEngine.Networking.Types;
using Unity.VisualScripting;
using MyBox;

public class SendBirdManager : MonoBehaviour
{

    public string User;

    [Foldout("Chat Settings",true)]
    public bool ReadReceipts;
    public bool IsTypingNotification;

    [Foldout("Private Chat",true)]
    public string UserOnOtherEnd;

    [Foldout("Group Chat",true)]
    public string ClanName;
    public Texture2D ClanImage;
    public string Description;
    public List<SendBird.User> UsersToAddInGroup;


    private void Awake()
    {
        InitializeSendBird();
        ConnectToServer();
    }

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
    
    public void CreateToGroup()
    {
        GroupChannel.CreateChannel(UsersToAddInGroup, true, ClanName, ClanImage, Description,"Clan", (GroupChannel groupChannel, SendBirdException e) =>
        {
            if (e != null)
            {
                // Handle error.
            }

            // A group channel with detailed configuration is successfully created.
            // By using groupChannel.Url, groupChannel.Data, groupChannel.CustomType, and so on,
            // you can access the result object from Sendbird server to check your parameter configuration.
            string CustomType = groupChannel.CustomType;
            
});
    }
}
