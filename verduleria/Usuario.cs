using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verduleria
{
    public class Usuario
    {
            public int IdUser { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string PasswordConfirma { get; set; }
            public string Nombre { get; set; }
            public int IdTipoUser { get; set; }

            public Usuario() { }

            public Usuario(string usuario, string pass)
            {
                this.User = usuario;
                this.Password = pass;
            }
        }
    }
