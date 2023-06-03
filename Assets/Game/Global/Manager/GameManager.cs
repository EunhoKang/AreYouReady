using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public Transform[] RoomSpawnArea;
    public ModeConfiguration modeConfiguration;
    private List<AbstractRoom> Rooms = new List<AbstractRoom>();
    private List<AbstractGameClient> Clients = new List<AbstractGameClient>();
    private static System.Random rng = new System.Random();

    void Start()
    {
        //Use for Debug
        BuildSchedule(modeConfiguration);
    }

    public void BuildSchedule(ModeConfiguration modeConfiguration)
    {
        for(int i = 0; i < RoomSpawnArea.Length; ++i)
        {
            Rooms.Add(null);
        }
        foreach (var room in modeConfiguration.RoomFrequency)
        {
            float time = 0f;
            while(time < modeConfiguration.LastingTime)
            {
                time += 60f / room.EncounterPerMinute;
                OpenRoom(Mathf.Clamp(time + UnityEngine.Random.Range(-1f, 1f), 0, float.MaxValue), room.room);
            }
        }

        foreach (var client in modeConfiguration.GameClientFrequency)
        {
            float time = 0f;
            while(time < modeConfiguration.LastingTime)
            {
                time += 60f / client.EncounterPerMinute;
                ClientEnter(Mathf.Clamp(time + UnityEngine.Random.Range(-1f, 1f), 0, float.MaxValue), client.client);
            }
        }
    }

    public async void OpenRoom(float time, AbstractRoom roomPrefab)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        for(int i = 0; i < Rooms.Count; ++i)
        {
            if(Rooms[i] == null)
            {
                Rooms[i] = Instantiate(roomPrefab, RoomSpawnArea[i].position, Quaternion.identity);
                return;
            }
        }
        //Disadvantage?
    }

    public void CloseRoom(AbstractRoom room)
    {
        Rooms[Rooms.IndexOf(room)] = null;
    }

    public async void ClientEnter(float time, AbstractGameClient clientPrefab)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        Vector3 newPosition = new Vector3(0, -999, 0);
        AbstractGameClient client = Instantiate(clientPrefab, newPosition, Quaternion.identity);
        List<AbstractRoom> randomRooms = Rooms.OrderBy(a => rng.Next()).ToList();
        foreach(var room in randomRooms)
        {
            if(room != null && room.TryJoinClient(client))
            {
                return;
            }
        }
        GameOver();
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }
}
