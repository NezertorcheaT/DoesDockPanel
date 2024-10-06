using UnityEngine;

public interface IEntriable
{
    void Begin();
}

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] executables;

    private void Start()
    {
        foreach (var behaviour in executables)
        {
            if (behaviour.enabled || behaviour is not IEntriable entriable) continue;
            entriable.Begin();
            behaviour.enabled = true;
        }
    }
}