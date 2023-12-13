using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Enums;

public class DungeonGenerator : MonoBehaviour
{
    #region Keys
    protected const string _horizontalCorridorKey = "HorizontalCorridor";
    protected const string _verticalCorridorKey = "VerticalCorridor";
    private const string _mapPrefabsPath = "Maps/Fantasy/";
    #endregion

    #region Room Prefabs
    protected List<GameObject> _rooms = new List<GameObject>();
    [SerializeField] protected GameObject _spawnRoom;
    protected GameObject _horizontalCorridor;
    protected GameObject _verticalCorridor;
    #endregion

    #region Variables
    protected int _roomCount = 1;
    protected int _maxRoom = 30;
    protected int _corridorCount = 0;
    protected int _line = 0;
    protected int _failCount = 0;
    protected List<Vector3> _roomPositions = new List<Vector3>();
    [SerializeField] private float _roomDistance;
    #endregion

    protected virtual void Awake()
    {
        _horizontalCorridor = ResourceManager.Instance.LoadPrefab<GameObject>(_mapPrefabsPath + _horizontalCorridorKey, _horizontalCorridorKey);
        _verticalCorridor = ResourceManager.Instance.LoadPrefab<GameObject>(_mapPrefabsPath +  _verticalCorridorKey, _verticalCorridorKey);
        for (int i = 1; i <= 5; i++)
        {
            _rooms.Add(ResourceManager.Instance.LoadPrefab<GameObject>((_mapPrefabsPath + i),$"Fantasy_{i}"));
        }

        ConnectRoom(_spawnRoom.GetComponent<RoomTemplate>().Doors[0].GetComponent<DoorData>(), _horizontalCorridor);
        _roomCount = 0;
        ConnectRoom(_spawnRoom.GetComponent<RoomTemplate>().Doors[1].GetComponent<DoorData>(), _horizontalCorridor);
    }

    protected virtual void ConnectRoom(DoorData exitDoor, GameObject to)
    {
        if (_roomCount > _maxRoom) return;
        DoorType target = DoorType.Bottom;
        switch(exitDoor.DoorType)
        {
            case DoorType.Left:
                target = DoorType.Right; break;
            case DoorType.Right:
                target = DoorType.Left; break;
            case DoorType.Top:
                target = DoorType.Bottom; break;
            case DoorType.Bottom:
                target = DoorType.Top; break;
        }

        RoomTemplate targetRoomTemplate = to.GetComponent<RoomTemplate>();

        DoorData entranceDoor = SelectEntrance(target, targetRoomTemplate.Doors);
        if (entranceDoor == null)
        {
            TryCreateRoom(exitDoor);
            return;
        }

        GameObject currentRoom = CreateRoom(entranceDoor, exitDoor, to);
        
        if (currentRoom != null)
        {
            RoomTemplate currentRoomTemplate = currentRoom.GetComponent<RoomTemplate>();
            if (!entranceDoor.IsCorridor)
                _corridorCount = 0;
            foreach (GameObject door in currentRoomTemplate.Doors)
            {
                DoorData data = door.GetComponent<DoorData>();
                if (data.DoorType == target)
                    data.IsConnected = true;
            }


            if (currentRoomTemplate.Type != RoomType.HorizontalCorridor && currentRoomTemplate.Type != RoomType.VerticalCorridor)
            {
                _roomCount++;
                _line++;
            }

            if (currentRoomTemplate.Doors.Length > 1)
            {
                GameObject[] doors = currentRoomTemplate.Doors.OrderBy(x => Random.Range(-4, 4)).ToArray();
                foreach (GameObject door in doors)
                {
                    TryCreateRoom(door);
                }
            }
        }
        else if (_failCount < 50)
        {
            _failCount++;
            TryCreateRoom(exitDoor);
            return;
        }
        else return;
        
    }

    protected DoorData SelectEntrance(DoorType target, GameObject[] doors)
    {
        foreach (GameObject door in doors)
        {
            DoorData currentDoor = door.GetComponent<DoorData>();
            if (currentDoor.DoorType == target)
            {
                return currentDoor;
            }   
        }
        return null;
    }

    protected void TryCreateRoom(GameObject door)
    {
        int roomIndex = Random.Range(0, 5);
        if (!door.GetComponent<DoorData>().IsConnected && roomIndex < _rooms.Count && door.GetComponent<DoorData>().IsCorridor && _corridorCount > 5)
        {
            ConnectRoom(door.GetComponent<DoorData>(), _rooms[roomIndex]);
        }
        else if (!door.GetComponent<DoorData>().IsConnected)
        {
            _corridorCount++;
            if (door.GetComponent<DoorData>().DoorType == DoorType.Top || door.GetComponent<DoorData>().DoorType == DoorType.Bottom)
                ConnectRoom(door.GetComponent<DoorData>(), _verticalCorridor);
            else if (door.GetComponent<DoorData>().DoorType == DoorType.Left || door.GetComponent<DoorData>().DoorType == DoorType.Right)
                ConnectRoom(door.GetComponent<DoorData>(), _horizontalCorridor);  
        }
    }

    protected void TryCreateRoom(DoorData door)
    {
        int roomIndex = Random.Range(0, 5);
        if (!door.IsConnected && roomIndex < _rooms.Count && door.IsCorridor && _corridorCount > 5)
        {
            ConnectRoom(door, _rooms[roomIndex]);
        }
        else if (!door.IsConnected)
        {
            if(door.DoorType == DoorType.Top || door.DoorType == DoorType.Bottom)
                ConnectRoom(door, _verticalCorridor);
            else ConnectRoom(door, _horizontalCorridor);
        }
    }

    protected GameObject CreateRoom(DoorData entranceDoor, DoorData exitDoor, GameObject to)
    {
        Vector3 spawnPos = exitDoor.transform.position - entranceDoor.PivotDiff;
        
        if (CheckSpawnableRoom(spawnPos, to.gameObject.GetComponent<RoomTemplate>()))
        {
            GameObject room = Instantiate(to, spawnPos, Quaternion.identity);
            exitDoor.IsConnected = true;
            _roomPositions.Add(spawnPos);
            return room;
        }
        else return null;
    }

    protected bool CheckSpawnableRoom(Vector3 spawnPos, RoomTemplate roomInfo)
    {
        foreach (Vector3 position in _roomPositions)
        {
            if (roomInfo.Type == RoomType.HorizontalCorridor || roomInfo.Type == RoomType.VerticalCorridor)
            {
                if (((spawnPos - position).sqrMagnitude < 10 * roomInfo.Width * roomInfo.Width) || ((spawnPos - position).sqrMagnitude < 10 * roomInfo.Height * roomInfo.Height)) return false;
            }
            if (((spawnPos - position).sqrMagnitude < _roomDistance * roomInfo.Width * roomInfo.Width) || ((spawnPos - position).sqrMagnitude < _roomDistance * roomInfo.Height * roomInfo.Height)) return false;
        }
        return true;
    }
}
