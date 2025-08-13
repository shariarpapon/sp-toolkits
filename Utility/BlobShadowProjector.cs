using UnityEngine;

namespace SPToolkits.Utility
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class BlobShadowProjector : MonoBehaviour
    {
        [Header("Projection Settings")]
        public LayerMask groundLayer;
        public float maxProjectionDistance = 5f;
        public float fadeDistance = 2f; // how far below before fade/hide
        public Vector2 scaleFacBounds = new Vector2(0.3f, 1.0f);

        private Mesh mesh;
        private Vector3[] originalVertices;
        private Vector3[] deformedVertices;
        private Renderer rend;

        void Start()
        {
            mesh = GetComponent<MeshFilter>().mesh;
            originalVertices = mesh.vertices;
            deformedVertices = new Vector3[originalVertices.Length];
            rend = GetComponent<MeshRenderer>();
        }

        void LateUpdate()
        {
            bool anyHit = false;
            float lowestHitY = float.MaxValue;

            for (int i = 0; i < originalVertices.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(originalVertices[i]);
                if (Physics.Raycast(worldPos + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, maxProjectionDistance, groundLayer))
                {
                    deformedVertices[i] = transform.InverseTransformPoint(hit.point + (hit.normal * 0.05f));
                    anyHit = true;
                    if (hit.point.y < lowestHitY) 
                        lowestHitY = hit.point.y;
                }
                else
                {
                    deformedVertices[i] = originalVertices[i];
                }
            }

            mesh.vertices = deformedVertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            if (!anyHit)
            {
                rend.enabled = false;
                return;
            }

            float drop = transform.position.y - lowestHitY;
            rend.enabled = drop <= fadeDistance;

            float dropRatio = drop / fadeDistance;
            transform.localScale = Vector3.one * Mathf.Lerp(scaleFacBounds.x, scaleFacBounds.y, dropRatio);
        }
    }
}