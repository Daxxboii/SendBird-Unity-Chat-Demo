using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using TMPro;

public class MenuChatManager : MonoBehaviour
{
    [Foldout("Menu", true)]
    [SerializeField] private GameObject ChatArea;
    [SerializeField] private GameObject ChatPrefab;
    private TextMeshProUGUI ChatText;
    private List<GameObject> ChatPrefabs;


    private void Start()
    {
        
    }




}
