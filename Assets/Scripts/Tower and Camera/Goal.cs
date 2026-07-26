using UnityEngine;

namespace Tower_and_Camera
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed = 0.5f;
        private Renderer _rend;

        void Start()
        {
            _rend = GetComponent<Renderer>();
        }

        void Update()
        {
            // Smoothly shifts the texture horizontally over time
            float offset = Time.time * scrollSpeed;
            _rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        }
    }
}