using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected ICatchBehaviour CatchBehaviour;
    protected ISpecialBehaviour SpecialBehaviour;

    public void PerformCatch()
    {
        CatchBehaviour.Catch();
    }

    public void PerformSpecial()
    {
        SpecialBehaviour?.Special();
    }
}
