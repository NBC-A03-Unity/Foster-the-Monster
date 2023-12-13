using UnityEngine;

public class FrozenGenerator : DungeonGenerator
{
    private const string _mapPrefabsPath = "Maps/Frozen/";


    protected override void Awake()
    {
        _horizontalCorridor = ResourceManager.Instance.LoadPrefab<GameObject>(_mapPrefabsPath + _horizontalCorridorKey, _horizontalCorridorKey);
        _verticalCorridor = ResourceManager.Instance.LoadPrefab<GameObject>(_mapPrefabsPath + _verticalCorridorKey, _verticalCorridorKey);
        for (int i = 1; i <= 4; i++)
        {
            _rooms.Add(ResourceManager.Instance.LoadPrefab<GameObject>((_mapPrefabsPath + i), $"Frozen_{i}"));
        }

        ConnectRoom(_spawnRoom.GetComponent<RoomTemplate>().Doors[0].GetComponent<DoorData>(), _horizontalCorridor);
        _roomCount = 0;
        ConnectRoom(_spawnRoom.GetComponent<RoomTemplate>().Doors[1].GetComponent<DoorData>(), _horizontalCorridor);
    }

    protected override void ConnectRoom(DoorData exitDoor, GameObject to)
    {
        if (_line > 2)
        {
            _line = 0;
            return;
        }
        base.ConnectRoom(exitDoor, to);
    }
}
