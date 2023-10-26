using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TurnSequencer : MonoBehaviour
{
    public Action OnSequenceStart;
    public Action<Transform, Transform, float> OnKillStart;
    public Action OnSequenceEnd;

    private FieldTileComponent _startTile, _finalTile;

    public void StartTurnSequence(FieldTileComponent startTile, FieldTileComponent finalTile) {
        _startTile = startTile;
        _finalTile = finalTile;

        Vector3 finalPosition = new Vector3(finalTile.Coordinates.x - 2, 0, finalTile.Coordinates.y - 2);
        Vector3 tempPosition = Vector3.zero;
        if (finalTile.CharacterOnTile != null) {
            tempPosition = finalPosition + (new Vector3(startTile.Coordinates.x - 2, 0, startTile.Coordinates.y - 2) -finalPosition).normalized;
        }

        Sequence sequence = new Sequence(
            startTile.CharacterOnTile, 
            finalTile.CharacterOnTile ?? null,
            tempPosition,
            finalPosition
            );

        StartCoroutine(SequenceRoutine(sequence));
    }

    private IEnumerator SequenceRoutine(Sequence sequence) {
        OnSequenceStart?.Invoke();

        Debug.Log("start sequence");

        Vector3 direction = sequence.FinalPosition - sequence.MovableCharacter.transform.position;

        sequence.MovableCharacter.transform.DORotateQuaternion(Quaternion.LookRotation(direction, Vector3.up), .5f).SetEase(Ease.InOutCubic).SetLink(gameObject);

        yield return new WaitForSeconds(.5f);

        float moveTime = 2;

        if (sequence.AttackableCharacter != null) {


            //sequence.AttackableCharacter.transform.DORotateQuaternion(Quaternion.LookRotation(-direction, Vector3.up), .5f).SetEase(Ease.InOutCubic);

            moveTime = (sequence.TempPosition - sequence.MovableCharacter.transform.position).magnitude / 3 + 1f;
            
            OnKillStart?.Invoke(sequence.MovableCharacter.transform, sequence.AttackableCharacter.transform, moveTime);

            sequence.MovableCharacter.transform.DOMove(sequence.TempPosition, moveTime).SetEase(Ease.InOutSine).SetLink(gameObject).OnStart(() => {
                sequence.MovableCharacter.StartMove();
            }).OnComplete(() => {
                sequence.MovableCharacter.StopMove();
            });

            yield return new WaitForSeconds(moveTime);
        }

        if (sequence.AttackableCharacter != null) {
            sequence.MovableCharacter.Attack();

            yield return new WaitForSeconds(.66f);

            sequence.AttackableCharacter.Die(sequence.MovableCharacter.transform.forward);

            yield return new WaitForSeconds(1.5f);
        }

        moveTime = (sequence.FinalPosition - sequence.MovableCharacter.transform.position).magnitude / 3 + 1f;

        sequence.MovableCharacter.transform.DOMove(sequence.FinalPosition, moveTime).SetEase(Ease.InOutSine).SetLink(gameObject).OnStart(() => {
            sequence.MovableCharacter.StartMove();
        }).OnComplete(() => {
            sequence.MovableCharacter.StopMove();
        });

        yield return new WaitForSeconds(moveTime);

        _finalTile.SetCharacterOnTile(_startTile.CharacterOnTile);
        _startTile.SetCharacterOnTile(null);

        yield return null;

        OnSequenceEnd?.Invoke();
    }
}

public class Sequence {
    public Character MovableCharacter;
    public Character AttackableCharacter;

    public Vector3 TempPosition;
    public Vector3 FinalPosition;

    public Sequence(Character movableCharacter, Character attackableCharacter, Vector3 tempPosition, Vector3 finalPosition) {
        MovableCharacter = movableCharacter;
        AttackableCharacter = attackableCharacter;
        TempPosition = tempPosition;
        FinalPosition = finalPosition;
    }
}
