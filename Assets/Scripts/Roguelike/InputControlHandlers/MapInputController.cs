using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Roguelike.Map;

using UnityEngine;

public class MapInputController : MonoBehaviour
{

    private ControlInputHandler controlInputHandler;
    private Map map;

    public GameObject Level;

    public GameObject Minimap;

    public GameObject Map;

    void Start()
    {
        controlInputHandler = Level.GetComponent<ControlInputHandler>();
        map = Map.GetComponent<Map>();

        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.M))
            {
                map.OpenMapFunc.Invoke();
            }

        };

#if UNITY_EDITOR
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                RoomMovementController.Instance.MoveTopDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                RoomMovementController.Instance.MoveRightDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                RoomMovementController.Instance.MoveDownDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                RoomMovementController.Instance.MoveLeftDoorFunc.Invoke();
            }

        };
#endif
    }
}
