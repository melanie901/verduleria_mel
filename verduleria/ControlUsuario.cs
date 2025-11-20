using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace verduleria
{
    public class ControlUsuario
    {
        public ControlUsuario(){

        }
        public bool ControlRegistroUsuarios(Usuario user)
        {
            ModeloUsuarios modelo = new ModeloUsuarios();
       
            if ((string.IsNullOrEmpty(user.User)) ||
               (string.IsNullOrEmpty(user.Password)) ||
               (string.IsNullOrEmpty(user.PasswordConfirma)) ||
               (user.IdTipoUser < 0) ||
               (string.IsNullOrEmpty(user.Nombre)))
                return false;
            else
            {
                if (user.Password == user.PasswordConfirma)
                {
                    if (modelo.existeUsuario(user))
                        return false;
                    else
                    {

                        user.Password = generarSHA1(user.Password);

                        if (modelo.registrarUsuario(user))
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
        }

        private string generarSHA1(string cadena)
        {
            UTF8Encoding enc = new UTF8Encoding();
            byte[] data = enc.GetBytes(cadena);
            byte[] result;
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] < 16)
                {
                    sb.Append("0");
                }
                sb.Append(result[i].ToString("x"));
            }
            return sb.ToString();
        }
    }
}
