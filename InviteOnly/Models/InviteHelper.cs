using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InviteOnly.Models
{
    /// <summary>
    /// A static class for 
    /// </summary>
    public static class InviteHelper
    {
        /// <summary>
        /// Finds the invite with the given value using the given Invite context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Invite FindInvite(this IQueryable<Invite> queryable, string value)
        {
            return queryable.Where(i => i.Value == value).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Invite FindInvite(this IQueryable<Invite> queryable, string value, int type)
        {
            return queryable.Where(i => i.Value == value && i.Type == type).Take(1).SingleOrDefault();
        }
    }
}
