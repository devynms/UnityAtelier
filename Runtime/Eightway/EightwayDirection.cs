using System;
using UnityEngine;


namespace Atelier.Eightway {

    /// <summary>
    /// All the 8-way directions. Since C# doesn't (didn't?) have enumerated classes, the 
    /// corresponding vectors are in the EightwayDirections class.
    /// 
    /// Since the methods in EightwayDirections are extension methods, you can use them as if they
    /// were methods on the EightwayDirection class.
    /// 
    /// Add the following import to gain access to the extension methods:
    /// <code>
    /// using static Atelier.Eightway.EightwayDirections;
    /// </code>
    /// </summary>
    public enum EightwayDirection {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    /// <summary>
    /// Helpers (esp. extension methods) for dealing with 8-way directions. For now, specialized for
    /// 2-d code.
    /// </summary>
    public static class EightwayDirections {

        private readonly static Vector2 NorthVector = new Vector2 { x = 0, y = 1 }.normalized;
        private readonly static Vector2 NorthEastVector = new Vector2 { x = 1, y = 1 }.normalized;
        private readonly static Vector2 EastVector = new Vector2 { x = 1, y = 0 }.normalized;
        private readonly static Vector2 SouthEastVector = new Vector2 { x = 1, y = -1 }.normalized;
        private readonly static Vector2 SouthVector = new Vector2 { x = 0, y = -1 }.normalized;
        private readonly static Vector2 SouthWestVector = new Vector2 { x = -1, y = -1 }.normalized;
        private readonly static Vector2 WestVector = new Vector2 { x = -1, y = 0 }.normalized;
        private readonly static Vector2 NorthWestVector = new Vector2 { x = -1, y = 1 }.normalized;

        public static float AngleFrom(this EightwayDirection direction, Vector2 vector) {
            return Vector2.SignedAngle(vector, direction.ToVector());
        }

        public static Vector2 ToDirection(this EightwayDirection direction) {
            return direction.ToVector().normalized;
        }

        public static EightwayDirection ToEightway(this Vector2 vector) {
            const float NorthAngle = 90.0f;
            const float NorthEastAngle = 45.0f;
            const float EastAngle = 0.0f;
            const float SouthEastAngle = -45.0f;
            const float SouthAngle = -90.0f;
            const float SouthWestAngle = -135.0f;
            const float WestAngle = 180.0f;
            const float NorthWestAngle = 135.0f;


            float angle = Vector2.SignedAngle(Vector2.right, vector);

            if (AngleInRange(angle, NorthWestAngle, NorthAngle, NorthEastAngle)) {
                return EightwayDirection.North;
            } else if (AngleInRange(angle, NorthAngle, NorthEastAngle, EastAngle)) {
                return EightwayDirection.NorthEast;
            } else if (AngleInRange(angle, NorthEastAngle, EastAngle, SouthEastAngle)) {
                return EightwayDirection.East;
            } else if (AngleInRange(angle, EastAngle, SouthEastAngle, SouthAngle)) {
                return EightwayDirection.SouthEast;
            } else if (AngleInRange(angle, SouthEastAngle, SouthAngle, SouthWestAngle)) {
                return EightwayDirection.South;
            } else if (AngleInRange(angle, SouthAngle, SouthWestAngle, -WestAngle)) {
                return EightwayDirection.SouthWest;
            } else if ((angle < (SouthWestAngle - WestAngle) / 2.0f && -WestAngle <= angle) || (angle >= (NorthWestAngle + WestAngle) / 2.0f && WestAngle >= angle)) {
                return EightwayDirection.West;
            } else if (AngleInRange(angle, WestAngle, NorthWestAngle, NorthAngle)) {
                return EightwayDirection.NorthWest;
            } else {
                throw new ArgumentException("Vector to Eightway " + vector);
            }
        }

        public static Vector2 ToEightwayDirection(this Vector2 vector) {
            return vector.ToEightway().ToDirection();
        }

        public static Vector2 ToVector(this EightwayDirection direction) {
            switch (direction) {
                case EightwayDirection.North:       return NorthVector;
                case EightwayDirection.NorthEast:   return NorthEastVector;
                case EightwayDirection.East:        return EastVector;
                case EightwayDirection.SouthEast:   return SouthEastVector;
                case EightwayDirection.South:       return SouthVector;
                case EightwayDirection.SouthWest:   return SouthWestVector;
                case EightwayDirection.West:        return WestVector;
                case EightwayDirection.NorthWest:   return NorthWestVector;
                default:
                    throw new ArgumentException("EightwayDirection " + direction);
            }
        }

        private static bool AngleInRange(float angle, float low, float target, float high) {
            float lowBound = (low + target) / 2.0f;
            float highBound = (target + high) / 2.0f;
            return Mathf.Min(lowBound, highBound) <= angle && angle < Mathf.Max(lowBound, highBound);
        }

    }

}
