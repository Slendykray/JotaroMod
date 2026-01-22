using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.Components
{
    public class MenuSound: MonoBehaviour
    {
        public void PlayMenuSound()
        {
            Util.PlaySound("Play_Menu_Random", gameObject);
        }

    } 
}
