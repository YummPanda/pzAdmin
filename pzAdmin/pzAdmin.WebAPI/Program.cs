using Microsoft.OpenApi.Models;
using pzAdmin.Common.CachExtension;
using pzAdmin.Common.CorsExtension;
using pzAdmin.Common.Extensions;
using pzAdmin.Common.JWTExtension;
using pzAdmin.Repository.Base;
using pzAdmin.Repository.MenuRep;
using pzAdmin.Repository.UserRep;
using pzAdmin.Service.MenuServ;
using pzAdmin.Service.UserServ;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册SqlSugar
builder.Services.AddSqlDb(builder.Configuration.GetConnectionString("DefaultConnection")!);
//注册仓储
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//注册启用内存缓存
builder.Services.AddMemoryCache();
//注册CPRES扩展
builder.Services.AddCorsExtension(builder.Configuration["Cors:urls"]!.Split("|"));
//注册JWT配置
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
//注册JWT验证
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>()!;
builder.Services.AddJwtAuthentication(jwtOptions); //注册JWT验证
builder.Services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();//注册缓存帮助类
//注册用户仓储和服务
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
//注册菜单仓储和服务
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
//注册角色仓储和服务


//注册Swagger增加token授权机制
builder.Services.AddSwaggerGen(option =>
{
      //添加安全定义--配置支持token授权机制
      option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
            Description = "请输入token,格式为 Bearer xxxxxxxx（注意中间必须有空格）",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
      });
      //添加安全要求
      option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                        new OpenApiSecurityScheme
                        {
                            Reference =new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id ="Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
      app.UseSwaggerUI();
}

//使用CORS扩展
app.UseCorsExtension();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
