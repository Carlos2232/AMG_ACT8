using System;
using UnityEngine;

public enum ColliderType
{
    Player,
    Ground,
    Obstacle,
    FallCollider
}

public class AABBCollider : MonoBehaviour
{
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width = 1f;
    [SerializeField] public ColliderType type;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Register(this);
        }
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Unregister(this);
        }
    }

    private void Update()
    {
        var posX = this.transform.position.x + _width * .5f;
        var posY = this.transform.position.y + _length * .5f;
        
        var negX = this.transform.position.x - _width * .5f;
        var negY = this.transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);
        
        Color debugColor = type switch
        {
            ColliderType.Player => Color.green,
            ColliderType.Ground => Color.yellow,
            ColliderType.Obstacle => Color.red,
            ColliderType.FallCollider => Color.magenta,
            _ => Color.blueViolet
        };

        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), debugColor);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), debugColor);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), debugColor);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), debugColor);
    }

    public void OnCollision(AABBCollider other)
    {
        PlayerController player = GetComponent<PlayerController>();
        if (player != null)
        {
            player.HandleCollision(other);
        }
    }
}