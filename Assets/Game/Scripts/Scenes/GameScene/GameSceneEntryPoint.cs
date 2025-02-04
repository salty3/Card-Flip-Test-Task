﻿using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.ApplicationCore;
using Game.Scripts.ApplicationCore.ApplicationInitialization;
using Game.Scripts.LevelsSystem;
using Game.Scripts.LevelsSystem.Levels;
using Game.Scripts.Scenes.GameScene.Behaviour;
using Game.Scripts.Scenes.GameScene.Behaviour.States;
using Game.Scripts.TimerSystem;
using Tools.Runtime;
using Tools.SceneManagement.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.Scenes.GameScene
{
    public class GameSceneEntryPoint : CachedMonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] private SceneContext _sceneContext;
        
        //Kinda workaround
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Button _backButton;

        private Timer _timer;
        private ILevelsService _levelsService;
        private IReadOnlyLevelEntity _levelEntity;
        
        [Inject]
        private void Construct(Timer timer, ILevelsService levelsService, IReadOnlyLevelEntity levelEntity)
        {
            _timer = timer;
            _levelsService = levelsService;
            _levelEntity = levelEntity;
        }
        
        //Can be used for more control over scene initialization process, heavy loadings etc.
        //Loading screen hides only after this method is completed.
        public UniTask OnSceneOpen(IProgress<float> progress)
        {
            _sceneContext.ParentContractNames = new[] { nameof(ApplicationInitializer) };
            _sceneContext.Run();
            _sceneContext.Container.Resolve<GameplayLoopStateManager>().SwitchToState<PreparationPhaseState>();

            
            var levelData = _levelsService.GetLevelData(_levelEntity.LevelIndex);
            _backgroundImage.sprite = levelData.LevelBackground;
            
            _backButton.onClick.AddListener(ReturnToMenu);
            
            _timer.Updated += UpdateTimer;
            
            progress.Report(1f);
            return UniTask.CompletedTask;
        }
        
        private void ReturnToMenu()
        {
            var sceneController = _sceneContext.Container.Resolve<SceneController>();
            var sceneReferences = _sceneContext.Container.Resolve<SceneReferences>();

            var builder = sceneReferences.MainMenu.LoadScene()
                .WithLoadingScreen(sceneReferences.Loading)
                .WithMode(LoadSceneMode.Additive)
                .WithClosing(sceneReferences.GameScene);
            
            sceneController.LoadAsync(builder).Forget();
        }

        private void UpdateTimer()
        {
            _levelsService.UpdateElapsedTime(_levelEntity.LevelIndex, _timer.ElapsedTime);
        }
        
        private void OnDestroy()
        {
            _timer.Dispose();
        }
    }
}