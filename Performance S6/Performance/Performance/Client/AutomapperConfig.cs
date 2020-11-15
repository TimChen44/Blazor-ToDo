using AutoMapper;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Performance.Client
{
    public class AutomapperConfig: Profile
    {
        public AutomapperConfig()
        {
            CreateMap<OrgDto, OrgTreeDto>();
        }
    }
}
