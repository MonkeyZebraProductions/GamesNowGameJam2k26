using UnityEngine;
public class CallAfterDelay : MonoBehaviour
{
    float delay;
    System.Action action;

    // Will never call this frame, always the next frame at the earliest
    public static CallAfterDelay Create(float delay, System.Action action)
    {
        CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
        cad.delay = delay;
        cad.action = action;
        return cad;
    }

    float age;

    void Update()
    {
        if (age > delay)
        {
            if (action == null) return;

            action();

            if (gameObject == null) return;

            Destroy(gameObject);
        }
    }
    void LateUpdate()
    {
        age += Time.deltaTime;
    }
}
