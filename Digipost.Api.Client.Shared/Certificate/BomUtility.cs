using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digipost.Api.Client.Shared.Certificate
{
    public class BomUtility ////Todo: Var internal, og bør nok være det videre
    {
        /// <summary>
        ///     Removes the invisible character often encountered when copying thumbprint from Microsoft Management Console.
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <returns>cleansed thumbprint</returns>
        public virtual string RemoveBom(string thumbprint) //Todo: Var internal, og bør nok være det videre
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