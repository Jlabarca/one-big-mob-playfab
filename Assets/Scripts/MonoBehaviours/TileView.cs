using Leopotam.Ecs;
using UnityEngine;

namespace Jlabarca.OneBigMob.MonoBehaviours
{
	public class TileView : MonoBehaviour
    {
	    public int id;
        public EcsEntity Entity;
        public Transform movingTile;
        public Transform targetTile;

        private void Awake()
        {
	        movingTile = transform.GetChild(0);
	        targetTile = transform.GetChild(1);
        }
    }
}
