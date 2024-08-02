﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Scripts.Gameplay.Card;
using Tools.Runtime;
using UnityEngine;
using Utility.SLayout;

namespace Game.Scripts.Gameplay
{
    public class CardsFieldView : UIMonoBehaviour
    {
        [SerializeField] private Vector2 _referenceFieldSize;
        [SerializeField] private Vector2 _referenceCellSize;
        
        [SerializeField] private SGridLayoutGroup _gridParent;
        
        [SerializeField] private CardView _cardPrefab;
        
        [SerializeField] private RectTransform _completionBoard;
        [SerializeField] private RectTransform _completionMoveToPoint;

        public float GridMoveDuration => _gridParent.moveDuration;

        public void Initialize()
        {
            var sizeMultiplier = RectTransform.sizeDelta / _referenceFieldSize;
            var cellSize = _referenceCellSize * sizeMultiplier;
            _gridParent.cellSize = cellSize;
        }
        
        public CardView CreateCardView(CardData cardData, Sprite backIcon)
        {
            var cardView = Instantiate(_cardPrefab, _gridParent.transform);
            cardView.RectTransform.localPosition = Vector3.zero;
            cardView.SetFrontIcon(cardData.Icon);
            cardView.SetBackIcon(backIcon);
            cardView.SetAnimationTransforms(_completionBoard, _completionMoveToPoint);
            return cardView;
        }
    }
}