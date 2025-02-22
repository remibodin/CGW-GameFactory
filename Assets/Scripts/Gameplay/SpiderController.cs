using Cgw.Assets;
using Cgw.Scripting;
using UnityEditor.Rendering.Analytics;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class SpiderController : SingleBehaviour<SpiderController>
    {
        public float Life = 3;
        public float LifePerLaunch = 0.5f;

        public float FollowSpeed = 2.0f;
        public float FollowDistance = 0.1f;
        public float FollowHeight = 0.6f;

        public bool CanLaunch = true;
        public float LaunchRange = 6.0f;
        public float LaunchSpeed = 4.0f;

        public ContactFilter2D ContactFilter;
        public float TouchTime = 3.0f;
        public float TouchSpeedMultiplier = 0.1f;

        public bool HasTarget = false;
        public Enemy Target = null;
        public Vector3 TargetPoint = Vector3.zero;

        public void Start()
        {
            var player = Player.Instance;

            if (player != null)
            {
                var targetPosition = player.transform.position + Vector3.up * FollowHeight;
                if (player.Facing.x > 0.0f)
                {
                    targetPosition += Vector3.left * FollowDistance;
                }
                else
                {
                    targetPosition += Vector3.right * FollowDistance;
                }
                transform.position = targetPosition;
            }
        }

        public void MoveFollow(Vector3 destination, float speed)
        {
            transform.position = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
        }

        public void MoveTowards(Vector3 destination, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }

        public Enemy CheckLaunch(Vector3 origin, Vector3 direction, float range)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (Physics2D.Raycast(origin, direction, ContactFilter, hits, range) > 0)
            {
                return hits[0].collider.GetComponent<Enemy>();
            }
            return null;
        }

        public void Launch()
        {
            if (CanLaunch && Life > 0)
            {
                var player = Player.Instance;
                var enemy = CheckLaunch(player.transform.position + Vector3.up * FollowHeight, player.Facing, LaunchRange);
                if (enemy != null)
                {
                    HasTarget = true;
                    Target = enemy;
                    Life = Life - LifePerLaunch;
                }
                else
                {
                    HasTarget = false;
                    Target = null;
                    TargetPoint = player.transform.position + player.Facing * LaunchRange;
                }
            }
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                var enemy = collision.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    HasTarget = false;
                    enemy.OnCollisionWithSpider();
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                var enemy = collision.GetComponent<Enemy>();
                if (enemy != null)
                {
                    HasTarget = false;
                    enemy.OnCollisionWithSpider();
                }
            }
        }

        public void Update()
        {
            var player = Player.Instance;
            var targetPosition = player.transform.position + Vector3.up * FollowHeight;
            if (!HasTarget && Vector3.Distance(transform.position, targetPosition) > FollowDistance)
            {
                if (transform.position.x < targetPosition.x)
                {
                    MoveFollow(targetPosition + Vector3.left * FollowDistance, FollowSpeed);
                }
                else
                {
                    MoveFollow(targetPosition + Vector3.right * FollowDistance, FollowSpeed);
                }
            }
            else if (HasTarget)
            {
                if (Target != null)
                {
                    MoveTowards(Target.transform.position, LaunchSpeed);
                }
                else
                {
                    MoveTowards(TargetPoint, LaunchSpeed);
                    if (Vector3.Distance(TargetPoint, transform.position) < 0.1f)
                    {
                        HasTarget = false;
                    }
                }
            }
        }
    }
}
