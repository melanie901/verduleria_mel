using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace verduleria
{
    internal class ControlLogin
    {
            public Usuario usuarioValido(Usuario u)
            {
                ModeloLogin m = new ModeloLogin();
                if (string.IsNullOrEmpty(u.User) || string.IsNullOrEmpty(u.Password))
                    return null;

                Usuario usuarioEncontrado = m.buscarUsuario(u);
                if (usuarioEncontrado != null)
                {
                    if (usuarioEncontrado.Password == generarSHA1(u.Password))
                        return usuarioEncontrado;
                }
                return null;
            }

            private string generarSHA1(string cadena)
            {
                UTF8Encoding enc = new UTF8Encoding();
                byte[] data = enc.GetBytes(cadena);
                byte[] result;
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                result = sha.ComputeHash(data);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }


