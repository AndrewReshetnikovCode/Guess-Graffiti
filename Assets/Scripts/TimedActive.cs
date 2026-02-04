using System.Collections;
using UnityEngine;

public class TimedActive : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float activeTime = 1f;

    public void Activate()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        target.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        target.SetActive(false);
    }
}
