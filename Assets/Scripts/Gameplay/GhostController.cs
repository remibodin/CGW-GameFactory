using Cgw.Assets;
using Cgw.Audio;
using Cgw.Scripting;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Cgw.Gameplay
{
    public class GhostController : Enemy
    {
        public float Life = 3.0f;

        public float ChaseDistance = 6.0f;
        public float ChaseSpeed = 2.0f;
        public bool ChaseMode = false;
        public bool ChargeMode = false;
        public float ChargeDistance = 1.0f;
        public float ChargeTime = 3.0f;
        public float ExplosionDamage = 1.0f;

        public bool IsSpiderTouched = false;

        public Color SpriteColor = Color.white;
        public AnimationCurve OpacityOverDistanceCurve;
        public float Opacity = 1.0f;

        public float ChargeTimer = 0.0f;
        public float SpiderTouchTimer = 0.0f;

        public ParticleSystem FlyingDotsParticles;

        private Rigidbody2D m_Rigidbody;
        private SpriteRenderer m_SpriteRenderer;
        private Animator m_Animator;
        private bool NoMove = false;
        private bool Dying = false;

        public void GhostIA()
        {
            if (Dying)
            {
                return;
            }

            Vector3 playerPosition = Player.Instance.transform.position + Vector3.up * 0.4f;
            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            Opacity = OpacityOverDistanceCurve.Evaluate(distanceToPlayer);

            if (NoMove)
            {
                return;
            }

            if (!ChaseMode && distanceToPlayer < ChaseDistance)
            {
                ChaseMode = true;
            }

            if (ChaseMode && !ChargeMode)
            {
                if (distanceToPlayer > ChargeDistance)
                {
                    ChargeMode = false;
                    float speed = ChaseSpeed;
                    if (IsSpiderTouched)
                    {
                        speed *= SpiderController.Instance.TouchSpeedMultiplier;
                    }
                    Move(playerPosition - transform.position, speed);
                }
                else
                {
                    ChargeMode = true;
                    ChargeTimer = ChargeTime;
                }
            }

            if (ChargeMode)
            {
                if (distanceToPlayer > ChargeDistance)
                {
                    ChargeMode = false;
                    ChargeTimer = 0.0f;
                }
                else if (Mathf.Approximately(ChargeTimer, 0.0f))
                {
                    Player.Instance.TakeDamage(ExplosionDamage, this);
                    CoroutineRunner.StartCoroutine(Die());
                }
            }
        }

        public void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            m_Animator = GetComponent<Animator>();

            Opacity = OpacityOverDistanceCurve.Evaluate(float.PositiveInfinity);
            //AudioManager.Instance.Play("Sounds/FANTOME_RODE_07_1");
        }

        public void Update()
        {
            ChargeTimer -= Time.deltaTime;
            ChargeTimer = MathF.Max(ChargeTimer, 0.0f);

            SpiderTouchTimer -= Time.deltaTime;
            SpiderTouchTimer = Mathf.Max(SpiderTouchTimer, 0.0f);

            if (Mathf.Approximately(SpiderTouchTimer, 0.0f) && IsSpiderTouched)
            {
                var emission = FlyingDotsParticles.emission;
                emission.enabled = false;
                IsSpiderTouched = false;
            }
            GhostIA();

            SpriteColor.a = Opacity;
            m_SpriteRenderer.color = SpriteColor;
        }

        public void LateUpdate()
        {
            m_Animator.SetBool("ChargeMode", ChargeMode);
        }

        public IEnumerator Die()
        {
            Dying = true;
            yield return new WaitForSeconds(0.7f);
            Destroy(gameObject);
        }

        public IEnumerator KnockBack(Vector2 directionFromPlayer)
        {
            NoMove = true;
            yield return new WaitForSeconds(0.32f);
            m_Rigidbody.AddForce(directionFromPlayer * 3.0f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1.0f);
            NoMove = false;
        }

        public void Move(Vector3 direction, float speed)
        {
            transform.Translate(speed * Time.deltaTime * direction.normalized);
        }

        public override void Attacked(float power)
        {
            if (Dying)
            {
                return;
            }

            Life -= power;
            Vector3 directionFromPlayer = (transform.position - Player.Instance.transform.position).normalized;
            CoroutineRunner.StartCoroutine(KnockBack(directionFromPlayer));
            if (Life <= 0.0f)
            {
                CoroutineRunner.StartCoroutine(Die());
            }
        }

        public override void OnCollisionWithPlayer()
        {
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
        }
    }
}