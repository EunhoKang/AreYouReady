using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class AbstractRoom : MonoBehaviour
{
    protected List<AbstractGameClient> Clients = new List<AbstractGameClient>();
    protected int GhostIndex;
    public abstract bool TryJoinClient(AbstractGameClient client);
    public abstract bool TryCloseRoom();
    public abstract void ReturnClient(AbstractGameClient client);
    public abstract void UpdateClientList();
    public void CloseRoom()
    {
        GameManager.instance.CloseRoom(this);
        foreach(var client in Clients)
        {
            Destroy(client.gameObject);
        }
        Destroy(this.gameObject);
    }
    public void TravelClient(AbstractGameClient client)
    {
        GhostIndex = Clients.IndexOf(client);
    }
    public void ReleaseClient(AbstractGameClient client)
    {
        Clients.Remove(client);
        UpdateClientList();
    }
}
