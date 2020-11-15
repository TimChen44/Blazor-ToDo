using AutoMapper;
using Performance.Entity;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Performance.Server
{
    public class AutomapperConfig: Profile
    {
        public AutomapperConfig()
        {
            CreateMap<IndexLibrary, IndexLibraryDto>();
            CreateMap<Organization, OrgDto>();
            CreateMap<Organization, OrgTreeDto>();
            CreateMap<OrgDto, OrgTreeDto>();
        }
    }
}
