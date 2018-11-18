﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Messages
{
    class Observable : MonoBehaviour
    {
        private Dictionary<Type, Delegate> observers = new Dictionary<Type, Delegate>();
        private Queue<Action> handlers = new Queue<Action>();

        private Type currentType;
        private Delegate currentDelegate;

        public void Subscribe<T>(Action<T> action)
        {
            SetCurrentType<T>();
            TryGetDelegateByCurrentType();
            AddActionToCurrentDelegate(action);
            SetCurrentDelegate();
        }

        public void Unsubscribe<T>(Action<T> action)
        {
            SetCurrentType<T>();
            TryGetDelegateByCurrentType();
            RemoveActionFromCurrentDelegate(action);
            SetOrRemoveCurrentDelegate();
        }

        public void Publish<T>(T message)
        {
            handlers.Enqueue(() =>
            {
                SetCurrentType<T>();
                TryGetDelegateByCurrentType();
                SafeInvokeCurrentDelegateWithMessage(message);
            });
        }

        private void Update()
        {
            RunAllHandlers();
        }

        private void RunAllHandlers()
        {
            var toHandleCount = handlers.Count;

            for(int i = 0; i < toHandleCount; i++)
            {
                RunHandler();
            }
        }

        private void RunHandler()
        {
            var handler = handlers.Dequeue();
            handler.Invoke();
        }

        private void SetCurrentType<T>()
        {
            currentType = typeof(T);
        }

        private void TryGetDelegateByCurrentType()
        {
            observers.TryGetValue(currentType, out currentDelegate);
        }

        private void SetCurrentDelegate()
        {
            observers[currentType] = currentDelegate;
        }

        private void SetOrRemoveCurrentDelegate()
        {
            if (currentDelegate == null)
            {
                observers.Remove(currentType);
            }
            else
            {
                observers[currentType] = currentDelegate;
            }
        }

        private void AddActionToCurrentDelegate<T>(Action<T> action)
        {
            currentDelegate = Delegate.Combine(currentDelegate, action);
        }

        private void RemoveActionFromCurrentDelegate<T>(Action<T> action)
        {
            currentDelegate = Delegate.Remove(currentDelegate, action);
        }

        private void SafeInvokeCurrentDelegateWithMessage<T>(T message)
        {
            if(currentDelegate == null)
            {
                return;
            }

            foreach (var action in currentDelegate.GetInvocationList().Cast<Action<T>>())
            {
                try
                {
                    action(message);
                }
                catch (Exception e)
                {
                    LogError(e);
                }
            }
        }

        private void LogError(Exception e)
        {
            Debug.LogException(e);
        }
    }
}