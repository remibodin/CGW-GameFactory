using System.Collections;
using UnityEngine;

namespace Cgw.Gameplay
{
    public class SpiderController : SingleBehaviourInScene<SpiderController>
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

        public ParticleSystem FlyingDotsParticles;

        public float LaunchTimer = 0.0f;

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

        private IEnumerator Feedback_FailedLaunch()
        {
            var animator = GetComponent<Animator>();
            animator.Play("Aragna@FailLaunch");
            yield return null;
        }

        public void Launch(float cooldown)
        {
            if (CanLaunch && Life > 0)
            {
                var player = Player.Instance;
                var enemy = CheckLaunch(player.transform.position + Vector3.up * FollowHeight, player.Facing, LaunchRange);
                if (enemy != null)
                {
                    var emission = FlyingDotsParticles.emission;
                    emission.enabled = false;
                    CanLaunch = false;
                    LaunchTimer = cooldown;
                    HasTarget = true;
                    Target = enemy;
                    Life = Life - LifePerLaunch;
                }
                else
                {
                    HasTarget = false;
                    Target = null;
                    TargetPoint = player.transform.position + player.Facing * LaunchRange;
                    CoroutineRunner.StartCoroutine(Feedback_FailedLaunch());
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
                    enemy.OnCollisionWithSpider();
                    HasTarget = false;
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
                    enemy.OnCollisionWithSpider();
                    HasTarget = false;
                }
            }
        }

        public void Update()
        {
            LaunchTimer -= Time.deltaTime;
            LaunchTimer = Mathf.Max(0.0f, LaunchTimer);
            
            if (Mathf.Approximately(LaunchTimer, 0.0f) && !CanLaunch)
            {
                CanLaunch = true;
                var emission = FlyingDotsParticles.emission;
                emission.enabled = true;
            }

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
                    Vector3 targetCenter = Target.GetComponent<Collider2D>().bounds.center;
                    MoveTowards(targetCenter, LaunchSpeed);
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
