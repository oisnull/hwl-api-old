using HWL.IMCore.Protocol;

namespace HWL.IMClient.Core
{
    public interface IClientMessageListenExecutor
    {
        void execute(ImMessageContext messageContext);

        bool executedAndClose();
    }
}
