using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using InviteOnly;

namespace Invite_Only.Models
{
    public class ExampleContext : DbContext, IInviteContext
    {
        public ExampleContext() : base("name=ExampleContext") { }

        // This property is required by IInviteContext
        public System.Data.Entity.DbSet<InviteOnly.Invite> Invites { get; set; }
    }
}
