using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[Serializable]
public abstract class AbstractRoom : MonoBehaviour
{
    protected List<AbstractGameClient> Clients = new List<AbstractGameClient>();
    protected int GhostIndex;
    [SerializeField] private bool canAddedByManager = true;
    public bool CanAddedByManager { get => canAddedByManager; set => canAddedByManager = value; }
    [SerializeField] private bool spawnOnInitialize = false;
    public bool SpawnOnInitialize { get => spawnOnInitialize; set => spawnOnInitialize = value; }

    public abstract bool TryJoinClient(AbstractGameClient client);
    public abstract void ReturnClient(AbstractGameClient client);
    public abstract void UpdateClientList();
    public int clearScore;
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
