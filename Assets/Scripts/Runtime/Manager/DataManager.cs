﻿using System;
using System.Numerics;
using UnityEngine;

namespace Runtime.Manager
{
    [Serializable]
    public class GameData
    {
        public int chapter;
        public int stage;
        public bool pacmomIsCleared;
    }

    public class DataManager
    {
        private GameData _gameData = new();
        private GameData SaveData { get { return _gameData; } set { _gameData = value; } }
        
        public int Chapter { get { return _gameData.chapter; } set { _gameData.chapter = value; } }
        public int Stage { get { return _gameData.stage; } set { _gameData.stage = value; } }
        public bool PacmomIsCleared { get { return _gameData.pacmomIsCleared; } set { _gameData.pacmomIsCleared = value; } }

        public void Init()
        {
            // 시작 데이터 Init
            
        }

        private string _path = Application.persistentDataPath + "/SaveData.json";
        public void SaveGame()
        {
            string jsonStr = JsonUtility.ToJson(Managers.Data.SaveData);
            System.IO.File.WriteAllText(_path, jsonStr);
            Debug.Log($"Save Game Completed : {_path}");
        }
        
        public bool LoadGame()
        {
            if (System.IO.File.Exists(_path) == false)
            {
                return false;
            }

            string jsonStr = System.IO.File.ReadAllText(_path);
            GameData data = JsonUtility.FromJson<GameData>(jsonStr);
            if (data == null)
            {
                return false;
            }
            
            Managers.Data.SaveData = data;
            Debug.Log($"Load Game Completed : {_path}");
            return true;
        }

    }
}