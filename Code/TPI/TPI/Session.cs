using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPI
{
    internal class Session
    {
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string Email { get; set; }
        public static string Role { get; set; }
        public static int UserId { get; set; }

        public static void Clear()
        {
            FirstName = null;
            LastName = null;
            Email = null;
            Role = null;
            UserId = 0;
        }
    }
}
