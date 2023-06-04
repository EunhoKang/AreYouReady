using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaitingRoom : RaidRoom
{
    public override bool TryJoinClient(AbstractGameClient client)
    {
        if(Clients.Count < MaxClientCount)
        {
            Clients.Add(client);
            client.HostToRoom(this);
            UpdateClientList();
            return true;
        }
        else
        {
            return false;
        }
    }
}
