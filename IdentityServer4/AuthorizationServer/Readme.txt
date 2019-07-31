Token验证：
===============================================================
如果在api里面验证了token的完整性, 那么我们就会知道token是ok的.
我们这里研究的token是Json Web Token.
token是由authorization server签名发布的.
authorization server就是使用oauth和openid connect等协议, 并且用户可以使用用户名密码等方式来换取token的服务, 这些token让我们拥有了访问一些资源的权限.
private key 是用来签发token用的, 它只存在于authorization server, 永远不要让别人知道private key.
public key 是用来验证token的, authorization server 会让所有人都知道这个public key的.
api或者各种消费者应该一直检验token, 以确保它们没有被篡改. 它们可以向authorization server申请public key, 并使用它来验证token. 也可以把token传递给authorization server, 并让authorization server来进行验证.
Token同时也包含着authorization server的一些信息. 比如是由哪个authorization server发布的token.
Token的信息是使用Base64编码的.

OAuth 和 OpenId Connect
========================================================
他们都是安全协议.
现在使用的都是OAuth 2.0, 注意它与1.0不兼容.
OAuth 2.0是用于authorization的工业标准协议. 它关注的是为web应用, 桌面应用, 移动应用等提供特定的authorization流程并保证开发的简单性.
Authorization的意思是授权, 它表示我们可以授权给某些用户, 以便他们能访问一些数据, 也就是提供访问权.
OAuth是基于token的协议.
有些人可能对Authorization和Authentication分不清, 上面讲了authorization, 而authentication则是证明我是谁, 例如使用用户名和密码进行登录就是authentication.
OAuth只负责Authorization. 那么谁来负责Authentication呢?
那就是OpenId Connect, OpenId Connect是对OAuth的一种补充, 因为它能进行Authentication.
OpenId Connect 是位于OAuth 2.0上的一个简单的验证层, 它允许客户端使用authorization server的authentication操作来验证终端用户的身份, 同时也可以或缺终端客户的一些基本信息.
可以有多种方式来实现OAuth和OpenId Connect这套协议.
你可以自己去实现. 
我要使用的是Identity Server 4.
其实也可以使用一些Saas/Paas服务, 例如Amazon Cognito, Auth0(这个用过, 有免费版), Stormpath.

发布IIS：应用程序池-加载用户配置文件=true

OpenSSL证书：
===============================================================
1.https://slproweb.com/products/Win32OpenSSL.html
2.openssl req -newkey rsa:2048 -nodes -keyout socialnetwork.key -x509 -days 365 -out socialnetwork.cer命令行生成key和证书(socialnetwork.key,socialnetwork.cer)
3.openssl pkcs12 -export -in socialnetwork.cer -inkey socialnetwork.key -out socialnetwork.pfx输入密码和确认密码后生成的socialnetwork.pfx就是我们要的证书文件

添加像样的UI:
===============================================================
Identity Server 4 提供了一套QuickStart UI : https://github.com/IdentityServer/IdentityServer4.Quickstart.UI/tree/release
在项目根目录打开Powershell(可以在项目根目录, 按住shift, 点击右键的Powershell),在命令行输入:
iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/release/get.ps1'))
然后UI就加载到项目中了

使用EF持久化数据:
===============================================================
1.然后使用命令行进入Auth Server项目的目录, 试一下dotnet ef命令:
很不幸, 没找到dotnet ef命令. 这里需要手动修改AuthServer的项目文件, 右键点击项目, 点击Edit AuthServer.csproj.
这部分操作的官方文档在这: https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet
我们需要添加这部分代码:
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
  </ItemGroup>
然后回到命令行, 再执行 dotnet ef:这次好用了. 
2.接下来就是add migrations了.
幸运的是, 之前装的库里面有封装好的model, 它们可以自动创建migration文件.
这里一共有两个命令(migrations), 一个是为了IdentityServer的配置, 另一个是为了持久化授权.
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
运行发现了问题, 这是因为我们还没有配置AuthServer来使用数据库.添加appSettings.json, 并指定连接字符串:
"ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AuthServer;Trusted_Connection=True;MultipleActiveResultSets=true"
},

Package Manager Console
Add-Migration MaintainDbInitial -c MaintainDbContext -o Data/Migrations/MaintainDb