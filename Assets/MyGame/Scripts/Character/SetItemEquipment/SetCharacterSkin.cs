using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterSkin : SetItemEquipment
{
    [SerializeField] private SkinDataSO skinData;
    

    protected override async void SetSkin()
    {
        if (skinParent.childCount > 0)
        {
            Destroy(skinParent.GetChild(0).gameObject);
        }

        var equipedIndex = PlayerPrefs.GetInt("CharEquipedIndex", 0);

        skin = Instantiate(
            skinData.GetSkinInfoByIndex(equipedIndex).itemPrefab,
            skinParent
        );
        skin.transform.SetAsFirstSibling();

        //GameEvent.OnChangeSkin?.Invoke();

        skin.name = "Model";
        await UniTask.Yield();

        if (animator != null)
        {
            animator.Rebind();
            //animator.Update(0f);
        }
    }

    protected override void SetStatBonus()
    {
        var skinEquipedIndex = PlayerPrefs.GetInt("CharEquipedIndex", 0);
        var statBonus = skinData.GetStatBonusByIndex(skinEquipedIndex);

        //speed *= statBonus.speed;
        //jumpForce *= statBonus.jumpForce;
        //health.maxHealth *= statBonus.hp;
        //health.ResetHealth();
    }

    protected override void SetStat() { }
}
