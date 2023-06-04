using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClientO : AbstractGameClient
{
    public SpriteRenderer SpriteRenderer;
    public Sprite ReadySprite;
    public Sprite NotReadySprite;
    public override bool ReadyForGame()
    {
        SpriteRenderer.sprite = ReadySprite;
        return true;
    }

    public override void ShowResponseForReady()
    {
        SpriteRenderer.sprite = ReadySprite;
    }

    public override void ShowResponseForReject()
    {
        SpriteRenderer.sprite = NotReadySprite;
    }
}
