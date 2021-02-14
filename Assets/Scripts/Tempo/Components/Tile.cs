using Jlabarca.OneBigMob.MonoBehaviours;
using UnityEngine;

namespace Jlabarca.OneBigMob.Tempo.Components
{
    internal struct Tile
    {
        public Vector3 SpawnPosition;
        public Vector3 TargetPosition;
        public float ReleaseTime;
        public float FinalTime;
        public TileView TileView;
    }
}
