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

//ע��SqlSugar
builder.Services.AddSqlDb(builder.Configuration.GetConnectionString("DefaultConnection")!);
//ע��ִ�
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//ע�������ڴ滺��
builder.Services.AddMemoryCache();
//ע��CPRES��չ
builder.Services.AddCorsExtension(builder.Configuration["Cors:urls"]!.Split("|"));
//ע��JWT����
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
//ע��JWT��֤
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>()!;
builder.Services.AddJwtAuthentication(jwtOptions); //ע��JWT��֤
builder.Services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();//ע�Ỻ�������
//ע���û��ִ��ͷ���
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
//ע��˵��ִ��ͷ���
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();
//ע���ɫ�ִ��ͷ���


//ע��Swagger����token��Ȩ����
builder.Services.AddSwaggerGen(option =>
{
      //��Ӱ�ȫ����--����֧��token��Ȩ����
      option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
            Description = "������token,��ʽΪ Bearer xxxxxxxx��ע���м�����пո�",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
      });
      //��Ӱ�ȫҪ��
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

//ʹ��CORS��չ
app.UseCorsExtension();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
