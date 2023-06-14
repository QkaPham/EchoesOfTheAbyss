using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObservedList<T> : IList<T>
{
    public delegate void ListChanged(int index, T oldValue, T newValue);

    [SerializeField] private List<T> _list = new List<T>();

    // NOTE: I changed the signature to provide a bit more information
    // now it returns index, oldValue, newValue
    public event ListChanged OnListChanged;

    public event Action Updated;

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _list.Add(item);
        Updated?.Invoke();
    }

    public void Clear()
    {
        _list.Clear();
        Updated?.Invoke();
    }

    public bool Contains(T item)
    {
        return _list.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var output = _list.Remove(item);
        Updated?.Invoke();

        return output;
    }

    public int Count => _list.Count;
    public bool IsReadOnly => false;

    public int IndexOf(T item)
    {
        return _list.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        Updated?.Invoke();
    }

    public void RemoveAt(int index)
    {
        _list.RemoveAt(index);
        Updated?.Invoke();
    }

    public void AddRange(IEnumerable<T> collection)
    {
        _list.AddRange(collection);
        Updated?.Invoke();
    }

    public void RemoveAll(Predicate<T> predicate)
    {
        _list.RemoveAll(predicate);
        Updated?.Invoke();
    }

    public void InsertRange(int index, IEnumerable<T> collection)
    {
        _list.InsertRange(index, collection);
        Updated?.Invoke();
    }

    public void RemoveRange(int index, int count)
    {
        _list.RemoveRange(index, count);
        Updated?.Invoke();
    }

    public T this[int index]
    {
        get { return _list[index]; }
        set
        {
            var oldValue = _list[index];
            _list[index] = value;
            OnListChanged?.Invoke(index, oldValue, value);
            // I would also call the generic one here
            Updated?.Invoke();
        }
    }
}
