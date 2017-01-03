using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TShockAPI;

namespace Koishi.BanBuff
{
    public class BuffBan : IEquatable<BuffBan>
    {
        public List<string> AllowedGroups
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                object[] item = new object[] { Koishi.BanBuff.Utils.BuffID[this.ID], "(", this.ID, ")" };
                return string.Concat(item);
            }
        }

        public BuffBan(int id) : this()
        {
            this.ID = id;
            this.AllowedGroups = new List<string>();
        }

        public BuffBan()
        {
            this.AllowedGroups = new List<string>();
        }

        public bool Equals(BuffBan other)
        {
            return this.ID == other.ID;
        }

        public bool HasPermissionToUseBuff(TSPlayer ply)
        {
            if (ply == null)
            {
                return false;
            }
            if (ply.HasPermission("komeiji.ignorebuffban"))
            {
                return true;
            }
            Group group = ply.Group;
            List<Group> groups = new List<Group>();
            while (group != null)
            {
                if (this.AllowedGroups.Contains(group.Name))
                {
                    return true;
                }
                if (groups.Contains(group))
                {
                    object[] name = new object[] { group.Name };
                    throw new InvalidOperationException(StringExt.SFormat("Infinite group parenting ({0})", name));
                }
                groups.Add(group);
                group = group.Parent;
            }
            return false;
        }

        public bool RemoveGroup(string groupName)
        {
            return this.AllowedGroups.Remove(groupName);
        }

        public void SetAllowedGroups(string groups)
        {
            if (!string.IsNullOrEmpty(groups))
            {
                char[] chrArray = new char[] { ',' };
                List<string> list = groups.Split(chrArray).ToList<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = list[i].Trim();
                }
                this.AllowedGroups = list;
            }
        }

        public override string ToString()
        {
            return string.Concat(this.Name, (this.AllowedGroups.Count > 0 ? string.Concat(" (", string.Join(",", this.AllowedGroups), ")") : ""));
        }
    }
}