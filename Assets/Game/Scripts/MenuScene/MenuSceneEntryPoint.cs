﻿using System;
using Cysharp.Threading.Tasks;
using Tools.Runtime;
using Tools.SceneManagement.Runtime;
using UnityEngine;
using Zenject;

namespace Game.Scripts.MenuScene
{
    public class MenuSceneEntryPoint : CachedMonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] private SceneContext _sceneContext;
        
        public UniTask OnSceneOpen(IProgress<float> progress)
        {
            _sceneContext.ParentContractNames = new[] { nameof(ApplicationInitializer) };
            _sceneContext.Run();

            var stateManager = _sceneContext.Container.Resolve<MenuStateManager>();
            stateManager.SwitchToState<MainMenuState>();
            
            progress.Report(1f);
            return UniTask.CompletedTask;
        }
    }
}