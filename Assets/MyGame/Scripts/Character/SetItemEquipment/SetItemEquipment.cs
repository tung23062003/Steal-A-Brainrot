
using UnityEngine;

public abstract class SetItemEquipment : MonoBehaviour
{
    [SerializeField] protected Transform skinParent;
    [HideInInspector] public GameObject skin;

    protected Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        SetSkin();
    }

    protected abstract void SetSkin();

    protected abstract void SetStatBonus();
    protected abstract void SetStat();
}
