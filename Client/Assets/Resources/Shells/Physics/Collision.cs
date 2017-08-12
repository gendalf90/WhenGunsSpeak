﻿using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    public class Collision : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            observable.Publish(new OnBulletHitEvent(Guid, other.GetInstanceID()));
        }

        //oncollision не нужен, гильзы будут уничтожаться по таймауту с момента создания

        public Guid Guid { get; set; }
    }
}