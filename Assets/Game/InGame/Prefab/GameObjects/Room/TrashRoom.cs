using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TrashRoom : AbstractRoom
{
    public int MaxClientCount = 1;
    public List<Transform> Slots;
    public SpriteRenderer SpriteRenderer;
    public Sprite ReadySprite;
    public override void ReturnClient(AbstractGameClient client)
    {
        return;
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
                ResponseForReady();
            }
            return true;
        }
        else
        {
            return false;
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

    public async void ResponseForReady()
    {
        foreach(var client in Clients)
        {
            client.CanMove = false;
        }
        SpriteRenderer.sprite = ReadySprite;
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        foreach(var client in Clients)
        {
            client.CanMove = true;
        }
        CloseRoom();
    }
}
