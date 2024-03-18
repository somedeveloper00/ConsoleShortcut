using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace ConsoleShortcut
{
    public sealed class ConsoleShortcut : MonoBehaviour
    {
        private readonly Dictionary<char, Action> _shortcuts = new(8);

        public static ConsoleShortcut Instance { get; private set; }

        private void Awake()
        {
            Debug.Log("cs awaked");
            if (Instance)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            new Thread(() =>
            {
                while (true)
                {
                    var key = Console.ReadKey().KeyChar;
                    if (_shortcuts.TryGetValue(key, out var callback))
                    {
                        try
                        {
                            callback?.Invoke();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
            }).Start();
            Debug.Log("instance initialized");
        }

        public void RegisterShortcut(char keyCode, Action callback) => _shortcuts[keyCode] = callback;

        public void UnregisterShortcut(char keyCode) => _shortcuts.Remove(keyCode);

        public void UnregisterAllShortcuts() => _shortcuts.Clear();
    }
}