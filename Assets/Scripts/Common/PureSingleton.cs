// MoreMountains 패키지에서 가져옴
using UnityEngine;


namespace Mizuvt.Common {
    public abstract class PureSingleton<T> where T : PureSingleton<T>, new() {
        private static T? _instance = null;
        private static readonly object _lock = new object();

        protected PureSingleton() { }

        public static T Instance {
            get {
                if (_instance == null) {
                    lock (_lock) {
                        if (_instance == null) {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}