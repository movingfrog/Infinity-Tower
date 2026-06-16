using System;
using System.Collections;
using UnityEngine;

public interface IAttack
{
    //공격 관련 변수
    bool isAttack { get; set; }
    Rigidbody2D rigid { get; set; }
    float AttackDamage { get; set; }
    float attackDelay { get; set; }

    //공격 관련 함수
    void resetAttack();
    void Attack();
    IEnumerator waitAttackCool(float delay, Action action);
}
