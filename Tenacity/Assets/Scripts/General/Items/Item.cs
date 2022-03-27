using Tenacity.Utility.Interfaces;
using UnityEngine;
using System;


namespace Tenacity.General.Items
{
    public abstract class Item : Base.BaseMono, Utility.Interfaces.IPickable, IEquatable<Item>
    {
        private static int _idCounter;


        [SerializeField] protected ItemType _type;

        protected bool _isPickable;
        protected int _id;

        public ItemType Type
        {
            get => _type;
        }

        public int Id
        {
            get { return this._id; }
        }


        protected override void Awake()
        {
            base.Awake();

            _id = _idCounter++;
        }


        public virtual bool SetPickable(bool flag)
        {
            _isPickable = flag;
            return true;
        }


        public override int GetHashCode()
        {
            return this._id.GetHashCode();
        }

        public bool Equals(Item obj)
        {
            return (obj != null) && (this._id == obj._id);
        }

        public override bool Equals(System.Object obj)
        {
            if ((obj == null) ||
                !(this.GetType().Equals(obj.GetType())))
                return false;

            return this.Equals(obj as Item);
        }
    }
}