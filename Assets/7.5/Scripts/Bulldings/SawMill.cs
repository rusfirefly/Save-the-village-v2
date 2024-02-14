public class SawMill : Mining
{
    private void Awake()
    {
        SetCycleMining(_playerData.timeWoodMine);
    }
}
