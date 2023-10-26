using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoreComponent : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Transform _neck;
    [SerializeField] private ParticleSystem _neckFountain;
    [SerializeField] private ParticleSystem _bloodPuff;
    [SerializeField] private Animator _animator;

    private Rigidbody[] _ragdollRBs;

    public void Awake() {
        _character.OnDie += (x) => DeathAnimation(x);

        _ragdollRBs = GetComponentsInChildren<Rigidbody>();
    }

    public void DeathAnimation(Vector3 killerForward) {
        _bloodPuff.transform.forward = killerForward;

        _neckFountain.Play();
        _neck.localScale = Vector3.zero;

        _animator.enabled = false;

        foreach (var item in _ragdollRBs) {
            item.isKinematic = false;
            item.velocity = Vector3.zero;
            item.angularVelocity = Vector3.zero;
            item.AddExplosionForce(25f, transform.position + Vector3.up * .75f - killerForward * .25f, .4f, 0, ForceMode.Impulse);
        }

        DOVirtual.Float(.1f, 1f, 3f, x => Time.timeScale = x).SetEase(Ease.InCubic).SetUpdate(true);
    }
}
