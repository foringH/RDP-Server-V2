using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RDPCOMAPILib;

namespace RDPNonStatic_Server
{
    class SharePC
    {
        public RDPSession currentSession = null;
        public object _lock = new object();

        private static int countControlClient = 0;
        private static int countViewClient = 0;

        private int flag = 0;

        public void createSession()  //made it non-static
        {
            lock (_lock)
            {

                if (null == currentSession)
                    currentSession = new RDPSession();
            }


        }


        private void connectWithControl()//RDPSession session)
        {
            Console.WriteLine("connecting control");

            currentSession.OnAttendeeConnected += incomingControl;
            currentSession.OnAttendeeDisconnected += outgoingControl;

            try
            {
                currentSession.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("the error is in opening connection: " + e);
                throw e;
            }

            //session.Open();
        }


        private void connectWithView()//RDPSession session)
        {
            Console.WriteLine("connecting View");
 
            currentSession.OnAttendeeConnected += incomingView;
            currentSession.OnAttendeeDisconnected += outgoingView;


            try
            {
                currentSession.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("the error is in opening connection: " + e);
                throw e;

            }

            //session.Open();
        }


        public void disconnect()//RDPSession session)
        {
            try
            {
                
                currentSession.Close();
                currentSession = null;

                flag = 0;

                countControlClient = 0;
                countViewClient = 0;
            }
            catch (Exception Ex)
            {
                // MessageBox.Show("Error Connecting", "Error connecting to remote desktop " + " Error:  " + Ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("error disconnecting" + Ex);
                throw Ex;
            }

        }


        private string connectionString(RDPSession session, String authString, string group, string password, int clientLimit)
        {
            IRDPSRAPIInvitation invitation = session.Invitations.CreateInvitation(authString, group, password, clientLimit);
            return invitation.ConnectionString;
        }


        private static void incomingControl(object Guest)
        {
            countControlClient++;

            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_INTERACTIVE;

            Console.WriteLine("connected with control: " + countControlClient);

        }

        private static void outgoingControl(object Guest)
        {
            countControlClient--;

            Console.WriteLine("connected with control: " + countControlClient);


        }



        private static void incomingView(object Guest)
        {
            countViewClient++;

            IRDPSRAPIAttendee MyGuest = (IRDPSRAPIAttendee)Guest;
            MyGuest.ControlLevel = CTRL_LEVEL.CTRL_LEVEL_VIEW;

            Console.WriteLine("connected with view: " + countViewClient);


        }

        private static void outgoingView(object Guest)
        {
            countViewClient--;

            Console.WriteLine("connected with view: " + countViewClient);


        }

        /// <summary>
        /// throws exception
        /// </summary>
        public void shareControl()
        {
            if (flag == 0)
            {
                createSession();
                connectWithControl();//currentSession);
                flag = 1;
            }
            else
            {
                throw new SessionExistsException();//"Without disconnecting,you can not create another one");
                //throw new Exception("There is already a session.Without disconnecting it,you can not create another one");
            }


        }

        /// <summary>
        /// throws exception
        /// </summary>
        public void shareView()
        {
            if (flag == 0)
            {
                createSession();
                connectWithView();//currentSession);
                flag = 1;
            }
            else
            {
                throw new SessionExistsException();//"There is already a session.Without disconnecting it,you can not create another one");
            }
        }



        public string getInvitationString(int clientLimit)
        {

            String invitation = getUnprotectedInvitationString(clientLimit);
            return invitation;

        }
        public String getUnprotectedInvitationString(int clientLimit)
        {
            Console.WriteLine("the invitation string is being generated: \n");

            String invitationString = connectionString(currentSession, "Test", "Group", "", clientLimit);
            Console.WriteLine("the invitation string: \n" + invitationString);

            return invitationString;

        }

        public String getProtectedInvitationString(int clientLimit, String password)
        {
            String invitationString = connectionString(currentSession, "Test", "Group", password, clientLimit);
            Console.WriteLine("the invitation string: \n" + invitationString);

            return invitationString;

        }

        public int getControlClientNumber()
        {
            return countControlClient;
        }

        public int getViewClientNumber()
        {
            return countViewClient;
        }


    }

    class SessionExistsException : ApplicationException
    {
        public SessionExistsException()
            : base("Without disconnecting,you can not create another one")
        {
            String exceptionMessage1 = "EXCEPTION:You are trying to open more than one connection without closing the previous one";
            String exceptionMessage2 = "Disconnect the previous connection then try again";


        }
        public SessionExistsException(String msg)
            : base(msg)
        {
            String exceptionMessage1 = "EXCEPTION:You are trying to open more than one connection without closing the previous one";
            String exceptionMessage2 = "Disconnect the previous connection then try again";

        }
    }
}
