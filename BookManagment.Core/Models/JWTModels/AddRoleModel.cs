﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Models.JWTModels
{

    public class AddRoleModel
    {
        public string UserId { get; set; }

        public string Role { get; set; }
    }

}
