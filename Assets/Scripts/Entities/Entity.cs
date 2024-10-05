using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual Vector3 Position => transform.position;
    public virtual Vector3 Forward => transform.forward;
    public virtual Vector3 Velocity => Vector3.zero;
    
    private readonly GameObject[] _colliders = new GameObject[8];

    protected virtual void Start()
    {
        var myCollider = gameObject.GetComponent<CapsuleCollider>();

        for (int i = 0; i < 8; i++)
        {
            var newCollider = new GameObject($"Collider_{i}");
            newCollider.layer = gameObject.layer;
            newCollider.transform.SetParent(transform);

            var col = newCollider.AddComponent<CapsuleCollider>();
            col.center = myCollider.center;
            col.radius = myCollider.radius;
            col.height = myCollider.height;
            col.direction = myCollider.direction;
            
            _colliders[i] = newCollider;
        }
        UpdateAreaColliders();
    }

    public Vector3 GetClosestPosition(Vector3 relativePosition)
    {
        if (this == null || gameObject == null)
        {
            Debug.LogWarning("Attempted to access a destroyed or null Entity.");
            return Vector3.zero; 
        }
        var bestDistance = (Position - relativePosition).sqrMagnitude;
        var bestPosition = Position;

        for (int i = 0; i < 8; i++)
        {
            if (this == null) 
            {
                return Vector3.zero;
            }
            var newDistance = (relativePosition - _colliders[i].transform.position).sqrMagnitude;
            if (this == null) 
            {
                return Vector3.zero;
            }
            if (newDistance < bestDistance)
            {
                bestDistance = newDistance;
                bestPosition = _colliders[i].transform.position;
            }
        }
        
        return bestPosition;
    }
    
    protected void UpdateAreaColliders()
    {
        // Vamos a mover los colliders secundarios de el boid para que esten "en otras pantallas".
            
        // collider principal => o
        // collider secundario => x
            
        // Cada seccion es una "pantalla" y asi nosotros solo con castear el OverlapSphere estariamos detectando.
            
        //  x | x | x
        // -----------
        //  x | o | x
        // -----------
        //  x | x | x
            
        // Conseguimos las dimensiones de area del manager, asi sabemos cuanto mover los colliders.
        var areaDimensions = AreaManager.Instance.Dimensions;
        var currentPosition = transform.position;
        var currentRotation = transform.rotation;
            
        // Muevo colliders en las posiciones arriba, abajo, izquierda, derecha.
/*        _colliders[0].transform.position = currentPosition + new Vector3(areaDimensions.x, 0, 0);
        _colliders[1].transform.position = currentPosition + new Vector3(-areaDimensions.x, 0, 0);
        _colliders[2].transform.position = currentPosition + new Vector3(0, 0, areaDimensions.y);
        _colliders[3].transform.position = currentPosition + new Vector3(0, 0, -areaDimensions.y);
            
        // Muevo colliders en las posiciones de las esquinas
        _colliders[4].transform.position = currentPosition + new Vector3(areaDimensions.x, 0, areaDimensions.y);
        _colliders[5].transform.position = currentPosition + new Vector3(areaDimensions.x, 0, -areaDimensions.y);
        _colliders[6].transform.position = currentPosition + new Vector3(-areaDimensions.x, 0, areaDimensions.y);
        _colliders[7].transform.position = currentPosition + new Vector3(-areaDimensions.x, 0, -areaDimensions.y);*/

        // Cambio la rotacion de todos los colliders a la mia.
        /*for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].transform.rotation = currentRotation;
        }*/
    }
    
    protected void UpdateAreaLimits()
    {
        var currentPosition = transform.position;
        var area = AreaManager.Instance.Dimensions;
        var halfArea = AreaManager.Instance.HalfDimensions;
        
        if(Mathf.Abs(currentPosition.x) < halfArea.x && Mathf.Abs(currentPosition.z) < halfArea.y) return;
        
        if (currentPosition.x > halfArea.x) currentPosition -= new Vector3(area.x, 0, 0);
        else if (currentPosition.x < -halfArea.x) currentPosition += new Vector3(area.x, 0, 0);
        
        if (currentPosition.z > halfArea.y) currentPosition -= new Vector3(0, 0, area.y);
        else if (currentPosition.z < -halfArea.y) currentPosition += new Vector3(0, 0, area.y);
        
        transform.position = currentPosition;
    }
}
