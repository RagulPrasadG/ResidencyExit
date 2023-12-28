using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class AICarController : MonoBehaviour
{
    public List<CarPath> wayPoints;
    public int carIndex = 0;
    private Vector3 currentPatrolPoint;
    private int currentpatrolPointIndex = 1;

    bool isReverse = false;
    public bool canMove = true;
    private Animator animator;
    private void Awake()
    {
        wayPoints = new List<CarPath>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        currentPatrolPoint = GetPatrolPoint();
        PatrolAICar();
    }

    public void PatrolAICar()
    {
        if (!canMove)
            return;

        Vector3 direction = (currentPatrolPoint - this.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(this.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(this.transform.DOMove(currentPatrolPoint, 3f));
        sequence.Play();
        sequence.onPlay += () => {
            animator.SetBool("isMoving", true);
            canMove = false;
        };

        sequence.onComplete += () =>
        {
            Debug.Log("Completed!");
            canMove = true;
            animator.SetBool("isMoving", false);
            if (currentpatrolPointIndex < wayPoints.Count && !isReverse)
            {
                currentpatrolPointIndex++;
                isReverse = false;
            }
            else
            {
                currentpatrolPointIndex--;
                if(currentpatrolPointIndex <= 0)
                {
                    isReverse = false;
                    currentpatrolPointIndex = 1;
                    PatrolAICar();
                    return;
                }
                isReverse = true;
            }
            currentPatrolPoint = GetPatrolPoint();
            animator.SetBool("isMoving", false);
            PatrolAICar();
        };
    }

    public Vector3 GetPatrolPoint()
    {
        foreach(CarPath path in wayPoints)
        {
            if(path.index == currentpatrolPointIndex)
            {
                return path.pathPosition;
            }
        }
        return Vector3.zero;
    }

}


[System.Serializable]
public struct CarPath
{
    public CarPath(int index,Vector3 pathPosition)
    {
        this.index = index;
        this.pathPosition = pathPosition;
    }
    public int index;
    public Vector3 pathPosition;
}