using pzAdmin.Model;
using pzAdmin.Repository.Base;
using SqlSugar;

namespace pzAdmin.Repository.UserRep
{
      public class UserRepository : BaseRepository<User>, IUserRepository
      {
            public UserRepository(ISqlSugarClient db) : base(db)
            {
            }
      }
}
