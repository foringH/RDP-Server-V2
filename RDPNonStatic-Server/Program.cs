using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDPNonStatic_Server
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("hello");

            SharePC pc1 = new SharePC();

            SharePC pc=new SharePC();

            pc1.shareControl();
            String viewInvitation = pc1.getInvitationString(5); //the max no of client 
            Console.WriteLine("for control:\n" + viewInvitation);

            pc1.disconnect();

            pc.shareView();
            String controlInvitation = pc.getUnprotectedInvitationString((int)2);
            Console.WriteLine("for control:\n" + controlInvitation);

            String a = Console.ReadLine();

        }
             

    }
}
