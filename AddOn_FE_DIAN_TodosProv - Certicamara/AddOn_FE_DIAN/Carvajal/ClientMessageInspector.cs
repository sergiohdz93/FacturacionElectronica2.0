using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace AddOn_FE_DIAN
{
    public class ClientMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState " /> argument of
        /// the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two
        /// <paramref name="correlationState" /> objects are the same.
        /// </returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            Procesos.requestSend = "";
            ///enter your username and password here.
            Security header = new Security("1", Procesos.username, Procesos.password, getNonce.RandomString(10));

            request.Headers.Add(header);

            //if (request.ToString().Contains("CabinDetail"))
            //{
                //int headerIndexOfAction = request.Headers.FindHeader("Action", "http://schemas.microsoft.com/ws/2005/05/addressing/none");
                //request.Headers.RemoveAt(headerIndexOfAction);
            //}

            Procesos.requestSend = request.ToString();
            return header.Name;
        }


        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Procesos.responseStatus = reply.ToString();
        }
    }
}
