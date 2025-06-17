using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int health;
    public int metal;
    public int hungry;
    public List<string> inventory;
}

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _health = 100;
    [SerializeField]
    private int _metal = 0;
    [SerializeField]
    private int _hungry = 0;
    [SerializeField]
    private List<string> _inventory = new List<string>();

    public int Health => _health;
    public int Metal => _metal;
    public int Hungry => _hungry;
    public IReadOnlyList<string> Inventory => _inventory;

    public void AddHealth(int amount)
    {
        _health += amount;
        Debug.Log($"체력 변화: {amount}, 현재 체력: {_health}");
    }

    public void AddMetal(int amount)
    {
        _metal += amount;
        Debug.Log($"금속 변화: {amount}, 현재 금속: {_metal}");
    }

    public void AddHungry(int amount)
    {
        _hungry += amount;
        Debug.Log($"허기 변화: {amount}, 현재 허기: {_hungry}");
    }

    public void AddItem(string item)
    {
        _inventory.Add(item);
        Debug.Log($"{item} 아이템을 획득했습니다.");
    }

    public void RemoveItem(string item)
    {
        if (_inventory.Remove(item))
            Debug.Log($"{item} 아이템을 잃었습니다.");
    }

    private string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "player_save.json");
    }

    public void Save()
    {
        var data = new PlayerData
        {
            health = _health,
            metal = _metal,
            hungry = _hungry,
            inventory = new List<string>(_inventory)
        };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(), json);
        Debug.Log("플레이어 데이터 저장 완료");
    }

    public void Load()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<PlayerData>(json);
            _health = data.health;
            _metal = data.metal;
            _hungry = data.hungry;
            _inventory = new List<string>(data.inventory);
            Debug.Log("플레이어 데이터 불러오기 완료");
        }
        else
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
        }
    }
}
