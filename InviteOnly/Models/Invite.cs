using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace InviteOnly.Models
{
    /// <summary>
    /// This class defines an instance of welcoming a user into the system and interacts with the action filter attributes
    /// </summary>
    public class Invite
    {
        #region Static Methods

        /// <summary>
        /// Generates a unique string for use as a simple invite identifier
        /// </summary>
        /// <returns></returns>
        public static string GenerateUniqueInviteValue()
        {
            // Create a Guid of the 32 naked digits format
            // http://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Creates a new invite instance
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Invite Create(string value = null, int type = 0)
        {
            // Default to using the generated unique key
            value = value ?? GenerateUniqueInviteValue();

            // Create the new invite
            Invite invite = new Invite();
            invite.CreatedDate = DateTime.Now;
            invite.Value = value;
            invite.Type = type;
            invite.Fulfilled = false;

            return invite;
        }

        /// <summary>
        /// Creates a new invite and adds it to the context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Invite Create(IInviteContext context, string value = null, int type = 0)
        {
            Invite invite = Create(value, type);
            
            // Add it to the context
            context.Invites.Add(invite);

            return invite;
        }

        #endregion

        #region Instance Properties (For DB Schema)

        /// <summary>
        /// Primary key
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// An optional type parameter. It is recommended client code use an enumeration
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// A unique value for the invite
        /// </summary>
        [Required]
        public string Value { get; set; }


        public bool Fulfilled { get; set; }

        /// <summary>
        /// The date this invite was created
        /// </summary>
        public DateTime? CreatedDate { get; set; } 

        #endregion
    }
}