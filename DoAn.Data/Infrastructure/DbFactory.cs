namespace DoAn.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private BkresContext dbContext;

        public BkresContext Init()
        {
            return dbContext ?? (dbContext = new BkresContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}