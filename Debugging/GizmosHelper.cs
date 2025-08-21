using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SPToolkits.Debugging
{
    public class GizmosHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        private static GizmosHelper _Instance;
        public static GizmosHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    GameObject g = new GameObject("~GizmosRenderer");
                    _Instance = g.AddComponent<GizmosHelper>();
                }
                return _Instance;
            }
        }

        private Dictionary<string, Action> _drawCalls = new Dictionary<string, Action>();
        private List<string> _ids = new List<string>();

        /// <returns>A style with the given font style and text color</returns>
        public static GUIStyle GetStyle(FontStyle fontStyle, Color textColor, int fontSize) 
        {
            GUIStyle style = new GUIStyle();
            style.fontStyle = fontStyle;
            style.normal.textColor = textColor;
            style.fontSize = fontSize;
            return style;
        }

        /// <returns>A style with the given font style</returns>
        public static GUIStyle GetStyle(FontStyle fontStyle)
            => GetStyle(fontStyle, Handles.color, 14);

        /// <returns>A style with the given font color.</returns>
        public static GUIStyle GetStyle(Color fontColor)
            => GetStyle(FontStyle.Normal, fontColor, 14);

        /// <summary>
        /// <br>Registers the draw action to be called within OnDrawGizmos.
        /// <br/>If one already exists with the same id, it will be overwritten.</br>
        /// </summary>
        public static void Register(string id, Action drawAction)
        {
            if (!Instance._drawCalls.ContainsKey(id))
            {
                Instance._ids.Add(id);
                Instance._drawCalls.Add(id, drawAction);
            }
            else
                Instance._drawCalls[id] = drawAction;
        }

        /// <summary>
        /// Unregisters the draw action with the givens id.
        /// </summary>
        public static void Unregister(string id)
        {
            if(Instance._drawCalls.Remove(id))
                Instance._ids.Remove(id);
        }

        /// <summary>
        /// Clears all gizmos draw actions.
        /// </summary>
        public static void ClearDrawList()
        {
            Instance._drawCalls.Clear();
            Instance._ids.Clear();
        }

        private void OnDrawGizmos()
        {
            foreach (string id in Instance._ids)
                if (_drawCalls.ContainsKey(id))
                    _drawCalls[id]?.Invoke();
        }
#endif
    }
}