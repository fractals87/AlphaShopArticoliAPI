using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopArticoliAPI.DTO;
using AlphaShopArticoliAPI.Models;
using AutoMapper;

namespace AlphaShopArticoliAPI.Profiles
{
    public class ArticoliProfile : Profile
    {
        public ArticoliProfile()
        {
            CreateMap<Articoli, ArticoliDto>()
                .ForMember
                (
                    dest => dest.Categoria,
                    opt => opt.MapFrom(src => $"{ src.IdFamAss} {src.famAssort.Descrizione}")
                )
                .ForMember
                (
                    dest => dest.CodStat,
                    opt => opt.MapFrom(src => src.CodStat.Trim())
                )
                .ForMember
                (
                    dest => dest.Um,
                    opt => opt.MapFrom(src => src.Um.Trim())
                )
                .ForMember
                (
                    dest => dest.IdStatoArt,
                    opt => opt.MapFrom(src => src.IdStatoArt.Trim())
                );
        }
    }
}
