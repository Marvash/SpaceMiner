using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class SingletonPersistent<T> : MonoBehaviour where T : SingletonPersistent<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Returns whether the instance has been initialized or not.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return _instance != null;
            }
        }

        /// <summary>
        /// Base awake method that sets the singleton's unique instance.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
            }
            else
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}