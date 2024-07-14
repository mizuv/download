using System;
using System.Collections.Generic;

namespace Mizuvt.Common {
    public class BehaviorSubject<T> {
        private T _value;
        private readonly List<Action<T>> _subscribers = new List<Action<T>>();

        public T Value {
            private set {
                _value = value;
                NotifySubscribers();
            }
            get => _value;
        }

        public BehaviorSubject(T value) {
            _value = value;
            NotifySubscribers();
        }

        public void Next(T value) {
            Value = value;
        }

        public Action Subscribe(Action<T> subscriber) {
            _subscribers.Add(subscriber);
            subscriber(Value); // 초기 값 전달

            // 구독 해지 메서드 반환
            return () => _subscribers.Remove(subscriber);
        }

        public void Unsuscribe(Action<T> subscriber) {
            _subscribers.Remove(subscriber);
        }

        private void NotifySubscribers() {
            foreach (var subscriber in _subscribers) {
                subscriber(Value);
            }
        }
    }
}
