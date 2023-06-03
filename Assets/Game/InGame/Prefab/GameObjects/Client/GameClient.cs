using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClient : AbstractGameClient
{
    public override bool ReadyForGame()
    {
        return true;
    }

    public override void RejectByServer()
    {
        throw new System.NotImplementedException();
    }
}
