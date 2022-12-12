public class ScoreData
{
    private int _value;

    public int Value => _value;

    public void Increase(int value) => _value += value;
    public void Clear() => _value = 0;
}