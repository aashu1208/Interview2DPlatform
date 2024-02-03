using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        private float moveSpeed = 1f; 
        public LayerMask ground;
        public LayerMask wall;

        private Rigidbody2D rb; 
        public Collider2D triggerCollider;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

       /* void Update()
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }*/

        void FixedUpdate()
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (!triggerCollider.IsTouchingLayers(ground) || triggerCollider.IsTouchingLayers(wall))
            {
                Flip();
            }
        }
        
        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
        }
    }
}
