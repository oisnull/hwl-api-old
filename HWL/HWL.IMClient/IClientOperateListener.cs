using System;
using System.Collections.Generic;
using System.Text;

namespace HWL.IMClient
{
    public interface IClientOperateListener<TResponse>
    {
        void sessionidInvalid();
        void success(TResponse response);
        void failure(uint code, string message);
    }
}
