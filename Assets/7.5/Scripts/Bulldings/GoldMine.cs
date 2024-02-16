
public class GoldMine : Mining
{

    private void Awake()
    {
        SetCycleMining(_gameSetup.timeGoldMine);
    }
}
