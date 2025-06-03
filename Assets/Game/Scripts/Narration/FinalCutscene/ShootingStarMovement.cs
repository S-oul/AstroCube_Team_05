using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShootingStarMovement : StarMovement
{
    public Vector3 Target = new();
    [MinMaxSlider(0.6f, 2f)]
    [SerializeField] private Vector2 _minMaxMoveDuration;
    [SerializeField] Ease _moveEase = Ease.Linear;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(StartPos, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);

    }
    private void Start()
    {
        StartCoroutine(MovementAnim());
    }
    protected override void UpdateMovement()
    {
    }

    IEnumerator MovementAnim()
    {
        yield return new WaitForSeconds(1f);
        yield return transform.DOMove(Target, Random.Range(_minMaxMoveDuration.x, _minMaxMoveDuration.y)).SetEase(_moveEase).WaitForCompletion();
        transform.DOScale(0f, 0.5f).WaitForCompletion();
        Destroy(gameObject, 5);
    }
}
