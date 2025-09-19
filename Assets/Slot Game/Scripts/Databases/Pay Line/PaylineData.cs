
[System.Serializable]
public class PaylineData
{
    public int lineNumber;
    public int[] positions; 
    public string description;
    public PaylineType type;
}
public enum PaylineType
{
    Horizontal,
    Diagonal,
    Zigzag,
    Wave,
    Complex,
    Slope
}
