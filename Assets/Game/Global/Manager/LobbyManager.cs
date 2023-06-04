using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public List<ModeConfiguration> ModeConfigurations;

    public void MoveCameraToTransform(Transform target)
    {
        Camera.main.transform.position = new Vector3(target.position.x, target.position.y, Camera.main.transform.position.z);
    }

    public async void StartGame(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        await UniTask.DelayFrame(1);
        GameManager.instance.BuildSchedule(ModeConfigurations[index]);
    }
}
