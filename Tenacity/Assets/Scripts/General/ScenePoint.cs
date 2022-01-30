using UnityEngine;


namespace Tenacity.General
{
    public abstract class ScenePoint : Base.BaseMono
    {
#pragma warning disable 0649
        [SerializeField] protected float _size = 0.3f;
#pragma warning restore 0649

        /// <summary>
        /// Color of gizmo.
        /// </summary>
        public abstract Color Color
        {
            get;
        }


        protected void OnDrawGizmos()
        {
            Gizmos.color = Color;
            Gizmos.DrawWireSphere(transform.position, _size);
        }
    }
}