using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
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

    public enum InputState
    {
        None,
        Drag
    };
    private Vector2 InputPosition = Vector2.zero;
    private InputState CurrentState = InputState.None;
    private Vector2 DragOffset;
    private AbstractGameClient DraggedObject;
    void Update()
    {
        InputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (CurrentState == InputState.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectClient(RaycastFromWorldView<AbstractGameClient>());
            }
        }
        else if (CurrentState == InputState.Drag)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ReleaseClient(RaycastFromWorldView<AbstractRoom>());
                CurrentState = InputState.None;
                DraggedObject.gameObject.layer = LayerMask.NameToLayer("Default");
                DraggedObject = null;
            }
            else
            {
                DraggedObject.transform.position = new Vector3(InputPosition.x + DragOffset.x, InputPosition.y + DragOffset.y, DraggedObject.transform.position.z);
            }
        }
    }

    private T RaycastFromWorldView<T>()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, int.MaxValue, ~LayerMask.GetMask("Selected"));
        if (hit.collider == null)
        {
            return default(T);
        }
        return hit.collider.GetComponent<T>();
    }

    public void SelectClient(AbstractGameClient client)
    {
        if (client == default(AbstractGameClient) || client.CanMove == false)
        {
            return;
        }
        DraggedObject = client;
        DraggedObject.SelectedByServer();
        DraggedObject.gameObject.layer = LayerMask.NameToLayer("Selected");
        DragOffset = new Vector2(DraggedObject.transform.position.x - InputPosition.x, DraggedObject.transform.position.y - InputPosition.y);
        CurrentState = InputState.Drag;
    }

    public void ReleaseClient(AbstractRoom room)
    {
        if (room == default(AbstractRoom))
        {
            DraggedObject.ReturnToPreviousRoom();
            return;
        }
        if (!room.TryJoinClient(DraggedObject))
        {
            DraggedObject.ReturnToPreviousRoom();
        }
    }
}
