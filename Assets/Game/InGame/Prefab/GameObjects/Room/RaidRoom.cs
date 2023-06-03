using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidRoom : AbstractRoom
{
    public int MaxClientCount = 4;
    public List<Transform> Slots;

    public override bool TryCloseRoom()
    {
        bool flag = true;
        foreach(var client in Clients)
        {
            flag &= client.ReadyForGame();
        }
        if(!flag)
        {
            foreach(var client in Clients)
            {
                client.RejectByServer();
            }
        }
        return flag;
    }

    public override bool TryJoinClient(AbstractGameClient client)
    {
        if(Clients.Count < MaxClientCount)
        {
            Clients.Add(client);
            UpdateClientList();
            if(Clients.Count == MaxClientCount && TryCloseRoom())
            {
                CloseRoom();
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

    public override void ReturnClient(AbstractGameClient client)
    {
        if(GhostIndex != -1)
        {
            client.transform.position = Slots[GhostIndex].position;
        }
    }
}
