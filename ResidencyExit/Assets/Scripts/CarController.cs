using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CarController : MonoBehaviour
{
    [SerializeField] float carMoveSpeed;


    private bool canMove = true;
    private Animator animator;
    public LevelDataSO levelDataSO;
    private GameObject carClone;

    public AudioClip[] audioclips;
    private AudioSource source;

    private void Start()
    {
        carClone = LevelManager.instance.carInstance;
        source = carClone.GetComponent<AudioSource>();
        animator = carClone.GetComponent<Animator>();

        source.PlayOneShot(audioclips[0]);
        source.clip = audioclips[1];
        source.loop = true;
        source.PlayDelayed(1f);
    }

    public void MoveLeft()
    {
        if (!canMove) return;

        AudioManager.instance.PlayClick();
        animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetLeftEndPosition();
        if (endPosition == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - carClone.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(carClone.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(carClone.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            source.DOPitch(1.0f, 0.2f);
            animator.SetBool("isMoving", false);
        };
    }

    public void MoveRight()
    {
        if (!canMove) return;

        AudioManager.instance.PlayClick();
        animator.SetBool("isMoving", true);
        
        Vector3 endPosition = GridManager.instance.GetRightEndPosition();
        if (endPosition == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - carClone.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(carClone.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(carClone.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            source.DOPitch(1.0f, 0.2f);
            animator.SetBool("isMoving", false);

        };
    }

    public void MoveUp()
    {
        if (!canMove) return;

        AudioManager.instance.PlayClick();
        animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetUpEndPosition();
        if (endPosition == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - carClone.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(carClone.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(carClone.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            source.DOPitch(1.0f, 0.2f);
            animator.SetBool("isMoving", false);
        };
    }
    public void MoveDown()
    {
        if (!canMove) return;

        AudioManager.instance.PlayClick();
        animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetDownEndPosition();
        if (endPosition == Vector3.zero)
        {
            animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - carClone.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(carClone.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(carClone.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            source.DOPitch(1.0f, 0.2f);
            animator.SetBool("isMoving", false);
        };
    }

}
