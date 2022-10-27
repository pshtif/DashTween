/*
 *	Created by:  Peter @sHTiF Stefcek
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

namespace Dash
{
    enum DashTweenType
    {
        FLOAT,
        VECTOR2,
        VECTOR3,
        COLOR
    }
    
    public class DashTween : IInternalTweenAccess
    {
        static internal List<DashTween> _activeTweens = new List<DashTween>();

        static private Queue<DashTween> _pooledTweens = new Queue<DashTween>();

        private DashTweenType type = DashTweenType.FLOAT;
        
        public Object target { get; private set; }
        public float delay { get; private set; }
        
        public float from { get; private set; }
        public float to { get; private set; }
        
        public Vector2 fromVector2 { get; private set; }
        public Vector2 toVector2 { get; private set; }
        
        public Vector3 fromVector { get; private set; }
        public Vector3 toVector { get; private set; }
        
        public Color fromColor { get; private set; }
        public Color toColor { get; private set; }
        
        public float duration { get; private set; }

        public EaseType easeType = EaseType.LINEAR;
        
        public float current { get; private set; }

        public bool running { get; private set; } = false;

        public bool relative { get; private set; } = false;

        public bool useSpeed { get; private set; } = false;

        private Action<float> _updateCallbackFloat;
        private Action<Vector2> _updateCallbackVector2;
        private Action<Vector3> _updateCallbackVector3;
        private Action<Color> _updateCallbackColor;

        private Action<float> _updateInternalCallbackFloat;
        private Action<Vector2> _updateInternalCallbackVector2;
        private Action<Vector3> _updateInternalCallbackVector3;
        private Action<Color> _updateInternalCallbackColor;

        private Action _completeCallback;
        
        private bool _active = true;
        public int Id { get; private set; } = 0;

        public static DashTween To(Object p_target, float p_from, float p_to, float p_time)
        {
            DashTweenCore.Initialize();
            
            DashTween tween = Create(p_target, p_from, p_to, p_time);
            return tween;
        }
        
        public static DashTween To(Object p_target, Vector3 p_from, Vector3 p_to, float p_time)
        {
            DashTweenCore.Initialize();
            
            DashTween tween = Create(p_target, p_from, p_to, p_time);
            return tween;
        }
        
        public static DashTween To(Object p_target, Color p_from, Color p_to, float p_time)
        {
            DashTweenCore.Initialize();
            
            DashTween tween = Create(p_target, p_from, p_to, p_time);
            return tween;
        }

        public static void DelayedCall(float p_time, Action p_callback)
        {
            DashTweenCore.Initialize();

            Create(null, 0, 1, p_time).OnComplete(p_callback).Start();
        }

        private static DashTween Create(Object p_target, float p_from, float p_to, float p_duration)
        {
            DashTween tween;
            if (_pooledTweens.Count == 0)
            {
                tween = new DashTween();
            }
            else
            {
                tween = _pooledTweens.Dequeue();
                tween.Reset();
            }
            
            tween.type = DashTweenType.FLOAT;
            tween.target = p_target;
            tween.from = p_from;
            tween.to = p_to;
            tween.duration = p_duration;
            
            _activeTweens.Add(tween);

            return tween;
        }
        
        private static DashTween Create(Object p_target, Vector2 p_from, Vector2 p_to, float p_duration)
        {
            DashTween tween;
            if (_pooledTweens.Count == 0)
            {
                tween = new DashTween();
            }
            else
            {
                tween = _pooledTweens.Dequeue();
                tween.Reset();
            }
            
            tween.type = DashTweenType.VECTOR2;
            tween.target = p_target;
            tween.fromVector2 = p_from;
            tween.toVector2 = p_to;
            tween.duration = p_duration;
            
            _activeTweens.Add(tween);

            return tween;
        }
        
        private static DashTween Create(Object p_target, Vector3 p_from, Vector3 p_to, float p_duration)
        {
            DashTween tween;
            if (_pooledTweens.Count == 0)
            {
                tween = new DashTween();
            }
            else
            {
                tween = _pooledTweens.Dequeue();
                tween.Reset();
            }
            
            tween.type = DashTweenType.VECTOR3;
            tween.target = p_target;
            tween.fromVector = p_from;
            tween.toVector = p_to;
            tween.duration = p_duration;
            
            _activeTweens.Add(tween);

            return tween;
        }
        
        private static DashTween Create(Object p_target, Color p_from, Color p_to, float p_duration)
        {
            DashTween tween;
            if (_pooledTweens.Count == 0)
            {
                tween = new DashTween();
            }
            else
            {
                tween = _pooledTweens.Dequeue();
                tween.Reset();
            }
            
            tween.type = DashTweenType.COLOR;
            tween.target = p_target;
            tween.fromColor = p_from;
            tween.toColor = p_to;
            tween.duration = p_duration;
            
            _activeTweens.Add(tween);

            return tween;
        }

        void Reset()
        {
            current = 0;
            running = false;
            delay = 0;
            easeType = EaseType.LINEAR;
            relative = false;
            _updateCallbackFloat = null;
            _updateCallbackVector2 = null;
            _updateCallbackVector3 = null;
            _updateCallbackColor = null;
            _updateInternalCallbackFloat = null;
            _updateInternalCallbackVector2 = null;
            _updateInternalCallbackVector3 = null;
            _updateInternalCallbackColor = null;
            _completeCallback = null;
            _active = true;
        }

        public DashTween SetDelay(float p_delay)
        {
            delay = p_delay;
            return this;
        }

        public DashTween SetEase(EaseType p_easeType)
        {
            easeType = p_easeType;
            return this;
        }

        public DashTween SetRelative(bool p_relative)
        {
            relative = p_relative;
            return this;
        }
        
        public DashTween SetUseSpeed(bool p_useSpeed)
        {
            useSpeed = p_useSpeed;
            return this;
        }

        public DashTween OnUpdate(Action<float> p_callback)
        {
            _updateCallbackFloat = p_callback;
            return this;
        }
        
        public DashTween OnUpdate(Action<Vector2> p_callback)
        {
            _updateCallbackVector2 = p_callback;
            return this;
        }
        
        public DashTween OnUpdate(Action<Vector3> p_callback)
        {
            _updateCallbackVector3 = p_callback;
            return this;
        }
        
        public DashTween OnUpdate(Action<Color> p_callback)
        {
            _updateCallbackColor = p_callback;
            return this;
        }

        internal DashTween OnInternalUpdate(Action<float> p_callback)
        {
            _updateCallbackFloat = p_callback;
            return this;
        }
        
        internal DashTween OnInternalUpdate(Action<Vector2> p_callback)
        {
            _updateCallbackVector2 = p_callback;
            return this;
        }
        
        internal DashTween OnInternalUpdate(Action<Vector3> p_callback)
        {
            _updateCallbackVector3 = p_callback;
            return this;
        }
        
        internal DashTween OnInternalUpdate(Action<Color> p_callback)
        {
            _updateCallbackColor = p_callback;
            return this;
        }

        public DashTween OnComplete(Action p_callback)
        {
            _completeCallback = p_callback;
            return this;
        }
        
        public DashTween Start()
        {
            if (duration == 0 && delay == 0)
            {
                if (useSpeed) Debug.LogWarning("Infinite tween with 0 speed.");
                CallUpdate(1);
                _completeCallback?.Invoke();
                Clean();
            }
            else
            {
                if (useSpeed) CalculateDurationFromSpeed();
                CallUpdate(0);
                running = true;
            }

            return this;
        }

        public void Kill(bool p_complete)
        {
            if (p_complete)
            {
                current = duration + delay;
                CallUpdate((current - delay) / duration);
                _completeCallback?.Invoke();
            }
            
            Clean();
        }

        void IInternalTweenAccess.Update(float p_time)
        {
            if (!running)
                return;

            current += p_time;
            if (current >= duration + delay)
            {
                current = duration + delay;
                
                CallUpdate((current - delay) / duration);
                _completeCallback?.Invoke();
                Clean();

                return;
            }
            
            if (current > delay)
            {
                CallUpdate((current - delay) / duration);
            }
        }

        void CallUpdate(float p_time)
        {
            if (type == DashTweenType.FLOAT)
            {
                var f = EaseValue(relative ? 0 : from, to, p_time, easeType);
                _updateInternalCallbackFloat?.Invoke(f);
                _updateCallbackFloat?.Invoke(f);
            } 
            
            if (type == DashTweenType.VECTOR2)
            {
                var v2 = EaseValue(relative ? Vector2.zero : fromVector2, toVector2, p_time, easeType);
                _updateInternalCallbackVector2?.Invoke(v2);
                _updateCallbackVector2?.Invoke(v2);
            }
            
            if (type == DashTweenType.VECTOR3)
            {
                var v3 = EaseValue(relative ? Vector3.zero : fromVector, toVector, p_time, easeType);
                _updateInternalCallbackVector3?.Invoke(v3);
                _updateCallbackVector3?.Invoke(v3);
            }
            
            if (type == DashTweenType.COLOR)
            {
                var c = EaseValue(relative ? Color.clear : fromColor, toColor, p_time, easeType);
                _updateInternalCallbackColor?.Invoke(c);
                _updateCallbackColor?.Invoke(c);
            }
        }

        void CalculateDurationFromSpeed()
        {
            switch (type)
            {
                case DashTweenType.FLOAT:
                    duration = Mathf.Abs(to - from) / duration;
                    break;
                case DashTweenType.VECTOR2:
                    duration = (toVector2 - fromVector2).magnitude / duration;
                    break;
                case DashTweenType.VECTOR3:
                    duration = (toVector - fromVector).magnitude / duration;
                    break;
                case DashTweenType.COLOR:
                    Debug.LogWarning("Speed based tweening not implemented for colors.");
                    break;
            }
        }
        
        void Clean()
        {
            if (!_active)
                return;
            
            _active = false;
            running = false;
            _activeTweens.Remove(this);
            _pooledTweens.Enqueue(this);
        }

        public static void CleanAll()
        {
            _activeTweens.Clear();
            _pooledTweens.Clear();
        }

        public static float EaseValue(float p_from, float p_to, float p_delta, EaseType p_easeType)
        {
            return p_from + (p_to - p_from) * EaseFunctions.Evaluate(p_delta, 1f, p_easeType);
        }
        
        public static Vector2 EaseValue(Vector2 p_from, Vector2 p_to, float p_delta, EaseType p_easeType)
        {
            float e = EaseFunctions.Evaluate(p_delta, 1f, p_easeType);
            return new Vector2(p_from.x + (p_to.x - p_from.x) * e,
                p_from.y + (p_to.y - p_from.y) * e);
        }
        
        public static Vector3 EaseValue(Vector3 p_from, Vector3 p_to, float p_delta, EaseType p_easeType)
        {
            float e = EaseFunctions.Evaluate(p_delta, 1f, p_easeType);
            return new Vector3(p_from.x + (p_to.x - p_from.x) * e,
                p_from.y + (p_to.y - p_from.y) * e,
                p_from.z + (p_to.z - p_from.z) * e);
        }
        
        public static Color EaseValue(Color p_from, Color p_to, float p_delta, EaseType p_easeType)
        {
            float e = EaseFunctions.Evaluate(p_delta, 1f, p_easeType);
            return new Color(p_from.r + (p_to.r - p_from.r) * e,
                p_from.g + (p_to.g - p_from.g) * e,
                p_from.b + (p_to.b - p_from.b) * e, 
                p_from.a + (p_to.a - p_from.a) * e);
        }
    }
}