﻿/*
 * This file is part of the OpenNos Emulator Project. See AUTHORS file for Copyright information
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 */

using OpenNos.Data;
using OpenNos.Domain;
using System.Linq;
using System;

namespace OpenNos.GameObject
{
    public class Mate : MateDTO
    {
        #region Instantiation
        public Mate()
        {

        }

        public Mate(Character owner, short VNum, byte level, MateType matetype)
        {
            NpcMonsterVNum = VNum;
            Monster = ServerManager.GetNpc(VNum);
            Hp = Monster.MaxHP;
            Mp = Monster.MaxMP;
            Name = Monster.Name;
            MateType = matetype;
            Level = level;
            Loyalty = 1000;
            PositionY = (short)(owner.PositionY + 1);
            PositionX = (short)(owner.PositionX + 1);
            MapX = (short)(owner.PositionX + 1);
            MapY = (short)(owner.PositionY + 1);
            Direction = 2;
            CharacterId = owner.CharacterId;
            Owner = owner;
            GeneateMateTransportId();
        }

        #endregion

        #region Properties

        public int MateTransportId { get; set; }
        public short PositionX { get; set; }
        public short PositionY { get; set; }
        public bool IsSitting { get; set; }
        private NpcMonster monster;
        private Character owner;
        public NpcMonster Monster
        {
            get
            {
                if (monster == null)
                {
                    monster = ServerManager.GetNpc(NpcMonsterVNum);
                }
                return monster;
            }
            set { monster = value; }
        }

        public Character Owner
        {
            get
            {
                if (owner == null)
                {
                    owner = ServerManager.Instance.GetSessionByCharacterId(CharacterId).Character;
                }
                return owner;
            }
            set
            {
                owner = value;
            }
        }
        #endregion

        #region Methods
        public short MaxHp { get { return 200; } }
        public short MaxMp { get { return 200; } }

        public string GenerateScPacket()
        {
            switch (MateType)
            {
                case MateType.Partner:
                    return $"{NpcMonsterVNum} {MateTransportId} {Level} {Loyalty} {Experience} 991.0.0 996.0.0 -1 -1 0 0 1 0 142 174 232 4 70 0 73 158 86 158 69 0 0 0 0 0 2641 2641 1065 1065 0 285816 {Name.Replace(' ', '^')} {(Skin != 0 ? Skin : -1)} 0 -1 -1 -1 -1";

                case MateType.Pet:
                    return $"{NpcMonsterVNum} {MateTransportId} {Level} {Loyalty} {Experience} 0 0 {Monster.AttackUpgrade} {Monster.DamageMinimum} {Monster.DamageMaximum} {Monster.Concentrate} {Monster.CriticalChance} {Monster.CriticalRate} {Monster.DefenceUpgrade} {Monster.CloseDefence} {Monster.DefenceDodge} {Monster.DistanceDefence} {Monster.DistanceDefenceDodge} {Monster.MagicDefence} {Monster.FireResistance} {Monster.WaterResistance} {Monster.LightResistance} {Monster.DarkResistance} {Hp} {MaxHp} {Mp} {MaxMp} 0 15 {(CanPickUp ? 1 : 0)} {Name.Replace(' ', '^')} {(IsSummonable ? 1 : 0)}";
            }
            return string.Empty;
        }

        public string GenerateCMode(short MorphId)
        {
            return $"c_mode 2 {MateTransportId} {MorphId} 0 0";
        }


        /// <summary>
        /// Checks if the current character is in range of the given position
        /// </summary>
        /// <param name="xCoordinate">The x coordinate of the object to check.</param>
        /// <param name="yCoordinate">The y coordinate of the object to check.</param>
        /// <param name="range">The range of the coordinates to be maximal distanced.</param>
        /// <returns>True if the object is in Range, False if not.</returns>
        public bool IsInRange(int xCoordinate, int yCoordinate, int range)
        {
            return Math.Abs(PositionX - xCoordinate) <= range && Math.Abs(PositionY - yCoordinate) <= range;
        }
        public void GeneateMateTransportId()
        {
            int nextId = ServerManager.Instance.MateIds.Any() ? ServerManager.Instance.MateIds.Last() + 1 : 2000000;
            ServerManager.Instance.MateIds.Add(nextId);
            MateTransportId = nextId;
        }

        public override void Initialize()
        {
        }

        public string GenerateSay(string message, int type)
        {
            return $"say 2 {MateTransportId} 2 {message}";
        }
        public string GenerateRest()
        {
            IsSitting = !IsSitting;
            return $"rest 2 {MateTransportId} {(IsSitting ? 1 : 0)}";
        }
        public string GenerateIn()
        {
            return $"in 2 {NpcMonsterVNum} {MateTransportId} {(IsTeamMember ? PositionX : MapX)} {(IsTeamMember ? PositionY : MapY)} {Direction} {(int)(Hp / (float)MaxHp * 100)} {(int)(Mp / (float)MaxMp * 100)} 0 0 3 {CharacterId} 1 0 {(Skin!=0?Skin:-1)} {Name.Replace(' ', '^')} 0 -1 0 0 0 0 0 0 0 0";
        }
        public EffectPacket GenerateEff(int effectid)
        {
            return new EffectPacket
            {
                EffectType = 2,
                CharacterId = MateTransportId,
                Id = effectid
            };
        }
        public string GenerateOut()
        {
            return $"out 2 {MateTransportId}";
        }

        public string GenerateEInfo()
        {
            return $"e_info 10 {NpcMonsterVNum} {Level} {Monster.Element} {Monster.AttackClass} {Monster.ElementRate} {Monster.AttackUpgrade} {Monster.DamageMinimum} {Monster.DamageMaximum} {Monster.Concentrate} {Monster.CriticalChance} {Monster.CriticalRate} {Monster.DefenceUpgrade} {Monster.CloseDefence} {Monster.DefenceDodge} {Monster.DistanceDefence} {Monster.DistanceDefenceDodge} {Monster.MagicDefence} {Monster.FireResistance} {Monster.WaterResistance} {Monster.LightResistance} {Monster.DarkResistance} {Monster.MaxHP} {Monster.MaxMP} -1 {Name.Replace(' ', '^')}";
        }

        #endregion
    }
}