using UnityEngine;

namespace SPToolkits.Movement
{
    [CreateAssetMenu(fileName = "PlayerForcedown", menuName = "SPToolkits/Motion Suppliers/"+ nameof(PlayerForcedown))]
    public class PlayerForcedown: MotionSupplier
    {
        protected override void _Tick(float deltaTime, RuntimeControlContext ctx)
        {
        }
    }
}
            