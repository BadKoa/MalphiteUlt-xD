using System;
using GTA;
using GTA.Math;
using System.Windows.Forms;
using GTA.Native;



namespace npcAttack
{
    public class Malphite : Script
    {
        private Ped npc;
        private bool explosionTriggered = false;

        public Malphite()
        {
            this.Tick += onTick;
            this.KeyDown += onKeyDown;
        }

        private void onTick(object sender, EventArgs e)
        {
            
            if (npc != null)
            {
                
                float distance = Vector3.Distance(Game.Player.Character.Position, npc.Position);
                
                if (distance < 5f && !explosionTriggered)         
                {
                    Vector3 collisionPos = Game.Player.Character.Position;
                    World.AddExplosion(collisionPos, ExplosionType.BombStandard, 1f, 0.5f);
                    explosionTriggered = true;
                    npc.Task.FightAgainst(Game.Player.Character);
                    
                }

                else if (explosionTriggered == true)
                {
                    npc.Task.FightAgainst(Game.Player.Character);
                }
                else
                {
                    npc.ApplyForceRelative(new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f));
                }

            }


        }
        
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.O)

            {

                Vector3 spawn_Loc = Game.Player.Character.Position + (Game.Player.Character.ForwardVector * 12);

                npc = World.CreatePed(PedHash.Orleans, spawn_Loc);//orlean
                
                npc.IsExplosionProof = true;
                npc.IsFireProof= true;

                Function.Call(Hash.SET_RUN_SPRINT_MULTIPLIER_FOR_PLAYER, npc, 10f);

                Function.Call(Hash.TASK_FOLLOW_TO_OFFSET_OF_ENTITY, npc.Handle, Game.Player.Character.Handle, 0f, 0f, 0f, 10.0f, -1, 5.0f, 1);

                //npc.Weapons.Give(WeaponHash.RPG, 1, true, true);

                Function.Call(Hash._SET_MOVE_SPEED_MULTIPLIER, npc.Handle, 10f);
            }
            
            if(e.KeyCode == Keys.J)
            {
                foreach(Ped ped in World.GetAllPeds())
                {
                    if (ped.IsInVehicle() && !ped.IsPlayer)
                    {
                        ped.Task.VehicleChase(Game.Player.Character);
                    }
                }
            }
        }
    }
}



