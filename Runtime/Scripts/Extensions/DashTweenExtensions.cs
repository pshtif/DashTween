/*
 *	Created by:  Peter @sHTiF Stefcek
 */

using UnityEngine;
using UnityEngine.UI;

namespace Dash
{
    public static class DashTweenExtensions
    {
        public static bool DoNullChecks = false;
        
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
        
        public static DashTween DashAnchoredPosition(this RectTransform p_rectTransform,
            Vector2 p_finalPosition,
            float p_duration,
            EaseType p_easeType = EaseType.LINEAR,
            float p_delay = 0f)
        {
            var original = p_rectTransform.anchoredPosition;
            var tween = DashTween.To(p_rectTransform, 0f, 1f, p_duration);

            if (p_delay >= 0f)
            {
                tween.SetDelay(p_delay);
            }

            tween.OnInternalUpdate(delta =>
            {
                if (DoNullChecks && p_rectTransform == null)
                {
                    return;
                }

                p_rectTransform.anchoredPosition = new Vector2(
                    DashTween.EaseValue(original.x, p_finalPosition.x, delta, p_easeType),
                    DashTween.EaseValue(original.y, p_finalPosition.y, delta, p_easeType));
            });
            tween.Start();
            return tween;
        }

		public static DashTween DashAnchoredPositionX(this RectTransform p_rectTransform,
            float p_finalPosition,
            float p_duration,
            EaseType p_easeType = EaseType.LINEAR,
            float p_delay = 0f)
        {
            var posY = p_rectTransform.anchoredPosition.y;
            return p_rectTransform.TEST_DashAnchoredPosition(new Vector2(p_finalPosition, posY),
                p_duration,
                p_easeType,
                p_delay);
        }

        public static DashTween DashAnchoredPositionY(this RectTransform p_rectTransform,
            float p_finalPosition,
            float p_duration,
            EaseType p_easeType = EaseType.LINEAR,
            float p_delay = 0f)
        {
            var posX = p_rectTransform.anchoredPosition.x;
            return p_rectTransform.TEST_DashAnchoredPosition(new Vector2(posX, p_finalPosition),
                p_duration,
                p_easeType,
                p_delay);
        }

        public static DashTween DashSizeDelta(this RectTransform p_rectTransform,
            Vector2 p_finalSize,
            float p_duration,
            EaseType p_easeType = EaseType.LINEAR,
            float p_delay = 0f)
        {
            var original = p_rectTransform.sizeDelta;
            var tween = DashTween.To(p_rectTransform, 0f, 1f, p_duration);

            if (p_delay >= 0f)
            {
                tween.SetDelay(p_delay);
            }

            tween.OnInternalUpdate(delta =>
            {
                if (DoNullChecks && p_rectTransform == null)
                {
                    return;
                }

                p_rectTransform.sizeDelta = new Vector2(
                    DashTween.EaseValue(original.x, p_finalSize.x, delta, p_easeType),
                    DashTween.EaseValue(original.y, p_finalSize.y, delta, p_easeType));
            });
            tween.Start();
            return tween;
        }

        public static DashTween DashScale(this RectTransform p_rectTransform,
            Vector2 p_finalScale,
            float p_duration,
            EaseType p_easeType = EaseType.LINEAR,
            float p_delay = 0f)
        {
            var original = p_rectTransform.localScale;
            var tween = DashTween.To(p_rectTransform, 0f, 1f, p_duration);

            if (p_delay >= 0f)
            {
                tween.SetDelay(p_delay);
            }

            tween.OnInternalUpdate(delta =>
            {
                if (DoNullChecks && p_rectTransform == null)
                {
                    return;
                }

                p_rectTransform.localScale = new Vector3(
                    DashTween.EaseValue(original.x, p_finalScale.x, delta, p_easeType),
                    DashTween.EaseValue(original.y, p_finalScale.y, delta, p_easeType),
                    1f);
            });
            tween.Start();
            return tween;
        }
    }
}