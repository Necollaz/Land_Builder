using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parentContainer;
    private readonly Stack<T> cached = new Stack<T>();
    private readonly List<T> inUse = new List<T>();
    
    public GameObjectPool(T prefab, Transform parentContainer, int preloadCount)
    {
        this.prefab = prefab;
        this.parentContainer = parentContainer;
        
        for (int i = 0; i < preloadCount; i++)
        {
            T instance = Object.Instantiate(prefab, parentContainer);
            
            instance.gameObject.SetActive(false);
            cached.Push(instance);
        }
    }

    public T Take()
    {
        T instance = cached.Count > 0 ? cached.Pop() : Object.Instantiate(prefab, parentContainer);
        
        inUse.Add(instance);
        instance.gameObject.SetActive(true);
        
        return instance;
    }

    public void Return(T instance)
    {
        if (instance == null)
            return;

        instance.gameObject.SetActive(false);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one;
        
        inUse.Remove(instance);
        cached.Push(instance);
    }

    public void ReturnAll()
    {
        for (int i = inUse.Count - 1; i >= 0; i--)
            Return(inUse[i]);
    }
}