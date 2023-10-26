using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Zenject;

public class CameraComponent : MonoBehaviour {
    [SerializeField] private Camera _camera;

    [SerializeField] private float _camDistance;

    [SerializeField] private float _minCamAngle, _maxCamAngle;

    [SerializeField] private VolumeProfile _volumeProfile;

    [Inject] private GameLoopManager _gameLoopManager;
    [Inject] private TurnSequencer _turnSequencer;
    [Inject] private InputsSO _inputs;

    private bool _isInSequence = false;

    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";

    private void Awake() {
        _turnSequencer.OnKillStart += (x, y, t) => StartCoroutine(OnKillSequence(x, y, t));

        if (_volumeProfile.TryGet(out DepthOfField dof)) {
            MinFloatParameter distance = dof.focusDistance;
            distance.value = _camDistance;
        }
    }

    void Update() {
        HandleCamRotation();
    }

    private void HandleCamRotation() {
        if (_gameLoopManager.IsGameEnded) return;
 
        if (_isInSequence) return;

        if (Input.GetKey(_inputs.CamRotationButton)) {
            Vector3 mouseInput = new Vector3(-Input.GetAxis(MOUSE_Y), Input.GetAxis(MOUSE_X), 0);

            Vector3 startRotation = transform.eulerAngles;

            startRotation += mouseInput * _inputs.CamRotationSensetivity;

            startRotation.x = Mathf.Clamp(startRotation.x, _minCamAngle, _maxCamAngle);

            transform.eulerAngles = startRotation;
            
            transform.position = transform.rotation * Vector3.back * _camDistance;
        }
    }

    public IEnumerator OnKillSequence(Transform killer, Transform victim, float startDelay) {
        _isInSequence = true;

        yield return new WaitForSeconds(startDelay * .5f);

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        Vector3 middle = victim.transform.position + (killer.transform.position - victim.transform.position).normalized / 2;
        Vector3 offset = transform.position - middle;
        offset.y = 0;
        offset.Normalize();
        offset *= 2;
        offset += Vector3.up * .5f;

        Vector3 finalPosition = middle + offset;
        Quaternion finalRotation = Quaternion.LookRotation(middle + Vector3.up * .75f - finalPosition, Vector3.up);

        if (_volumeProfile.TryGet(out DepthOfField dof)) {
            MinFloatParameter distance = dof.focusDistance;
            distance.value = 1.85f;

        }
        
        transform.DOMove(finalPosition, .5f).SetEase(Ease.InOutCubic);
        transform.DORotateQuaternion(finalRotation, .5f).SetEase(Ease.InOutCubic);
        
        yield return new WaitForSeconds(startDelay * .5f);

        yield return new WaitForSeconds(.66f);

        transform.DOShakeRotation(.5f, 1f, 6, 90, true);

        yield return null;

        if (_gameLoopManager.IsGameEnded) yield break;

        yield return new WaitForSeconds(1.5f);

        if (_volumeProfile.TryGet(out dof)) {
            MinFloatParameter distance = dof.focusDistance;
            distance.value = _camDistance;
        }

        transform.DOMove(startPosition, 1f).SetEase(Ease.InOutCubic);
        transform.DORotateQuaternion(startRotation, 1f).SetEase(Ease.InOutCubic);

        yield return new WaitForSeconds(1f);

        _isInSequence = false;
    }


#if UNITY_EDITOR

    private void OnValidate() {
        transform.position = transform.rotation * Vector3.back * _camDistance;
    }

#endif
}
