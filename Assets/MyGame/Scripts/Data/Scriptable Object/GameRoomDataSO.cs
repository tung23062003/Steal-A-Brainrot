using System.Collections.Generic;
using UnityEngine;

public class GameRoomDataSO : ScriptableObject
{
    public List<EntityInfo> entities = new();

    public void AddEntity(EntityInfo entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(EntityInfo entity)
    {
        if (entities.Contains(entity))
            entities.Remove(entity);
        else
            LogUtils.Log("Entity is not founded");
    }

    public void RemoveAllEntity()
    {
        if(entities.Count > 0)
            entities.Clear();
    }

    public List<EntityInfo> GetEntityByType(EntityType entityType)
    {
        return entities.FindAll(item => item.entityType == entityType);
    }

    public List<EntityInfo> GetEnemyByType(EnemyType enemyType)
    {
        return entities.FindAll(item => item.enemyType == enemyType);
    }

    public EntityInfo GetEntityByName(string name)
    {
        return entities.Find(item => item.name.Equals(name));
    }

}

[System.Serializable]
public class EntityInfo
{
    public string name;
    public EntityType entityType;
    public EnemyType enemyType;
    //public int health;
    //public int maxHealth;

    //private bool ShowEnemyType => entityType == EntityType.NPC;
}