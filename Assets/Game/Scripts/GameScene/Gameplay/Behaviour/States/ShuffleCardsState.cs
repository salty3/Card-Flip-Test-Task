﻿using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Gameplay;
using Zenject;

namespace Game.Scripts.GameScene.Gameplay.Behaviour.States
{
    public class ShuffleCardsState : GameState
    {
        private readonly CardsFieldPresenter _cardsFieldPresenter;
        private readonly GameplayLoopStateManager _gameplayLoopStateManager;
        
        [Inject]
        public ShuffleCardsState(CardsFieldPresenter cardsFieldPresenter, GameplayLoopStateManager gameplayLoopStateManager)
        {
            _cardsFieldPresenter = cardsFieldPresenter;
            _gameplayLoopStateManager = gameplayLoopStateManager;
        }

        public override void Initialize()
        {
            _cardsFieldPresenter.BlockInteraction();
            ShuffleCards().Forget();
        }

        private async UniTask ShuffleCards()
        {
            await _cardsFieldPresenter.Shuffle();
            _gameplayLoopStateManager.SwitchToState<ShowCardsState>();
        }

        public override void Dispose()
        {
            _cardsFieldPresenter.UnblockInteraction();
        }
    }
}