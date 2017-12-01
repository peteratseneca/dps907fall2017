using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// new...
using System.Security.Cryptography;
using System.Text;

namespace IA.Controllers
{
    // This controller will generate a custom "machine key"
    // When the app that issues a cookie or token is separated 
    // from the app that validates a cookie or token,
    // the apps MUST use the same machineKey

    // Call the Index() method (with the resource URI /GenerateMachineKey)
    // Then copy the response to the <system.web> section of Web.config
    // in each app that shares the security environment

    public class GenerateMachineKeyController : Controller
    {
        // GET: GenerateMachineKey
        public ActionResult Index()
        {
            // Create the decryption key
            var decryptionKey = CreateKey(24);
            // Create the validation key
            var validationKey = CreateKey(64);

            // Prepare the response
            var machineKey = string.Format(@"<machineKey validationKey=""{0}"" decryptionKey=""{1}"" validation=""SHA1""/>", validationKey, decryptionKey);

            return Content(machineKey, "text/plain");
        }

        private string CreateKey(int numBytes)
        {
            // Initialize a crypto service provider
            var rng = new RNGCryptoServiceProvider();
            // Create a container to hold a random value
            byte[] buff = new byte[numBytes];

            // Generate the random value
            rng.GetBytes(buff);
            // Return a hex character version of the random value
            return KeyToHexString(buff);
        }

        private string KeyToHexString(byte[] bytes)
        {
            // Initialize a string container
            var hexString = new StringBuilder(64);

            // Convert each byte of the random value to its hex character version
            for (int i = 0; i < bytes.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[i]));
            }
            return hexString.ToString();
        }

    }
}