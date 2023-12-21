using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using Assets.Scripts.Roguelike.Map;

using UnityEngine;

public class MapInputController : MonoBehaviour
{

    private ControlInputHandler controlInputHandler;
    private Minimap minimap;
    private Map map;

    public GameObject Level;

    public GameObject Minimap;

    public GameObject Map;

    void Start()
    {
        controlInputHandler = Level.GetComponent<ControlInputHandler>();
        minimap = Minimap.GetComponent<Minimap>();
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
                minimap.MoveTopDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                minimap.MoveRightDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                minimap.MoveDownDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                minimap.MoveLeftDoorFunc.Invoke();
            }

        };
#endif
    }
}
