using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InkassoServiceTest
{
    public class SoapMessageInspector : IClientMessageInspector
    {
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Clone the message to avoid consuming it
            MessageBuffer buffer = request.CreateBufferedCopy(int.MaxValue);
            Message copy = buffer.CreateMessage();
            request = buffer.CreateMessage(); // restore original

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            copy.WriteMessage(writer);
            writer.Flush();

            string soapXml = sw.ToString();
            Console.WriteLine("SOAP Request:\n" + soapXml);

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Clone the message to avoid consuming it
            MessageBuffer buffer = reply.CreateBufferedCopy(int.MaxValue);
            Message copy = buffer.CreateMessage();
            reply = buffer.CreateMessage(); // restore original

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            copy.WriteMessage(writer);
            writer.Flush();

            string soapXml = sw.ToString();
            Console.WriteLine("SOAP Response:\n" + soapXml);
        }
    }
}
