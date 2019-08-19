using HWL.IMCore.Protocol;

namespace HWL.IMClient.Core
{
    public interface IClientMessageSendExecutor
    {
        ImMessageType getMessageType();

        ImMessageContext getMessageContext();

        void sendResultCallback(bool isSuccess);

        bool isSendFailedAndClose();
    }
}
