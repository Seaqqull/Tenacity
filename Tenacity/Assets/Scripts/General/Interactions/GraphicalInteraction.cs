using System;
using UnityEngine;


namespace Tenacity.General.Interactions
{
    public class GraphicalInteraction : ConditionalInteraction
    {
        [Header("Ui")]
        [SerializeField] private GameObject _objectToShow;


        protected override void Start()
        {
            base.Start();
            
            NeedToShow(_isInteractable);
        }

        public override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            NeedToShow(true);
        }

        public override void OnTriggerExit(Collider collision)
        {
            base.OnTriggerExit(collision);

            NeedToShow(false);
        }

        public void OnDestroy()
        {
            Destroy(_objectToShow);
        }


        protected virtual void NeedToShow(bool flag)
        {
            if (_objectToShow == null) return;

            _objectToShow.SetActive(flag);
        }
    }
}