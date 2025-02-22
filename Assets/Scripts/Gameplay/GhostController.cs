using Cgw.Assets;
using Cgw.Audio;
using Cgw.Scripting;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Cgw.Gameplay
{
    public class GhostController : Enemy
    {
        public float Life = 3.0f;
        public float MaxOpacity = 1.0f;
        public float MinOpacity = 0.3f;
        public float MaxOpacityDistance = 3.0f;
        public float MinOpacityDistance = 8.0f;

        public float ChaseDistance = 6.0f;
        public float ChaseSpeed = 2.0f;
        public bool ChaseMode = false;
        public bool ChargeMode = false;
        public float ChargeDistance = 1.0f;
        public float ChargeTime = 3.0f;
        public float ExplosionDamage = 1.0f;

        public bool IsSpiderTouched = false;

        public float Opacity = 1.0f;
        public float ChargeCountdown = 0.0f;
        public float SpiderTouchTimer = 0.0f;

        private Rigidbody2D m_Rigidbody;
        private bool NoMove = false;

        public void UpdateOpacity(float distanceToPlayer)
        {
            if (distanceToPlayer < MinOpacityDistance)
            {
                if (distanceToPlayer < MaxOpacityDistance)
                {
                    Opacity = MaxOpacity;
                }
                else
                {
                    Opacity = Mathf.InverseLerp(MinOpacityDistance, MaxOpacityDistance, distanceToPlayer);
                }
            }
        }

        public void GhostIA()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position + Vector3.up);
            UpdateOpacity(distanceToPlayer);

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
                    Move((Player.Instance.transform.position + Vector3.up) - transform.position, speed);
                }
                else
                {
                    ChargeMode = true;
                    ChargeCountdown = ChargeTime;
                }
            }

            if (ChargeMode)
            {
                if (distanceToPlayer > ChargeDistance)
                {
                    ChargeMode = false;
                    ChargeCountdown = 0.0f;
                }
                else if (Mathf.Approximately(ChargeCountdown, 0.0f))
                {
                    Player.Instance.TakeDamage(ExplosionDamage, this);
                    CoroutineRunner.StartCoroutine(Die());
                }
            }
        }

        public void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();

            Opacity = MinOpacity;
            //AudioManager.Instance.Play("Sounds/FANTOME_RODE_07_1");
        }

        public void Update()
        {
            ChargeCountdown -= Time.deltaTime;
            ChargeCountdown = MathF.Max(ChargeCountdown, 0.0f);

            SpiderTouchTimer -= Time.deltaTime;
            SpiderTouchTimer = Mathf.Max(SpiderTouchTimer, 0.0f);

            if (Mathf.Approximately(SpiderTouchTimer, 0.0f))
            {
                IsSpiderTouched = false;
            }
            GhostIA();
        }

        public IEnumerator Die()
        {
            yield return new WaitForSeconds(0.7f);
            Destroy(gameObject);
        }

        public IEnumerator KnockBack(Vector2 directionFromPlayer)
        {
            yield return new WaitForSeconds(0.32f);
            m_Rigidbody.AddForce(directionFromPlayer * 3.0f, ForceMode2D.Impulse);
        }

        public void Move(Vector3 direction, float speed)
        {
            transform.Translate(speed * Time.deltaTime * direction);
        }

        public override void Attacked(float power)
        {
            Life -= power;
            NoMove = true;
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
            IsSpiderTouched = true;
            SpiderTouchTimer = SpiderController.Instance.TouchTime;
        }

        public override void OnCollisionWithDanger()
        {
        }
    }
}