public class HarpoonThrow : Weapon
{
    public HarpoonThrow()
    {
        CatchBehaviour = new RangedCatch();
        SpecialBehaviour = new HarpoonCatch();
    }
}
