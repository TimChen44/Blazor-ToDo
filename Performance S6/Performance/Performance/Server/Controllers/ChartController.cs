using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Performance.Entity;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Performance.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChartController : ControllerBase
    {
        PerformanceContext Context;
        IMapper Mapper;
        public ChartController(PerformanceContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        [HttpGet]
        public ResultDataSet<IndexLibraryChartDto> GetIndexLibraryColumn(Guid indexId)
        {
            var result = Context.Achievements.Where(x => x.IndexLibraryId == indexId).Select(x => new IndexLibraryChartDto()
            {
                Year = x.Year,
                OrgId=x.OrganizationId,
                OrgName = x.Organization.Name,
                ActualValue = x.ActualValue ?? 0,
            }).OrderBy(x=>x.Year).ToList();
            return new(result);
        }

    }
}
