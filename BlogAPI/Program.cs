using BlogAPI.Biz.Services.Authentication;
using BlogAPI.Biz.Services.Comments;
using BlogAPI.Biz.Services.People;
using BlogAPI.Biz.Services.PostRejectionComments;
using BlogAPI.Biz.Services.Posts;
using BlogAPI.Controllers;
using BlogAPI.Data;
using BlogAPI.Data.Models;
using BlogAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogAPI", Version = "V1", });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {new OpenApiSecurityScheme{Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }}, new List<string>()
        }
    });
});

//JWT configuration
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JWTSettings>();
builder.Services.AddSingleton(jwtSettings);

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = true,
    ValidateLifetime = true
};

builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(b =>
{
    b.SaveToken = true;
    b.TokenValidationParameters = tokenValidationParameters;
});


builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("Public", policy => policy.RequireClaim("personType", "Public"));
    x.AddPolicy("Editor", policy => policy.RequireClaim("personType", "Editor"));
    x.AddPolicy("Writer", policy => policy.RequireClaim("personType", "Writer"));
    x.AddPolicy("EditorWriter",
        policy => policy.RequireAssertion(context =>
            context.User.HasClaim(c =>
                (c.Type == "personType" && c.Value == "Editor")
                || (c.Type == "personType" && c.Value == "Writer"))));
});

//Database Configuration
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
builder.Services.AddDbContext<BlogAPIDbContext>(o => o.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<BlogAPIDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddHttpContextAccessor();
// Identity builder.Services
builder.Services.TryAddScoped<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
builder.Services.TryAddScoped<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
builder.Services.TryAddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
builder.Services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
// No interface for the error describer so we can add errors without rev'ing the interface
builder.Services.TryAddScoped<IdentityErrorDescriber>();
builder.Services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
builder.Services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<ApplicationUser>>();
builder.Services.TryAddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
builder.Services.TryAddScoped<UserManager<ApplicationUser>>();
builder.Services.TryAddScoped<SignInManager<ApplicationUser>>();


builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(1));

//Register services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<ICommentsService, CommentsService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IPostRejectionCommentService, PostRejectionCommentService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogAPI V1"));


app.AddErrorHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
