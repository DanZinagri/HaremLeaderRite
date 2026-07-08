using System.Collections.Generic;
using RimWorld;
using Verse;

namespace HaremLeaderRite
{
	[DefOf]
	public static class HaremRiteDefOf
	{
		// Defined in the required base mod (DanZinagri.HaremLeaderMeme).
		public static MemeDef LeadersHarem;

		static HaremRiteDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(HaremRiteDefOf));
		}
	}

	// Extends VE Memes' adult filter: participants must also be a living spouse of a
	// harem leader (or the harem leader themselves). Only evaluated when the ritual
	// UI/lord assembles participants - never ticked.
	public class RitualSpectatorFilter_HaremSpouse : VanillaMemesExpanded.RitualSpectatorFilter_Adult
	{
		public override bool Allowed(Pawn p)
		{
			if (!base.Allowed(p))
			{
				return false;
			}
			if (IsHaremLeader(p))
			{
				return true;
			}
			List<Pawn> spouses = p.GetSpouses(includeDead: false);
			for (int i = 0; i < spouses.Count; i++)
			{
				if (IsHaremLeader(spouses[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsHaremLeader(Pawn pawn)
		{
			if (pawn == null || pawn.Dead)
			{
				return false;
			}
			Ideo ideo = pawn.Ideo;
			if (ideo == null || !ideo.HasMeme(HaremRiteDefOf.LeadersHarem))
			{
				return false;
			}
			Precept_Role role = ideo.GetRole(pawn);
			return role != null && role.def.leaderRole;
		}
	}
}
