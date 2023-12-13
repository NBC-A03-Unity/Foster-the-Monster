using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerStatus Status;
    public PlayerController PlayerController;
    public List<Transform> RespawnPositions;

    protected override void Awake()
    {
        RespawnPositions = new List<Transform>(100);
        
    }
}
