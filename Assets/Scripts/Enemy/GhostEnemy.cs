using UnityEngine;
using System.Collections;

public class GhostEnemy : MonoBehaviour
{
    public bool isInvisible = true;
    public float invisibleDuration = 3f;
    public float visibleDuration = 4f;

    private Renderer[] renders;

    void Start()
    {
        renders = GetComponentsInChildren<Renderer>();
        StartCoroutine(GhostCycle());
    }

    IEnumerator GhostCycle()
    {
        while (true)
        {
            
            isInvisible = true;
            SetVisible(false);
            yield return new WaitForSeconds(invisibleDuration);

            
            isInvisible = false;
            SetVisible(true);
            yield return new WaitForSeconds(visibleDuration);
        }
    }

    void SetVisible(bool state)
    {
        foreach (Renderer r in renders)
            r.enabled = state;
    }
}
