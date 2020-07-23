using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ability parentAbility;

    public List<Vector3> points;
    public float duration = 1f;
    public GameObject graphic;
    public float colliderAOE = 0.5f;

    private bool play = false;
    private GameObject graphicsInstance;
    private float playTime;
    private CircleCollider2D hitbox;

    private void Start()
    {
        playTime = duration;
        hitbox = GetComponent<CircleCollider2D>();
        parentAbility = GetComponentInParent<Ability>();
        //graphicsInstance = Instantiate(graphic, points[0], transform.rotation, transform);
        Play();
    }

    void FixedUpdate()
    {
        if (play)
        {
            if (graphicsInstance == null)
            {
                graphicsInstance = Instantiate(graphic, transform.position, transform.rotation, transform);
                graphicsInstance.transform.localScale = transform.lossyScale;
            }
            //Make Projectile follow set path over time.
            int maxIndex = points.Count - 1;
            float deltaDuration = (duration - playTime) / duration;
            int index = Mathf.Clamp(Mathf.CeilToInt(maxIndex * deltaDuration), 0, maxIndex);
            //Debug.Log(maxIndex + " * " + deltaDuration + "= " + index);
            Vector3 destination = points[index];
            Vector3 startPos = new Vector3();
            if (index == 0)
            {
                startPos = points[0];
            }
            else
            {
                startPos = points[index - 1];
            }
            float speed = (deltaDuration * points.Count) - index;
            transform.localPosition = Vector3.Lerp(startPos, destination, speed);
            //Debug.Log(index + " : " + deltaDuration + " : " + speed + " : " + startPos + "->" + destination);
            playTime -= Time.deltaTime;
            if (playTime <= 0)
            {
                playTime = duration;
                End();
            }
        }
    }

    public void Pause(bool val)
    {
        play = val;
        hitbox.attachedRigidbody.simulated = false;
    }

    public void End()
    {
        play = false;
        playTime = duration;
        graphicsInstance.GetComponent<ParticleSystem>().Stop();
        Destroy(graphicsInstance, 2);
        graphicsInstance = null;
        hitbox.attachedRigidbody.simulated = false;
        Destroy(this.gameObject, 1);
    }

    public void Play()
    {
        play = true;
        playTime = duration;
        hitbox.attachedRigidbody.simulated = true;
        transform.localPosition = new Vector3();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("Hit an Enemy!");
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.Damage(parentAbility.parentPlayer, parentAbility.GetDamage());
            enemy.ApplyVelocity(new Vector3(1 * transform.lossyScale.x, 0, 0));
        }
        //TODO handle Destructible collisions
        if (collision.gameObject.tag == "Destructible")
        {
            Debug.Log("Hit a Destructible!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (points == null)
            return;
        Vector3 offset = transform.position;
        if (transform.parent != null)
            offset = transform.parent.position;
        Vector3 previousP = offset + points[0];
        foreach (var p in points)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(offset + p, 0.05f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(previousP, offset + p);
            previousP = offset + p;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, colliderAOE);
    }
}
