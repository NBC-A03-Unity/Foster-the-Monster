using System;
using static Enums;

public class BossEventSO : EventFuncSO
{

    public override Action Apply(Action complete, int value = 0)
    {
        return () => LoadingSceneController.LoadScene(SceneName.MovieScene_Boss);
    }
}
