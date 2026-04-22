using UnityEngine;

namespace Work.JW.Code.Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/Entity/Finder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Entity target;
        
        public void SetEntity(Entity entity)
        {
            target = entity;
        }
    }
}