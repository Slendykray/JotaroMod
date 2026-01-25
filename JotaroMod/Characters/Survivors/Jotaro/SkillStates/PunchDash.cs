using EntityStates;
using EntityStates.Merc;
using JotaroMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
using System.Linq;

using UnityEngine.Networking;
using JotaroMod.Survivors.Jotaro.Components;
using EntityStates.Loader;

namespace JotaroMod.Survivors.Jotaro.SkillStates
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