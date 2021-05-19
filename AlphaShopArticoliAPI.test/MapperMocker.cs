using AlphaShopArticoliAPI.Profiles;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaShopArticoliAPI.test
{
    class MapperMocker
    {
        public static IMapper GetMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ArticoliProfile());
            });
            var mapper = mockMapper.CreateMapper();
            return mapper;
        }
    }
}
