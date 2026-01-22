using HenryMod.Survivors.Henry.SkillStates;

namespace HenryMod.Survivors.Henry
{
    public static class HenryStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(Punch));

            Modules.Content.AddEntityState(typeof(RapidPunch));

            Modules.Content.AddEntityState(typeof(BaseDash));

            Modules.Content.AddEntityState(typeof(PunchDash));

            Modules.Content.AddEntityState(typeof(StopTime));

            Modules.Content.AddEntityState(typeof(TimeStopFreezeState));

            Modules.Content.AddEntityState(typeof(StarFinger));
        }
    }
}
