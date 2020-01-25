﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerScript : MonoBehaviour
{
    public enum State
    {
        Wait,
        Walk
    }

    //　目的地
    private Vector3 destination;
    //　巡回する位置の親
    [SerializeField]
    private Transform patrolPointsParent = null;
    //　巡回する位置
    private Transform[] patrolPositions;
    //　次に巡回する位置
    private int nowPatrolPosition = 0;
    //　エージェント
    private NavMeshAgent navMeshAgent;
    //　アニメーター
    private Animator animator;
    //　村人の状態
    private State state;
    //　待機した時間
    private float elapsedTime;
    //　待機する時間
    [SerializeField]
    private float waitTime = 5f;

    void OnEnable()
    {
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        //　巡回地点を設定
        patrolPositions = new Transform[patrolPointsParent.transform.childCount];
        for (int i = 0; i < patrolPointsParent.transform.childCount; i++)
        {
            patrolPositions[i] = patrolPointsParent.transform.GetChild(i);
        }
        SetState(State.Wait);
    }

    void Update()
    {
        //　見回り
        if (state == State.Walk)
        {
            //　エージェントの潜在的な速さを設定
            animator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);

            //　目的地に到着したかどうかの判定
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                SetState(State.Wait);
            }
            //　到着していたら一定時間待つ
        }
        else if (state == State.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(State.Walk);
            }
        }
    }

    //　村人の状態変更
    public void SetState(State state)
    {
        this.state = state;
        if (state == State.Wait)
        {
            elapsedTime = 0f;
            animator.SetFloat("Speed", 0f);
        }
        else if (state == State.Walk)
        {
            SetNextPosition();
            navMeshAgent.SetDestination(GetDestination());
        }
    }

    //　巡回地点を順に周る
    public void SetNextPosition()
    {
        SetDestination(patrolPositions[nowPatrolPosition].position);
        nowPatrolPosition++;
        if (nowPatrolPosition >= patrolPositions.Length)
        {
            nowPatrolPosition = 0;
        }
    }
    //　目的地を設定する
    public void SetDestination(Vector3 position)
    {
        destination = position;
    }

    //　目的地を取得する
    public Vector3 GetDestination()
    {
        return destination;
    }
}