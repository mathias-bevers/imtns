using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace CleanRoom.ScriptableStateMachine
{
    public class ScriptableStateContainer : MonoBehaviour
    {
        [field: SerializeField] public UnityEvent EnterEvent { get; private set; }
        [field: SerializeField] public UnityEvent ExitEvent { get; private set; }
        [Expandable, SerializeField] private ScriptableState stateData;
    }
}