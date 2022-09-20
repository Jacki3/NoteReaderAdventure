using System.Collections;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;

    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;

    protected Rigidbody2D rb2D;

    protected float inverseMoveTime;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDist = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDist > float.Epsilon)
        {
            Vector3 newPos =
                Vector3
                    .MoveTowards(rb2D.position,
                    end,
                    inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition (newPos);
            sqrRemainingDist = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    public virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        RaycastHit2D hit;

        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null) return;

        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent, xDir, yDir);
    }

    protected abstract void OnCantMove<T>(T Component, int x, int y)
        where T : Component;
}
