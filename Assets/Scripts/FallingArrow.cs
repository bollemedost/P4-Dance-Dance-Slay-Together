using UnityEngine;

public class FallingArrow : MonoBehaviour
{
    [SerializeField] public float arrowSpeed = 2f; // Editable in Unity Inspector

    private bool isFalling = false;

    public void StartFalling()
    {
        isFalling = true;
    }

    void Update()
    {
        if (isFalling)
        {
            transform.position += Vector3.down * arrowSpeed * Time.deltaTime;
        }

        if (transform.position.y < -6f) // Adjust based on your scene
        {
            Debug.Log("Arrow destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
