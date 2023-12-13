public class CloseNet : Weapon
{
    public CloseNet()
    {
        CatchBehaviour = new MeleeCatch();
        SpecialBehaviour = new MeleeSpecial();
    }
}
