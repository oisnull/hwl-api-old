using HWL.MQGroupDistribute.message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.MQGroupDistribute
{
    class Program
    {
        static void Main(string[] args)
        {
            double currTmr = 0;

            UserSource us = new UserSource();
            MessageSource ms = new MessageSource();
            int i = 1;
            ms.Listener((msgModel) =>
            {
                if (msgModel != null)
                {
                    var userQueueSymbols = us.GetUserQueueSymbolList(msgModel.GroupId);

                    DateTime beforDT = System.DateTime.Now;
                    ms.Distribute(userQueueSymbols);
                    DateTime afterDT = System.DateTime.Now;
                    TimeSpan ts = afterDT.Subtract(beforDT);
                    currTmr += ts.TotalMilliseconds;
                    Console.WriteLine(i + " current " + ts.TotalMilliseconds + " ms,total " + currTmr + " ms");
                }
                i++;
            });

            Console.ReadLine();
        }
    }
}
