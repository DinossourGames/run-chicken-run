using System;
using Models;
using UnityEngine;

namespace Network
{
    public class RemotePlayer : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _movment;

        private void Start() => _rb = GetComponent<Rigidbody2D>();

        public void UpdatePosition(NetworkedPlayer update)
        {
            _movment = new Vector2(update.x, update.y);
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_movment);
        }
    }
}