namespace EscapeRooms.Data
{
    public class Singleton<T> where T: class
    {
        private static T _instance;
        public static T Instance => _instance;

        public static bool TrySetInstance(T settings)
        {
            if (_instance != null) return false;
            
            _instance = settings;
            return true;
        }
        
        public static bool TryRemoveInstance()
        {
            if (_instance == null) return false;

            _instance = null;
            return true;
        }
    }
}