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
    public class AchievementController : ControllerBase
    {
        PerformanceContext Context;
        IMapper Mapper;
        public AchievementController(PerformanceContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        [HttpGet]
        public ResultDataSet<AchievementEditDto> GetEdit(int year, Guid orgId)
        {
            if (Context.Achievements.Any(x => x.Year == year && x.OrganizationId == orgId))
            {
                var result = Context.Achievements.Where(x => x.Year == year && x.OrganizationId == orgId)
                     .Select(x => new AchievementEditDto()
                     {
                         AchievementId = x.AchievementId,
                         IndexLibraryId = x.IndexLibraryId,
                         IndexName = x.IndexLibrary.Name,
                         OrganizationId = x.OrganizationId,
                         Year = x.Year,
                         TargetValue = x.TargetValue,
                         ActualValue = x.ActualValue,
                         GuaranteedValue = x.GuaranteedValue,
                         ChallengeValue = x.ChallengeValue,
                     }).ToList();
                return new(result);
                
            }
            else
            {
                var result = Context.OrganizationIndices.Where(x => x.OrganizationId == orgId && x.IndexLibrary.IsEnabled == true)
                    .Select(x => new AchievementEditDto()
                    {
                        AchievementId = Guid.NewGuid(),
                        IndexLibraryId = x.IndexLibraryId,
                        IndexName = x.IndexLibrary.Name,
                        OrganizationId = x.OrganizationId,
                        Year = year,
                        TargetValue = 0,
                        GuaranteedValue = 0,
                        ChallengeValue = 0,
                    }).ToList();
                return new(result);
            }
        }

        [HttpPost]
        public ResultMsg SaveEdit(List<AchievementEditDto> dtos)
        {
            foreach (var item in dtos)
            {
                var entity = Context.Achievements.FirstOrDefault(x => x.AchievementId == item.AchievementId);
                if (entity == null)
                {
                    entity = new Achievement();
                    entity.AchievementId = item.AchievementId;
                    Context.Add(entity);
                }
                entity.IndexLibraryId = item.IndexLibraryId;
                entity.OrganizationId = item.OrganizationId;
                entity.Year = item.Year;
                entity.TargetValue = item.TargetValue;
                entity.ActualValue = item.ActualValue;
                entity.GuaranteedValue = item.GuaranteedValue;
                entity.ChallengeValue = item.ChallengeValue;
            }
            Context.SaveChanges();
            return new(true);
        }

    }
}
