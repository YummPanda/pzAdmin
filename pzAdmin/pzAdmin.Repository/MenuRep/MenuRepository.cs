using pzAdmin.Model;
using pzAdmin.Repository.Base;
using SqlSugar;

namespace pzAdmin.Repository.MenuRep
{
      public class MenuRepository : BaseRepository<Menu>, IMenuRepository
      {
            public MenuRepository(ISqlSugarClient db) : base(db)
            {
            }
      }
}
