using System;

public class PlatformerPlayerEvents {

    public static Action<PlayerStateInfo, PlayerStateInfo> StateChanged;
    public static Action<float> MoveAmountChanged;
    public static Action<int> Turned;

}