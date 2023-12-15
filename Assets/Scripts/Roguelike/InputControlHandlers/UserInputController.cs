using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers.Roguelike;
using Assets.Scripts.Helpers.Roguelike.Minimap;
using Assets.Scripts.Roguelike.Map;

using UnityEngine;

public class UserInputController : MonoBehaviour
{

    private ControlInputHandler controlInputHandler;
    private Minimap minimap;
    private Map map;

    public GameObject Level;

    public GameObject Minimap;

    public GameObject Map;

    // Start is called before the first frame update
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
            if (Input.GetKeyUp(KeyCode.W))
            {
                minimap.MoveTopDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                minimap.MoveRightDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                minimap.MoveDownDoorFunc.Invoke();
            }

        };
        controlInputHandler.ControlActionList += delegate ()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                minimap.MoveLeftDoorFunc.Invoke();
            }

        };
#endif
    }
}
