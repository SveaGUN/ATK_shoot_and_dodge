using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bossのステート遷移情報が格納されているデータ

[CreateAssetMenu(menuName = "ScriptableObject/BossStateTransitionData")]
public class BossStateTransitionData : ScriptableObject
{
    public BossStateData[] transitions;
}
