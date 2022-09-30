public class Enemy : DestroyableDirectedModel
{
    public int Points { get; private set; }
    public void SetPoints(int points) => Points = points;
}