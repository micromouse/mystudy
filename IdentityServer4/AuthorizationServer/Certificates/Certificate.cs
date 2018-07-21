using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DXQ.Study.IdentityServer4.AuthorizationServer.Certificates {
    /// <summary>
    /// 凭据
    /// </summary>
    public class Certificate {
        public static X509Certificate2 Get() {
            var assembly = typeof(Certificate).GetTypeInfo().Assembly;

            /***********************************************************************************************
             *  Please note that here we are using a local certificate only for testing purposes. In a 
             *  real environment the certificate should be created and stored in a secure way, which is out
             *  of the scope of this project.
             **********************************************************************************************/
            using (var stream = assembly.GetManifestResourceStream("DXQ.Study.IdentityServer4.AuthorizationServer.Certificates.socialnetwork.pfx")) {
                return new X509Certificate2(ReadStream(stream), "1");
            }

        }

        /// <summary>
        /// 读取流到字节数组
        /// </summary>
        /// <param name="stream">输入流</param>
        /// <returns>输入流字节数组</returns>
        public static byte[] ReadStream(Stream stream) {
            byte[] buffers = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream()) {
                int read = 0;
                while ((read = stream.Read(buffers, 0, buffers.Length)) > 0) {
                    ms.Write(buffers, 0, buffers.Length);
                }
                return ms.ToArray();
            }
        }
    }
}
