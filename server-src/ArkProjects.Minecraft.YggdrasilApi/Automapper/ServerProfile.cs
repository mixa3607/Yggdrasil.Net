using System.Security.Cryptography.X509Certificates;
using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
using AutoMapper;

namespace ArkProjects.Minecraft.YggdrasilApi.Automapper;

public class ServerProfile : Profile
{
    public ServerProfile()
    {
        CreateMap<ServerEntity, ServerInfo>(MemberList.Destination)
            .ForMember(x => x.Cert, o => o.MapFrom(s => new X509Certificate2(s.PfxCert)));
    }
}