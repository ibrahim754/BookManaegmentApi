using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagment.Core.Models.JWTModels
{
    public class TokenRequestModel
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
