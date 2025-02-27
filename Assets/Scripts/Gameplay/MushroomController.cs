 using System;
using System.Collections;
using UnityEngine;

using FMODUnity;

namespace Cgw.Gameplay
{
    public class MushroomController : Enemy
    {
        public float Life = 1.0f;
        public float AttackPower = 1.0f;

        public float ChaseRange = 1.5f;
        public float ChaseSpeed = 0.8f;
        public bool ChaseMode = false;

        public bool IsSpiderTouched = false;
        public bool NoMove = false;
        public bool Dying = false;
        public bool TouchedPlayer = false;

        public GameObject BoomParticlePrefab;
        public ParticleSystem FlyingDotsParticles;

        public ContactFilter2D TerrainContactFilter;
        public float SpiderTouchTimer = 0.0f;

        public StudioEventEmitter onDeathFmodEvent;
        public StudioEventEmitter onHitFmodEvent;

        private Collider2D m_Collider;
        private Rigidbody2D m_Rigidbody;
        private Animator m_Animator;
        private Vector2 m_Facing;

        public void StartDying()
        {
            Dying = true;
            NoMove = true;
        }

        public void Die()
        {
            if (TouchedPlayer)
            {
                Player.Instance.TakeDamage(AttackPower, this);
            }
            Instantiate(BoomParticlePrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity);
            Destroy(gameObject);
            onDeathFmodEvent.Play();
        }

        public IEnumerator Knockback(Vector3 directionFromPlayer)
        {
            yield return new WaitForSeconds(0.32f);
            onHitFmodEvent.Play();
            m_Rigidbody.AddForce((Vector3.up + directionFromPlayer) * 3.0f, ForceMode2D.Impulse);
        }

        public override void Attacked(float power)
        {
            if (Dying)
            {
                return;
            }

            Life -= power;
            NoMove = true;
            Vector3 directionFromPlayer = (transform.position - Player.Instance.transform.position).normalized;
            CoroutineRunner.StartCoroutine(Knockback(directionFromPlayer));
            if (Life <= 0.0f)
            {
                // AudioManager.Instance.Play("Sounds/CHAMPI_DEGONFLER_06_1");
                TouchedPlayer = false;
                StartDying();
            }
        }

        public void Start()
        {
            m_Collider = GetComponent<Collider2D>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_Animator = GetComponent<Animator>();
        }

        public void Update()
        {
            SpiderTouchTimer -= Time.deltaTime;
            SpiderTouchTimer = Mathf.Max(SpiderTouchTimer, 0.0f);

            if (Mathf.Approximately(SpiderTouchTimer, 0.0f) && IsSpiderTouched)
            {
                var emission = FlyingDotsParticles.emission;
                emission.enabled = false;
                IsSpiderTouched = false;
            }
            MushroomIA();
        }

        public void LateUpdate()
        {
            m_Animator.SetBool("Dying", Dying);
        }

        public void Move(float direction, float speed)
        {
            m_Facing = (Vector2.right * direction).normalized;

            Vector2 translation = speed * Time.deltaTime * m_Facing;
            var hits = new RaycastHit2D[1];

            if (m_Collider.Cast(m_Facing, TerrainContactFilter, hits, speed * Time.deltaTime) > 0)
            {
                Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
                translation = hits[0].distance * m_Facing;
            }

            transform.Translate(translation);
        }

        public void MushroomIA()
        {
            if (NoMove || Dying)
            {
                return;
            }

            float distanceToPlayer = Vector3.Distance(Player.Instance.transform.position, transform.position);
            if (!ChaseMode && distanceToPlayer < ChaseRange)
            {
                ChaseMode = true;
            }

            if (ChaseMode)
            {
                float directionToPlayer = Player.Instance.transform.position.x - transform.position.x;
                if (directionToPlayer < 0.0f)
                {
                    directionToPlayer = -1.0f;
                }
                else
                {
                    directionToPlayer = 1.0f;
                }

                float speed = ChaseSpeed;
                if (IsSpiderTouched)
                {
                    speed *= SpiderController.Instance.TouchSpeedMultiplier;
                }
                Move(directionToPlayer, speed);
            }
        }

        public override void OnCollisionWithPlayer()
        {
            if (!IsSpiderTouched && !Dying)
            {
                // AudioManager.Instance.Play("Sounds/CHAMPI_POP_B-12_1");
                TouchedPlayer = true;
                StartDying();
            }
        }

        public override void OnCollisionWithSpider()
        {
            if (SpiderController.Instance.HasTarget)
            {
                var emission = FlyingDotsParticles.emission;
                emission.enabled = true;
                IsSpiderTouched = true;
                SpiderTouchTimer = SpiderController.Instance.TouchTime;
            }
        }

        public override void OnCollisionWithDanger()
        {
            if (!Dying)
            {
                StartDying();
            }
        }
    }
}
