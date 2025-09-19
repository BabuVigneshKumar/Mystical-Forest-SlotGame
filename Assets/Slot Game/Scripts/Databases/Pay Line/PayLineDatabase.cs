using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaylineData", menuName = "SlotMachine/PaylineDatabase")]

public class PayLineDatabase : ScriptableObject
{
    public List<PaylineData> paylines;

}
