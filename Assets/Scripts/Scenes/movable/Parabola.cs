using UnityEngine;
using System.Collections;

public class Parabola : MonoBehaviour
{

    public GameObject target;

    public float speed = 10.0f;

    private float distanceToTarget;

    private bool isMove = true;

    void Start()
    {
        distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (isMove)
        {
            Vector3 targetPos = target.transform.position;

            this.transform.LookAt(targetPos);

            float angle = Mathf.Min(1, Vector3.Distance(this.transform.position, targetPos) / distanceToTarget) * 45;

            this.transform.rotation = this.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);

            float currentDist = Vector3.Distance(this.transform.position, target.transform.position);

            if (currentDist < 0.5f)
            {
                isMove = false;
            }
            this.transform.Translate(Vector3.forward * Mathf.Min(speed * Time.deltaTime, currentDist));
            yield return null;
        }
    }
}  
