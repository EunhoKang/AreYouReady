using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameMode", menuName = "ScriptableObjects/GameMode", order = 1)]
public class ModeConfiguration : ScriptableObject
{
    public List<string> Instructions;
    public List<int> Challenges;
    public List<GameClientFrequencyData> GameClientFrequency;
    public List<RoomFrequencyData> RoomFrequency;
    public int LastingTime;    
}

[Serializable]
public struct RoomFrequencyData {
    public AbstractRoom room;
    public int EncounterPerMinute;
}

[Serializable]
public struct GameClientFrequencyData {
    public AbstractGameClient client;
    public int EncounterPerMinute;
}