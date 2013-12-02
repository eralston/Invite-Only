using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InviteOnly.Models
{
    /// <summary>
    /// Interface for an entity framework DB context that provides an invite DB Set
    /// </summary>
    public interface IInviteContext
    {
        System.Data.Entity.DbSet<InviteOnly.Models.Invite> Invites { get; }
    }

    /// <summary>
    /// Interface for any object that has an IInviteContext as a property
    /// Classes using the action filters should implement this interface to provide a common context for the attribute and their controller class
    /// </summary>
    public interface IInviteContextProvider
    {
        IInviteContext InviteContext { get; }
    }
}
