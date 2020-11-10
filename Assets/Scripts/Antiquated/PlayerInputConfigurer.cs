using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using UnityEngine.UIElements;

public class PlayerInputConfigurer : MonoBehaviour
{
    [ContextMenuItem("Refresh", "PullActionEventsTemplateFromPlayerInput")]
    public PlayerInput playerInput;
    [SerializeField]
    private List<PlayerInput.ActionEvent> inputEvents = new List<PlayerInput.ActionEvent>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void PullActionEventsTemplateFromPlayerInput()
    {
        inputEvents = new List<PlayerInput.ActionEvent>();
        for (int i = 0; i < playerInput.actionEvents.Count; i++)
        {

            inputEvents.Add(playerInput.actionEvents[i]);
            Debug.Log(playerInput.actions["Move"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
