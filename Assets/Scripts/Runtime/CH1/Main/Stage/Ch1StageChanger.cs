using Runtime.InGameSystem;
using Runtime.Interface;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.CH1.Main.Stage
{
    public class Ch1StageChanger : IStageChanger
    {
        private readonly GameObject _player;
        private readonly IStage[] _stages;
        private readonly FadeController _fadeController;
        private IStage _currentStage;
        
        public Action OnStageStart { get; set; }
        public Action OnStageEnd { get; set; }
        
        public Ch1StageChanger(GameObject player, IStage stage, IStage[] stages, FadeController fadeController)
        {
            _player = player;
            _currentStage = stage;
            _stages = stages;
            _fadeController = fadeController;
        }

        public async Task SwitchStage(int moveStageNumber, Vector2 spawnPosition)
        {
            OnStageStart?.Invoke();
            
            if (_fadeController is not null)
                await _fadeController.FadeOut();
            
            _currentStage?.Disable();
            
            _currentStage = _stages[moveStageNumber - 1];
            
            _player.transform.position = spawnPosition;
            
            _currentStage.Enable();
            _currentStage.SetSetting();
            
            if (_fadeController is not null)
                await _fadeController.FadeIn();
            
            OnStageEnd?.Invoke();

            return;
        }
    }
}