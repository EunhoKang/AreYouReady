using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GameMode", menuName = "ScriptableObjects/GameMode", order = 1)]
public class ModeConfiguration : ScriptableObject
{
    [TextArea]
    public string Instruction;
    public List<GameClientData> GameClientFrequency;
    public List<RoomData> RoomFrequency;
    public int LastingTime;    
}

[Serializable]
public class RoomData {
    public AbstractRoom room;
    public int EncounterPerMinute;
    public int StartingCount;
}

[Serializable]
public class GameClientData {
    public AbstractGameClient client;
    public int EncounterPerMinute;
}