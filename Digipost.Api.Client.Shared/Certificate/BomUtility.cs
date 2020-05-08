using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Digipost.Api.Client.Shared.Tests,PublicKey=0024000004800000940000000602000000240000525341310004000001000100f71f491a4cebe0a3d18a61744f92edfca908e4d756aa1140ebceeffb1fc4aa2e7bbe4d672067e2c0a3afd8c4511ef84cc1267ba04d8041e24d96c3d93e268fd69abc712fa81bcbae729f1c0524eef0254705bb2fcf1ffd43a647e9306b93e8dd7afd094a61ca2761fe87c20fdda758ad55d2c5ba6ad6edc9493309a355e51f99")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2,PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]

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
