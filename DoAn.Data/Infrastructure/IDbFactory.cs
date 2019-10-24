using System;

namespace DoAn.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BkresContext Init();
    }
}