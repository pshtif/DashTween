/*
 *	Created by:  Peter @sHTiF Stefcek
 */

using UnityEngine;
using UnityEngine.UI;

namespace Dash
{
    public static class DashTweenExtensions
    {
        static public bool DoNullChecks = false;
        
        public static DashTween DashLocalRotate(this Transform p_transform, Vector3 p_rotation, float p_time, bool p_useSpeed = false)
        {
            var original = p_transform.localRotation.eulerAngles;
            var tween = DashTween.To(p_transform, p_transform.localRotation.eulerAngles, p_rotation, p_time);
            tween.OnInternalUpdate(
                (Vector3 v) =>
                {
                    if (DoNullChecks && p_transform == null)
                        return;
                    
                    p_transform.localRotation = tween.relative ? Quaternion.Euler(original + v) : Quaternion.Euler(v);
                }).Start();
            return tween;
        }
        
        public static DashTween DashRotate(this Transform p_transform, Vector3 p_rotation, float p_time, bool p_useSpeed = false)
        {
            var original = p_transform.rotation.eulerAngles;
            var tween = DashTween.To(p_transform, p_transform.rotation.eulerAngles, p_rotation, p_time);
            tween.OnInternalUpdate(
                (Vector3 v) =>
                {
                    if (DoNullChecks && p_transform == null)
                        return;
                    
                    p_transform.rotation = tween.relative ? Quaternion.Euler(original + v) : Quaternion.Euler(v);
                }).Start();
            return tween;
        }
        
        public static DashTween DashMove(this Transform p_transform, Vector3 p_position, float p_time, bool p_useSpeed = false)
        {
            var original = p_transform.position;
            var tween = DashTween.To(p_transform, p_transform.position, p_position, p_time);
            tween.OnInternalUpdate(
                (Vector3 v) =>
                {
                    if (DoNullChecks && p_transform == null)
                        return;
                    
                    p_transform.position = tween.relative ? original + v : v;
                }).Start();
            return tween;
        }
        
        public static DashTween DashLocalMove(this Transform p_transform, Vector3 p_position, float p_time, bool p_useSpeed = false)
        {
            var original = p_transform.localPosition;
            var tween = DashTween.To(p_transform, p_transform.localPosition, p_position, p_time);
            tween.OnInternalUpdate(
                (Vector3 v) =>
                {
                    if (DoNullChecks && p_transform == null)
                        return;
                    
                    p_transform.localPosition = tween.relative ? original + v : v;
                }).Start();

            return tween;
        }

        public static DashTween DashColor(this Graphic p_graphic, Color p_color, float p_time)
        {
            var original = p_graphic.color;
            var tween = DashTween.To(p_graphic, p_graphic.color, p_color, p_time);
            tween.OnInternalUpdate(
                (Color c) =>
                {
                    if (DoNullChecks && p_graphic == null)
                        return;
                    
                    p_graphic.color = tween.relative ? original + c : c;
                }).Start();
            return tween;
        }
    }
}