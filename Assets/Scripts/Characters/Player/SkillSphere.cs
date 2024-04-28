using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class SkillSphere : MonoBehaviour
{
    private List<(Enemy, float)> targetList = new();
    public float attackInterval;

    private void Update()
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i].Item1 != null && (Time.time - targetList[i].Item2) >= attackInterval)
            {
                targetList[i].Item1.Damaged(1, targetList[i].Item1.transform.position, targetList[i].Item1.transform.position - transform.position);
                targetList[i] = (targetList[i].Item1, Time.time);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<Enemy>();
        if (target != null)
        {
            targetList.Add((target, Time.time));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.gameObject.GetComponent<Enemy>();
        if (target != null)
        {
            targetList.RemoveAll(x => x.Item1 == target);
        }
    }

    private void OnDisable()
    {
        targetList.Clear();
    }

}
