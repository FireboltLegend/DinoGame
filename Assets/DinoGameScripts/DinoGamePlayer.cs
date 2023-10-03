using UnityEngine;

public class DinoGamePlayer : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void Update()
    {
        
    }
}
