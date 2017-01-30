using Ketsu.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ketsu.Game
{
    public class GameManager : MonoBehaviour
    {
        public string StartingMap;

        MapManager Map;

        void Awake()
        {

        }

        void Start()
        {
            MapManager.Instance.LoadMap(StartingMap);
        }

        void Update()
        {

        }
    }
}