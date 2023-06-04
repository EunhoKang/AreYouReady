using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;

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
    public TextMeshProUGUI InstructionText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;
    public TextMeshProUGUI WinText;
    public GameObject WinObject;
    private int score = 0;
    private int timeleft = 0;
    private List<AbstractRoom> Rooms = new List<AbstractRoom>();
    private static System.Random rng = new System.Random();

    public void UIClicked()
    {
        Time.timeScale = 1f;
    }

    public async void BuildSchedule(ModeConfiguration modeConfiguration)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        InstructionText.text = modeConfiguration.Instruction;
        ScoreText.text = score.ToString();
        timeleft = modeConfiguration.LastingTime;
        TimeText.text = timeleft.ToString();
        
        for(int i = 0; i < RoomSpawnArea.Length; ++i)
        {
            Rooms.Add(null);
        }
        
        foreach (var room in modeConfiguration.RoomFrequency)
        {
            if(room.StartingCount > 0)
            {
                for(int i = 0; i < room.StartingCount; ++i)
                {
                    OpenRoom(0, room.room);
                }
            }
            else
            {
                float time = 0f;
                while(time < modeConfiguration.LastingTime)
                {
                    time += 60f / room.EncounterPerMinute;
                    OpenRoom(Mathf.Clamp(time + UnityEngine.Random.Range(-1f, 1f), 0, float.MaxValue), room.room);
                }
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

        Time.timeScale = 0f;
        TimeGoes();
    }
    public async void TimeGoes()
    {
        while(timeleft > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            TimeText.text = (--timeleft).ToString();
        }
        GameWin();
    }

    public async void OpenRoom(float time, AbstractRoom roomPrefab)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        int[] randomIndex = Enumerable.Range(0, RoomSpawnArea.Length).OrderBy(a => rng.Next()).ToArray();
        for(int i = 0; i < Rooms.Count; ++i)
        {
            if(Rooms[randomIndex[i]] == null)
            {
                Rooms[randomIndex[i]] = Instantiate(roomPrefab, RoomSpawnArea[randomIndex[i]].position, Quaternion.identity);
                return;
            }
        }
    }

    public void CloseRoom(AbstractRoom room)
    {
        Rooms[Rooms.IndexOf(room)] = null;
        score += room.clearScore;
        ScoreText.text = score.ToString();
    }

    public async void ClientEnter(float time, AbstractGameClient clientPrefab)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        Vector3 newPosition = new Vector3(0, -999, 0);
        AbstractGameClient client = Instantiate(clientPrefab, newPosition, Quaternion.identity);
        List<AbstractRoom> randomRooms = Rooms.OrderBy(a => rng.Next()).ToList();
        foreach(var room in randomRooms)
        {
            if(room != null && room.CanAddedByManager && room.TryJoinClient(client))
            {
                return;
            }
        }
        GameOver();
    }

    public async void GameWin()
    {
        WinObject.SetActive(true);
        WinText.text = "You Win!";
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        Destroy(this.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public async void GameOver()
    {
        WinObject.SetActive(true);
        WinText.text = "Game Over!";
        await UniTask.Delay(TimeSpan.FromSeconds(3f)); 
        Destroy(this.gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
