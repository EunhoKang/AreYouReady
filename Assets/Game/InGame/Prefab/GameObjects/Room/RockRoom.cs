using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RockRoom : AbstractRoom
{
    public AudioClip clip;
    public int ClickCount = 24;
    public TextMeshProUGUI ClickCountText;
    void Start()
    {
        ClickCountText.text = ClickCount.ToString();
    }
    public override void ReturnClient(AbstractGameClient client)
    {
        return;
    }

    public override bool TryJoinClient(AbstractGameClient client)
    {
        return false;
    }

    public override void UpdateClientList()
    {
        return;
    }

    public void ClickToBreak()
    {
        ClickCountText.text = (--ClickCount).ToString();
        if(ClickCount == 0)
        {
            SoundManager.instance.PlaySound(clip);
            CloseRoom();
        }
    }
}
