using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusManager : MonoBehaviour
{
    [SerializeField] private int walk_Speed = 8;             // 걷기 속도
    [SerializeField] private int run_Speed = 10;              // 질주 속도

    public int Walk_Speed
    {
        get => walk_Speed;
        set
        {
            int changeSpeed = value - walk_Speed;
            walk_Speed = value;

            run_Speed += changeSpeed;
        }
    }

    public int Run_Speed
    {
        get => run_Speed;
        set
        {
            run_Speed = value;
        }
    }
}
