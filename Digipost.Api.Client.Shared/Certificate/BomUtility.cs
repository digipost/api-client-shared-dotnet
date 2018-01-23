using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Digipost.Api.Client.Shared.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Digipost.Api.Client.Shared.Certificate
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    internal class BomUtility
    {
        /// <summary>
        ///     Removes the invisible character often encountered when copying thumbprint from Microsoft Management Console.
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <returns>cleansed thumbprint</returns>
        internal virtual string RemoveBom(string thumbprint)
        {
            var thumbprintBytes = Encoding.UTF8.GetBytes(thumbprint);
            var arrayOfKnownByteOrderMarks = new List<byte[]> {new byte[] {226, 128, 142}};

            foreach (var byteOrderMark in arrayOfKnownByteOrderMarks)
            {
                var potentialBom = thumbprintBytes.Take(byteOrderMark.Length).ToArray();
                if (potentialBom.SequenceEqual(byteOrderMark))
                {
                    return Encoding.UTF8.GetString(thumbprintBytes.Skip(byteOrderMark.Length).ToArray());
                }
            }

            return thumbprint;
        }
    }
}