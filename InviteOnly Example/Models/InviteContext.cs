using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using InviteOnly;

namespace Invite_Only.Models
{
    public class InviteContext : DbContext, IInviteContext
    {    
        public InviteContext() : base("name=InviteContext") { }

        public System.Data.Entity.DbSet<InviteOnly.Invite> Invites { get; set; }
    }
}
