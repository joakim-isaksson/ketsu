using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class GameManager : MonoBehaviour
    {
        public string StartingMap;
        public int StartingKetsuPower;

        void Awake()
        {

        }

        void Start()
        {
            MapManager.Instance.LoadMap(StartingMap);
            Character.KetsuPower += StartingKetsuPower;
        }

        void Update()
        {

        }
    }
}