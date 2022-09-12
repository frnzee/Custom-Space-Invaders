using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Background : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Material _material;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        float offset = Time.time * _speed;
        _material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
