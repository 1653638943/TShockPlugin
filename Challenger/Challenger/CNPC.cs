﻿using System.Collections.Generic;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using static TShockAPI.GetDataHandlers;

namespace Challenger
{
    public class CNPC
    {
        public NPC? npc;

        private int netID;

        private int index;

        public float[] ai;

        public int state;

        public int LifeMax;

        public HashSet<string> AccOfObsidian;

        private bool _isActive;

        public bool isActive
        {
            get
            {
                return npc != null && npc.netID == netID && ((Entity)npc).whoAmI == index && ((Entity)npc).active && _isActive;
            }
            set
            {
                _isActive = value;
            }
        }

        public CNPC()
        {
            npc = null;
            netID = 0;
            index = 0;
            ai = new float[8];
            state = 0;
            LifeMax = 0;
            AccOfObsidian = new HashSet<string>();
            isActive = false;
        }

        public CNPC(NPC? npc)
        {
            if (npc == null)
            {
                this.npc = null;
                netID = 0;
                index = 0;
                ai = new float[8];
                state = 0;
                LifeMax = 0;
                isActive = false;
            }
            else
            {
                this.npc = npc;
                netID = npc.netID;
                index = ((Entity)npc).whoAmI;
                ai = new float[8];
                state = 0;
                LifeMax = npc.life;
                isActive = ((Entity)npc).active;
            }
            AccOfObsidian = new HashSet<string>();
        }

        public CNPC(NPC? npc, float[] ai, int state)
        {
            if (npc == null)
            {
                this.npc = null;
                netID = 0;
                index = 0;
                ai = new float[8];
                state = 0;
                LifeMax = 0;
                isActive = false;
            }
            else
            {
                this.npc = npc;
                netID = npc.netID;
                index = ((Entity)npc).whoAmI;
                this.ai = ai;
                this.state = state;
                LifeMax = npc.life;
                isActive = ((Entity)npc).active;
            }
            AccOfObsidian = new HashSet<string>();
        }

        public virtual void NPCAI()
        {
        }

        public virtual int SetState()
        {
            return 0;
        }

        public virtual void OnHurtPlayers(PlayerDamageEventArgs e)
        {
        }

        public virtual void OnKilled()
        {
            if (AccOfObsidian.Count == 0)
            {
                return;
            }
            try
            {
                if (npc.boss || npc.rarity > 1 || npc.lifeMax > 7000)
                {
                    return;
                }
                foreach (string item in AccOfObsidian)
                {
                    TSPlayer[] players = TShock.Players;
                    foreach (TSPlayer val in players)
                    {
                        if (val != null && val.Active && val.Name == item)
                        {
                            npc.NPCLoot_DropItems(val.TPlayer);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public virtual void WhenHurtByPlayer(NpcStrikeEventArgs args)
        {
        }
    }
}