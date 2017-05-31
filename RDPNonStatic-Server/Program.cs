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

            SharePC pc2=new SharePC();

            pc1.shareControl();
            String viewInvitation = pc1.getInvitationString(16); //the max no of client 
            Console.WriteLine("for control:\n" + viewInvitation);

            //pc1.disconnect();

            pc2.shareView();
            String controlInvitation = pc2.getUnprotectedInvitationString(12); //the max no of client
            Console.WriteLine("for view:\n" + controlInvitation);

           
            String a = Console.ReadLine();

        }
             

    }
}
