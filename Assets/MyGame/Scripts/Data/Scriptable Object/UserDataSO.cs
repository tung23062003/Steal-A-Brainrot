using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New User Data", menuName = "SO/User Data", order = 4)]
public class UserDataSO : ScriptableObject
{
    public int characterEquipedIndex;
    public int wingsEquipedIndex;
    public List<int> petsEquipedIndex;
}
