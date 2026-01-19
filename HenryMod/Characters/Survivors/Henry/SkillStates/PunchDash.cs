using EntityStates;
using EntityStates.Merc;
using HenryMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
using System.Linq;

using UnityEngine.Networking;
using HenryMod.Survivors.Henry.Components;
using EntityStates.Loader;

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