using UnityEngine;


namespace Tenacity.Navigation.Data
{
    /// <summary>
    /// Represents point belonging to a certain group.
    /// </summary>
    public enum PointType
    {
        Undefined,
        Path,
        Suspicion,
        Target
    }

    /// <summary>
    /// Action, that will be performed on reaching the point.
    /// </summary>
    public enum PointAction
    {
        Undefined,
        Continue,
        Stop,
        Attack
    }


    /// <summary>
    /// Represents point of some type in scene and action,
    /// that need to perform on reaching destination.
    /// </summary>
    [System.Serializable]
    public class NavigationPoint
    {
        /// <summary>
        /// Represents point belonging to a certain group.
        /// </summary>
        public PointType Type = PointType.Path;
        /// <summary>
        /// Action, that will be performed on reaching the point.
        /// </summary>
        public PointAction Action;
        [Range(0, byte.MaxValue)] public ushort Priority;
        public IntermediatePoint Point;
        /// <summary>
        /// The delay time of the transition to the next step.
        /// </summary>
        [Range(0, ushort.MaxValue)] public float TransferDelay;

        /// <summary>
        /// Transform of attached intermediate point.
        /// </summary>
        public Transform Transform
        {
            get { return Point?.Transform ?? Point.transform; }
        }


        /// <summary>
        /// Destroys gameObject of attached intermediate point.
        /// <remarks>
        /// Only suspicion or other, which are used only by
        /// one instance of navigation class.
        /// </remarks>
        /// </summary>
        public void DestroyPoint()
        {
            if ((Point) &&
                (Type == PointType.Suspicion))
                Object.Destroy(Point.gameObject);
        }

    }
}
