using UnityEngine;

namespace MiniRpg.Actor.Behaviour
{
    public class Move : MonoBehaviour
    {
        [Header("Callback")]
        [SerializeField] UnityEngine.Events.UnityEvent m_OnEnable = new UnityEngine.Events.UnityEvent();
        [SerializeField] UnityEngine.Events.UnityEvent m_OnDisable = new UnityEngine.Events.UnityEvent();

        protected virtual void OnEnable()
        {
            if (m_OnEnable != null) m_OnEnable.Invoke();
        }

        protected virtual void OnDisable()
        {
            if (m_OnDisable != null) m_OnDisable.Invoke();
        }
    }
}