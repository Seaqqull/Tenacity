using Tenacity.Battles.Controllers.CellModifiers;
using Tenacity.Battles.Views.Creatures;
using Tenacity.Battles.Data.Field;
using Tenacity.Battles.Views;
using Tenacity.Battles.Data;
using Tenacity.Base;
using UnityEngine;
using System;


namespace Tenacity.Battles.Controllers
{
    [Flags] public enum AttackSide { None, Center, Left, Right = 4, Front = 8, Back = 16 }
    [Serializable]
    public class AttackPosition
    {
        [field: SerializeField] public Transform Position { get; set; }
        [field: SerializeField] public AttackSide Side { get; set; }
    }
    
    
    public class CellController : BaseMono, ICell
    {
        [SerializeField] private Transform _view;
        [SerializeField] private Transform _placementParent;
        [Space]
        [SerializeField] private AttackPosition[] _attackSides;
        [SerializeField] private CellModifierSO[] _modifiers;

        private Action<CellController> _onClick;
        private InteractionView _selection;
        private CreatureView _creature;
        private GameObject _viewObject;

        public InteractionState SelectionState => _selection?.State ?? InteractionState.None;
        public ICellState State { get; private set; } = new EmptyCell();
        public Transform PlacementParent => _placementParent;
        public Vector2Int FieldPosition { get; set; }
        public ICreatureData CreatureData => (_creature != null) ? _creature.Data : null;
        public CreatureView Creature => _creature;
        public Transform View => _view; 


        // public void OnMouseDown()
        // {
        //     Debug.LogError("Cell clicked");
        //     _onClick?.Invoke(this);
        // }

        
        private void UpdateObject<T>(ref T from, T to)
        {
            if (from != null)
            {
                if (from is MonoBehaviour mono)
                    Destroy(mono.gameObject);
                else if (from is GameObject gameObject)
                    Destroy(gameObject);
            }
            from = to;
        }

        public void SwitchSelection(InteractionState state)
        {
            if (_selection != null)
                _selection.State = state;
        }

        public void ResetController(bool resetViewObject, bool resetCreature, bool resetOnClick, bool selectionObject, bool destroy = true)
        {
            if (resetViewObject)
            {
                if (destroy)
                    UpdateObject(ref _viewObject, null);
                else
                    _viewObject = null;
            }
            if (resetCreature)
            {
                if (destroy)
                    UpdateObject(ref _creature, null);
                else
                    _creature = null;
            }
            if (selectionObject)
            {
                if (destroy)
                    UpdateObject(ref _selection, null);
                else
                    _selection = null;
            }
            if (resetOnClick)
                _onClick = null;
        }
        
        public void UpdateController(ICellState state, GameObject viewObject = null, InteractionView selection = null, CreatureView creature = null, Action<CellController> onClick = null)
        {
            if (onClick != null)
                _onClick = onClick;
            if (viewObject != null)
            {
                UpdateObject(ref _viewObject, viewObject);
                var viewTransform = _viewObject.transform; 

                viewTransform.parent = _view;
                viewTransform.localPosition = Vector3.zero;
                viewTransform.localRotation = Quaternion.identity;
            }
            if (selection != null)
            {
                UpdateObject(ref _selection, selection);
                
                _selection.Transform.parent = _placementParent;
                _selection.Transform.localPosition = Vector3.zero;
                _selection.Transform.localRotation = Quaternion.identity;
            }
            if (creature != null)
            {
                UpdateObject(ref _creature, creature);
                
                _creature.Transform.parent = _placementParent;
                _creature.Transform.localPosition = Vector3.zero;
                _creature.Transform.localRotation = Quaternion.identity;
            }
            
            State = state;
            foreach (var modifier in _modifiers)
                modifier.ModifyCell(this);
        }
    }
}
