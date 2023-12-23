using System;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class RoomMovementController : MonoBehaviour
{
    public static RoomMovementController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<RoomMovementController>();
            }

            return _instance;
        }
    }

    public Action MoveRightDoorFunc;

    public Action MoveDownDoorFunc;

    public Action MoveLeftDoorFunc;

    public Action MoveTopDoorFunc;

    private NavMeshSurface _navMeshSurface;
    private static RoomMovementController _instance;

    private RoomMovementController() { }

    private void Start()
    {
        _instance = this;

        _navMeshSurface = GetComponent<NavMeshSurface>();

        MoveRightDoorFunc += _navMeshSurface.BuildNavMesh;
        MoveDownDoorFunc += _navMeshSurface.BuildNavMesh;
        MoveLeftDoorFunc += _navMeshSurface.BuildNavMesh;
        MoveTopDoorFunc += _navMeshSurface.BuildNavMesh;
    }
}
