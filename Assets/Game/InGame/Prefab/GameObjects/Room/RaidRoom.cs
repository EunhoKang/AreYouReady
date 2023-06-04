using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RaidRoom : AbstractRoom
{
    public AudioClip clip;
    public int MaxClientCount = 4;
    public List<Transform> Slots;

    public bool TryCloseRoom()
    {
        bool flag = true;
        foreach(var client in Clients)
        {
            flag &= client.ReadyForGame();
        }
        return flag;
    }

    public override bool TryJoinClient(AbstractGameClient client)
    {
        if(Clients.Count < MaxClientCount)
        {
            Clients.Add(client);
            client.HostToRoom(this);
            UpdateClientList();
            if(Clients.Count == MaxClientCount)
            {
                if(TryCloseRoom())
                {
                    RequestResponseForReady();
                }
                else
                {
                    RequestResponseForReject();
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public async void RequestResponseForReady()
    {
        SoundManager.instance.PlaySound(clip);
        foreach(var client in Clients)
        {
            client.ShowResponseForReady();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        CloseRoom();
    }

    public async void RequestResponseForReject()
    {
        foreach(var client in Clients)
        {
            client.ShowResponseForReady();
        }
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        foreach(var client in Clients)
        {
            client.ShowResponseForReject();
            client.gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    public override void UpdateClientList()
    {
        for(int i = 0; i < Clients.Count; ++i)
        {
            Clients[i].transform.position = Slots[i].position;
            Clients[i].transform.SetParent(Slots[i]);
        }
    }

    public override void ReturnClient(AbstractGameClient client)
    {
        if(GhostIndex != -1)
        {
            client.transform.position = Slots[GhostIndex].position;
        }
    }
}
