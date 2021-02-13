using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Events;

namespace Jlabarca.OneBigMob.MonoBehaviours
{
	public class UnitView : MonoBehaviour
    {
	    public int id;

        public EcsEntity Entity;

		public UnityAction<UnitView> OnDie;
    }
}
