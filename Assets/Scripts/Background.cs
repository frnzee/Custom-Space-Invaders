using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;
    private Renderer _backgroundRenderer;
    void Start()
    {
        _backgroundRenderer= GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * speed;
        _backgroundRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
