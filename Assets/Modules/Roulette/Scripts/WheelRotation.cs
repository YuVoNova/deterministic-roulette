using UnityEngine;

namespace Roulette
{
    public class WheelRotation : MonoBehaviour
    {
        private const float ROTATION_SPEED = -25f;
        
        private void Update()
        {
            transform.Rotate(Vector3.up, ROTATION_SPEED * Time.deltaTime);
        }
    }
}
