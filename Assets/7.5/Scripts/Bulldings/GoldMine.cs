public class GoldMine : Mining
{
    private void Awake()
    {
        SetCycleMining(_playerData.timeGoldMine);
    }
}
