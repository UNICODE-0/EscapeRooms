using UnityEngine;

namespace EscapeRooms.Data
{
    public class Singleton<T> where T: class
    {
        private static T _instance;
        public static T Instance => _instance;

        public static bool TrySetInstance(T instance)
        {
            if (_instance != null)
            {
                Debug.LogError($"{typeof(T)} already has instance");
                return false;
            }
            
            _instance = instance;
            return true;
        }
        
        public static bool TryRemoveInstance()
        {
            if (_instance == null)
            {
                Debug.LogError($"{typeof(T)} instance already disposed");
                return false;
            }

            _instance = null;
            return true;
        }
    }
}