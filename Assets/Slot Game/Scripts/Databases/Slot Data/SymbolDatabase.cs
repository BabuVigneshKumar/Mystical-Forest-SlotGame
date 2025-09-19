using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolDatabase", menuName = "SlotGame/SymbolDatabase")]
public class SymbolDatabase : ScriptableObject
{
    public List<SymbolData> symbols;
}