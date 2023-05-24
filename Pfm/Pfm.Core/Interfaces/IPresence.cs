using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pfm.Core.Interfaces
{
    public interface IPresence
    {
        void Checkin(byte[] face, Action<string> successAction, Action<string> failAction);
    }
}