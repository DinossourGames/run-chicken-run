using UnityEngine;

namespace Models
{
    public class NetworkedPlayer
    {
        public float x;
        public float y;
        
        public bool IsFacingRight;

        public NetworkedPlayer(Vector2 position, bool isFacingRight)
        {
            x = position.x;
            y = position.y;
            IsFacingRight = isFacingRight;
        }

        public NetworkedPlayer()
        {
            
        }
    }
}