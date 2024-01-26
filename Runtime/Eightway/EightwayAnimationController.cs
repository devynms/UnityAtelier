using UnityEngine;
using System;

namespace Atelier.Eightway {

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(EightwayFacing))]
    [AddComponentMenu("Atelier/Eightway/Eightway Animation Controller")]
    public class EightwayAnimationController : MonoBehaviour {

        [SerializeField] private Sprite north;
        [SerializeField] private Sprite northEast;
        [SerializeField] private Sprite east;
        [SerializeField] private Sprite southEast;
        [SerializeField] private Sprite south;
        [SerializeField] private Sprite southWest;
        [SerializeField] private Sprite west;
        [SerializeField] private Sprite northWest;

        private EightwayFacing facing;
        private SpriteRenderer spriteRenderer;

        private void Awake() {
            this.facing = GetComponent<EightwayFacing>();
            this.spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            this.spriteRenderer.sprite = this.DirectionToSprite(this.facing.Direction);
        }

        private Sprite DirectionToSprite(EightwayDirection direction) {
            switch (direction) {
                case EightwayDirection.North:       return this.north;
                case EightwayDirection.NorthEast:   return this.northEast;
                case EightwayDirection.East:        return this.east;
                case EightwayDirection.SouthEast:   return this.southEast;
                case EightwayDirection.South:       return this.south;
                case EightwayDirection.SouthWest:   return this.southWest;
                case EightwayDirection.West:        return this.west;
                case EightwayDirection.NorthWest:   return this.northWest;
                default:
                    throw new ArgumentException("direction " + direction);
            }
        }


    }

}

