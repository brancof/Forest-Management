using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoFlorestas.WebSite.Models
{
    public class Estado
    {
        private int userType;
        private String username;

        public Estado()
        {
            this.userType = 0;
            this.username = "";
        }

        public Estado(int userType, String username)
        {
            this.userType = userType;
            this.username = username;
        }


        public Estado(Estado E)
        {
            this.userType = E.getUserType();
            this.username = E.getUsername();
        }


        public int getUserType() { return this.userType; }
        public String getUsername() { return this.username; }
        public void setUsername(String username) { this.username = username; }
        public void setUserType(int userType) { this.userType = userType; }

    }
}
