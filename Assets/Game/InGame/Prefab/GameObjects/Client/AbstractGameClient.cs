using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class AbstractGameClient : MonoBehaviour
{
    public AbstractRoom CurrentRoom;
    public abstract bool ReadyForGame();
    public abstract void ShowResponseForReady();
    public abstract void ShowResponseForReject();
    private bool canMove = true;
    public bool CanMove { get => canMove; set => canMove = value; }

    public void HostToRoom(AbstractRoom room)
    {
        CurrentRoom?.ReleaseClient(this);
        CurrentRoom = room;
    }
    public void ReturnToPreviousRoom()
    {
        CurrentRoom?.ReturnClient(this);
    }
    public void SelectedByServer()
    {
        CurrentRoom?.TravelClient(this);
    }
}
