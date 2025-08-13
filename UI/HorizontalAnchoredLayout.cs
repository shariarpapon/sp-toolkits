using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using SPToolkits.UI;
[CustomEditor(typeof(HorizontalAnchoredLayout))]
public class HorizontalAnchoredLayoutEditor : Editor 
{
    private int _childCount = 0;
    private int _activeChildCount = 0;

    private HorizontalAnchoredLayout _instance;
    private void OnEnable()
    {
        _instance = (HorizontalAnchoredLayout)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (ChangeWasMade())
        {
            _instance.UpdateLayout();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"Total Elements: {_childCount}");
        EditorGUILayout.LabelField($"Active Elements: {_activeChildCount}");
    }

    private bool ChangeWasMade() 
    {
        int childCount = _instance.transform.childCount;
        if (childCount != _childCount) 
        {
            _childCount = childCount;
            return true;
        }

        int activeChildCount = GetActiveChildCount();
        if (activeChildCount != _activeChildCount) 
        {
            _activeChildCount = activeChildCount;
            return true;
        }

        return false;
    }

    private int GetActiveChildCount() 
    {
        int i = 0;
        foreach (Transform t in _instance.transform)
            if (t.gameObject.activeSelf)
                i++;
        return i;
    }
}

#endif

namespace SPToolkits.UI
{
    public class HorizontalAnchoredLayout : MonoBehaviour
    {
        [System.Serializable]
        internal enum Anchor { Left, Right }
        internal Anchor anchor = Anchor.Left;

        public float gap = 100;
        public bool ignoreInactive = true;
        public int ElementCount => transform.childCount;

        private RectTransform _transform => (RectTransform)transform;

        private void OnValidate()
        {
            UpdateLayout();
        }

        public void InstantiateElement(GameObject prefab) 
        {
            Instantiate(prefab, transform);
            UpdateLayout();
        }

        public void DestroyLastElement() 
        {
            if (transform.childCount <= 0)
                return;
            Destroy(transform.GetChild(transform.childCount - 1).gameObject);
            UpdateLayout();
        }

        public void DestroyFirstElement()
        {
            if (transform.childCount <= 0)
                return;

            Destroy(transform.GetChild(0).gameObject);
            UpdateLayout();
        }

        public void DestroyAllElements() 
        {
            if (transform.childCount <= 0)
                return;

            foreach (Transform t in transform)
                Destroy(t.gameObject);
            UpdateLayout();
        }

        public void UpdateLayout() 
        {
            if (transform.childCount <= 0)
                return;

            int i = 0;
            float anchorDir = anchor == Anchor.Left ? 1.0f : -1.0f;
            float origin = anchor == Anchor.Left ? _transform.offsetMin.x : _transform.offsetMax.x;
            foreach (Transform child in transform)
            {
                if (ignoreInactive && !child.gameObject.activeSelf)
                    continue;
                RectTransform childTf = (RectTransform)child;
                Vector2 pos = childTf.position;
                pos.x = origin + (i * gap * anchorDir);
                childTf.position = pos;
                i++;
            }
        }
    }
}