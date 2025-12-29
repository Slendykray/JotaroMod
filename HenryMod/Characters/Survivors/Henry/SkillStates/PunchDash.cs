
namespace HenryMod.Survivors.Henry.SkillStates
{
    public class PunchDash : BaseDash
    {
        public override void OnEnter()
        { 
            base.OnEnter();

            nextState = new Punch();
        }
    
    }
}