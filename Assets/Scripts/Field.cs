using UnityEngine;

namespace AnimalSimulation
{
    public class Field : MonoBehaviour, IField
    {
        [SerializeField] private int InitialSize = 10;
        [SerializeField] private int InitialTextureTiles = 2;
        [SerializeField] private Transform _visuals;
        [SerializeField] private MeshFilter _planeMeshFilter;
        [SerializeField] private Renderer _planeRenderer;

        public Vector2Int Size { get; private set; }
        public Bounds Bounds => _planeRenderer.bounds; 

        public void Init(Vector2Int size)
        {
            Size = size;
            _visuals.localScale = new Vector3((float)Size.x / InitialSize, 1f, (float)Size.y / InitialSize);
            RefreshUV();
        }

        private void RefreshUV()
        {
            Mesh mesh = _planeMeshFilter.mesh;
            Vector2[] uvs = mesh.uv;
            Vector3[] vertices = mesh.vertices;

            Vector2 halfSize = new Vector2(0.5f * Size.x, 0.5f * Size.y);
            float initialHalfSize = InitialSize * 0.5f;
            Vector2 uvScale = halfSize / initialHalfSize / InitialTextureTiles; 

            for (var i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(vertices[i].x * uvScale.x, vertices[i].z * uvScale.y);
            }
            mesh.uv = uvs;
        }
    }
}