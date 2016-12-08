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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OpenNos.Domain;

namespace OpenNos.DAL.EF
{
    public class FamilyCharacter
    {
        #region Instantiation

        public FamilyCharacter()
        {
            Character = new HashSet<Character>();
        }

        #endregion

        #region Properties

        public FamilyAuthority Authority { get; set; }

        public virtual ICollection<Character> Character { get; set; }

        [MaxLength(255)]
        public string DailyMessage { get; set; }

        public int Experience { get; set; }

        public virtual Family Family { get; set; }

        public long FamilyCharacterId { get; set; }

        public long FamilyId { get; set; }

        public DateTime JoinDate { get; set; }

        public FamilyMemberRank Rank { get; set; }

        #endregion
    }
}