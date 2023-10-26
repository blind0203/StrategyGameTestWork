using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum Team {
        Blue, Red
    }

    public Action<Vector3> OnDie;

    public Team CharacterTeam => _characterTeam;

    [SerializeField] private Team _characterTeam;

    [SerializeField] private Animator _animator;

    private int _attackTriggerHash = Animator.StringToHash("Attack");
    private int _dieTriggerHash = Animator.StringToHash("Die");
    private int _speedHash = Animator.StringToHash("Speed");

    private bool _isMoving = false;

    public void StartMove() {
        _isMoving = true;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine() {
        Vector3 prevPosition = transform.position;


        while (_isMoving) {
            float speed = (prevPosition - transform.position).magnitude / Time.deltaTime / 3;
            _animator.SetFloat(_speedHash, speed);
            prevPosition = transform.position;
            yield return null;
        }
    }

    public void StopMove() {
        _isMoving = false;
        _animator.SetFloat(_speedHash, 0);
    }

    public void Attack() {
        _animator.SetTrigger(_attackTriggerHash);
    }

    public void Die(Vector3 killerForward) {
        _animator.SetTrigger(_dieTriggerHash);

        OnDie?.Invoke(killerForward);
    }
}
