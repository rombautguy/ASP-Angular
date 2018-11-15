using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EAccess.Models
{
    // Models returned by AccountController actions.

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
