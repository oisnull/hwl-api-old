using System;

namespace HWL.Tools
{
    public class SmsSend
    {

        private const string SmsSn = "SDK-ZER-010-00008";
        private const string SmsPwd = "B30B53DD7F26DC12BE2BA2DFBE343FF7";


        /// <summary>
        /// App send sms
        /// </summary>
        /// <param name="mobile">mobile number</param>
        /// <param name="code">code</param>
        /// <returns>code</returns>
        public static bool ToUser(string mobile, string smsBody,out string error)
        {
            error = "";
            //SmsWebService smsSendWs = new SmsWebService();

            //var SmsPwds = Encryptor.MD5_Encrypt("SDK-ZER-010-00008" + "2cE9b-86").ToUpper();

            //string x = smsSendWs.mdgxsend(SmsSn,
            //    SmsPwds,
            //    mobileNumber.Trim(),
            //    smsBody, null, null, null, null);

            //return !x.Contains("-");
            return true;
        }

    }
}

