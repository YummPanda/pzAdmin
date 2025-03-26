using pzAdmin.Model;
using pzAdmin.Repository.Base;
using SqlSugar;

namespace pzAdmin.Repository.MenuRep
{
      public class MenuRepository : BaseRepository<Menu>, IMenuRepository
      {
            private readonly ISqlSugarClient db;
            public MenuRepository(ISqlSugarClient db) : base(db)
            {
                  this.db = db;
            }
            public async Task<List<Menu>> GetOwnMenuByRole(int roleId)
            {
                  var role = await db.Queryable<Role>()
                                     .Includes(r => r.Menus)
                                     .FirstAsync(r => r.Id == roleId);
                  return role?.Menus ?? new List<Menu>();
            }
      }
}
