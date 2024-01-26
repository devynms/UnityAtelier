using NUnit.Framework;
using UnityEngine;


namespace Atelier.Eightway {

    public class EightwayDirectionTest {

        public static EightwayDirection ToEightway(Vector2 vector) {
            return vector.ToEightway();
        }

        [Test]
        public void SimpleVectorsToEightway() {
            Vector2 upRight = Vector2.up + Vector2.right;
            Vector2 downRight = Vector2.down + Vector2.right;
            Vector2 downLeft = Vector2.down + Vector2.left;
            Vector2 upLeft = Vector2.up + Vector2.left;

            Assert.AreEqual(EightwayDirection.North,        ToEightway(Vector2.up),             "up => North");
            Assert.AreEqual(EightwayDirection.NorthEast,    ToEightway(upRight),                "upRight => NorthEast");
            Assert.AreEqual(EightwayDirection.NorthEast,    ToEightway(upRight.normalized),     "upRight.normalized => NorthEast");
            Assert.AreEqual(EightwayDirection.East,         ToEightway(Vector2.right),          "Right => East");
            Assert.AreEqual(EightwayDirection.SouthEast,    ToEightway(downRight),              "downRight => SouthEast");
            Assert.AreEqual(EightwayDirection.SouthEast,    ToEightway(downRight.normalized),   "downRight.normalized => SouthEast");
            Assert.AreEqual(EightwayDirection.South,        ToEightway(Vector2.down),           "down => South");
            Assert.AreEqual(EightwayDirection.SouthWest,    ToEightway(downLeft),               "downLeft => SouthWest");
            Assert.AreEqual(EightwayDirection.SouthWest,    ToEightway(downLeft.normalized),    "downLeft.normalized => SouthWest");
            Assert.AreEqual(EightwayDirection.West,         ToEightway(Vector2.left),           "left => West");
            Assert.AreEqual(EightwayDirection.NorthWest,    ToEightway(upLeft),                 "upLeft => NorthWest");
            Assert.AreEqual(EightwayDirection.NorthWest,    ToEightway(upLeft.normalized),      "upLeft.normalized => NorthWest");
        }

        [Test]
        public void BoundaryVectorsToEightway() {
            Assert.AreEqual(EightwayDirection.West,         ToEightway(DegToVec(-179)));
            Assert.AreEqual(EightwayDirection.West,         ToEightway(DegToVec(179)));

            Assert.AreEqual(EightwayDirection.East,         ToEightway(DegToVec(22)));
            Assert.AreEqual(EightwayDirection.NorthEast,    ToEightway(DegToVec(23)));

            Assert.AreEqual(EightwayDirection.NorthEast,    ToEightway(DegToVec(67)));
            Assert.AreEqual(EightwayDirection.North,        ToEightway(DegToVec(68)));

            Assert.AreEqual(EightwayDirection.North,        ToEightway(DegToVec(112)));
            Assert.AreEqual(EightwayDirection.NorthWest,    ToEightway(DegToVec(113)));

            Assert.AreEqual(EightwayDirection.NorthWest,    ToEightway(DegToVec(157)));
            Assert.AreEqual(EightwayDirection.West,         ToEightway(DegToVec(158)));

            Assert.AreEqual(EightwayDirection.West,         ToEightway(DegToVec(-158)));
            Assert.AreEqual(EightwayDirection.SouthWest,    ToEightway(DegToVec(-157)));

            Assert.AreEqual(EightwayDirection.SouthWest,    ToEightway(DegToVec(-113)));
            Assert.AreEqual(EightwayDirection.South,        ToEightway(DegToVec(-112)));

            Assert.AreEqual(EightwayDirection.South,        ToEightway(DegToVec(-68)));
            Assert.AreEqual(EightwayDirection.SouthEast,    ToEightway(DegToVec(-67)));

            Assert.AreEqual(EightwayDirection.SouthEast,    ToEightway(DegToVec(-23)));
            Assert.AreEqual(EightwayDirection.East,         ToEightway(DegToVec(-22)));
        }

        private static Vector2 DegToVec(float angle) {
            return new Vector2 {
                x = Mathf.Cos(angle * Mathf.Deg2Rad),
                y = Mathf.Sin(angle * Mathf.Deg2Rad),
            };
        }

    }

}
