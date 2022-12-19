/*
 *	Created by:  Peter @sHTiF Stefcek
 */

using UnityEditor;
using UnityEngine;

namespace Dash.Editor
{
    [CustomEditor(typeof(DashTweenDebug))]
    public class DashTweenDebugInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Label("Active Tweens: "+DashTween._activeTweens.Count);
            GUILayout.Label("Dirty Tweens: "+DashTween._dirtyTweens.Count);
            GUILayout.Label("Pooled Tweens: "+DashTween._pooledTweens.Count);
        }
    }
}