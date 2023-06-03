using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class AbstractGameClient : MonoBehaviour
{
    public AbstractRoom CurrentRoom;
    public abstract bool ReadyForGame();
    public abstract void RejectByServer();
    protected bool CanMove = true;
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
