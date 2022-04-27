using System.Collections.Generic;
using UnityEngine;


namespace Tenacity.Navigation
{
    public abstract class NavigationContainer : MonoBehaviour
    {
        [SerializeField] protected List<Data.NavigationPoint> _points =
            new List<Data.NavigationPoint>();

        /// <summary>
        /// Is next destination point random.
        /// </summary>
        [SerializeField] protected bool _isRandom;
        [SerializeField] protected int _startupIndex;
        [SerializeField] protected Color _lineColor = Color.green;

        protected IReadOnlyList<Data.NavigationPoint> _pointsRestricted;

        public IReadOnlyList<Data.NavigationPoint> Points
        {
            get
            {
                return this._pointsRestricted ??
                    (this._pointsRestricted = this._points);
            }
        }
        /// <summary>
        /// Destination navigation point.
        /// </summary>
        public Data.NavigationPoint DestinationPoint
        {
            get
            {
                if (Points.Count == 0)
                    throw new System.Exception("Can't get destination point, navigation is empty.");
                return Points[_previousIndex];
            }
        }
        /// <summary>
        /// Destination position.
        /// </summary>
        public Vector3 PointPosition
        {
            get
            {
                if (Points.Count == 0)
                    throw new System.Exception("Can't get destination position, navigation is empty.");
                return Points[_previousIndex].Transform.position;
            }
        }
        public int Length
        {
            get => Points.Count;
        }

        /// <summary>
        /// Next point index, that will be setted as destination,
        /// after reaching current destination point + time delay.
        /// </summary>
        protected int _destinationIndex;
        /// <summary>
        /// Current destination point.
        /// </summary>
        protected int _previousIndex = -1;


        protected virtual void Awake()
        {
            _destinationIndex = _startupIndex;
        }

        protected virtual void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (Points.Count == 0) return;

            Gizmos.color = _lineColor;

            int i;
            for (i = 0; i < Points.Count - 1; i++)
            {
                if ((Points[i].Point != null) &&
                   (Points[i + 1].Point != null))
                {
                    Gizmos.DrawLine(Points[i].Point.transform.position,
                        Points[i + 1].Point.transform.position);
                }
                else
                    throw new System.Exception(string.Format("Point reference {0} or {1} does not exist.", i, i + 1));
            }
#endif
        }


        /// <summary>
        /// Clears navigation and reset its destination point to zero.
        /// </summary>
        public void Clear()
        {
            _points.Clear();
            ResetToZero();
        }

        /// <summary>
        /// Resets destination point to zero.
        /// </summary>
        public void ResetToZero()
        {
            _destinationIndex = 0;
            _previousIndex = -1;
        }

        /// <summary>
        /// Removes navigation point.
        /// </summary>
        /// <param name="index">Positional index.</param>
        public void RemoveAt(int index)
        {
            if ((index < 0) ||
                (Points.Count == 0) ||
                (index >= Points.Count))
                throw new System.Exception(string.Format("No such {0} index to be removed.", index));

            Points[index].DestroyPoint();
            _points.RemoveAt(index);
        }

        /// <summary>
        /// Removes navigation point.
        /// </summary>
        /// <param name="index">Positional index.</param>
        /// <param name="relIndex">Relative index, updates after deletion.</param>
        public void RemoveAt(int index, ref int relIndex)
        {
            if ((index < 0) ||
                (Points.Count == 0) ||
                (index >= Points.Count))
                throw new System.Exception(string.Format("No such {0} index to be removed.", index));

            Points[index].DestroyPoint();
            _points.RemoveAt(index);

            if (index == relIndex)
                relIndex = -1;
            else if (index < relIndex)
                relIndex--;
        }

        /// <summary>
        /// Returns position of navigation point.
        /// </summary>
        /// <param name="index">Positional index.</param>
        /// <returns>Position in scene.</returns>
        public Vector3 GetPathPosition(int index)
        {
            if ((index < 0) ||
                (Points.Count == 0) ||
                (index >= Points.Count))
                throw new System.Exception(string.Format("Can't get {0} navigation position, no such navigation point.", index));

            return Points[index].Transform.position;
        }

        /// <summary>
        /// Returns navigation point.
        /// </summary>
        /// <param name="index">Positional index.</param>
        /// <returns>Navigation point.</returns>
        public Data.NavigationPoint GetPoint(int index)
        {
            if ((index < 0) ||
                (Points.Count == 0) ||
                (index >= Points.Count))
                throw new System.Exception(string.Format("Can't get {0} navigation point, no such navigation point.", index));

            return Points[index];
        }

        /// <summary>
        /// Search for the nearest navigation point.
        /// </summary>
        /// <param name="destination">Relative index of nearest navigation point, updates after finding nearest point.</param>
        /// <returns>Position in scene.</returns>
        public Vector3 GetNearestPosition(Vector3 position, ref int destination)
        {
            if (Points.Count == 0)
                throw new System.Exception("Can't get nearest point position, navigation is empty.");

            float distanceMin = int.MaxValue;
            float distanceTemp = 0.0f;

            for (int i = 0; i < Points.Count; i++)
            {
                distanceTemp = Vector3.Distance(position,
                    Points[i].Transform.position);
                if (distanceTemp < distanceMin)
                {
                    distanceMin = distanceTemp;
                    destination = i;
                }
            }

            _previousIndex = destination;
            _destinationIndex = (_previousIndex + 1) % Points.Count;

            return Points[destination].Transform.position;
        }

        /// <summary>
        /// Updates destination point.
        /// </summary>
        /// <param name="destinationIndex">Relative index of navigation point, updates after choosing new destination.</param>
        /// <returns>Position of next navigation point.</returns>
        public Vector3 GetNextPosition(ref int destinationIndex)
        {
            if (Points.Count == 0)
                throw new System.Exception("Can't get next point, navigation is empty.");

            Vector3 destination = Points[_destinationIndex].Transform.position;
            destinationIndex = _destinationIndex;

            _previousIndex = _destinationIndex;
            _destinationIndex = (_destinationIndex + 1) % Points.Count;

            return destination;
        }

        /// <summary>
        /// Returns random navigation point.
        /// </summary>
        /// <param name="destinationIndex">Relative index of navigation point, updates after choosing new destination.</param>
        /// <returns>Position in scene.</returns>
        public Vector3 GetRandomPosition(ref int destinationIndex)
        {
            if (Points.Count == 0)
                throw new System.Exception("Can't get random point, navigation is empty.");

            int randomPoint;

            do
            {
                randomPoint = Random.Range(0, Points.Count);
            }
            while ((randomPoint == _previousIndex) ||
                   (randomPoint == _destinationIndex));


            Vector3 destination = Points[_destinationIndex].Transform.position;
            destinationIndex = _destinationIndex;
            //_prePreviousPoint = _previousIndex;
            _previousIndex = _destinationIndex;
            _destinationIndex = randomPoint;

            return destination;
        }

        /// <summary>
        /// Adds navigation point.
        /// </summary>
        /// <param name="point">Point to add.</param>
        public void Add(Data.NavigationPoint point)
        {
            _points.Add(point);
        }

        /// <summary>
        /// Adds list of navigation point.
        /// </summary>
        /// <param name="points">List of navigation point.</param>
        public void Add(List<Data.NavigationPoint> points)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                _points.Add(points[i]);
            }
            points.Clear();
        }

        /// <summary>
        /// Adds navigation point.
        /// </summary>
        /// <param name="point">Point to add.</param>
        /// <param name="index">Relative index of added navigation point.</param>
        public void Add(Data.NavigationPoint point, ref int index)
        {
            index = Points.Count;
            _points.Add(point);
        }


        /// <summary>
        /// Return next or random (based on container settings) navigation point.
        /// </summary>
        /// <param name="destinationIndex">Relative index of navigation point.</param>
        /// <returns>Position of target navigation  point.</returns>
        public abstract Vector3 GetDestination(ref int destinationIndex);
    }
}
