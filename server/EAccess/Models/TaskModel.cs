using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EAccess.Models
{
    // Models returned by AccountController actions.

    public class UserTask
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int priority { get; set; }
        public int state { get; set; }
        public int estimate { get; set; }
    }
}
