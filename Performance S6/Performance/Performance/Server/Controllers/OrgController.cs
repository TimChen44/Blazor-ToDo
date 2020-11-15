using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Performance.Entity;
using Performance.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Performance.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrgController : ControllerBase
    {
        PerformanceContext Context;
        IMapper Mapper;
        public OrgController(PerformanceContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        #region 维护组织树

        [HttpGet]
        public ResultDataSet<OrgTreeDto> GetOrgChilds(Guid? parentId)
        {
            var result = Context.Organizations.Where(x => x.ParentId == parentId).Select(x => Mapper.Map<OrgTreeDto>(x)).ToList();
            return new(result, result.Count);
        }

        [HttpGet]
        public ResultData<OrgDto> New(Guid? parentId)
        {
            return new() { Data = new() { OrganizationId = Guid.NewGuid(), ParentId = parentId } };
        }

        [HttpGet]
        public ResultData<OrgDto> Get(Guid id)
        {
            var entity = Context.Organizations.FirstOrDefault(x => x.OrganizationId == id);
            return new(Mapper.Map<OrgDto>(entity));
        }

        [HttpPut]
        public ResultData<OrgDto> Save(OrgDto model)
        {
            var entity = Context.Organizations.FirstOrDefault(x => x.OrganizationId == model.OrganizationId);
            if (entity == null)
            {
                entity = new Organization();
                entity.OrganizationId = model.OrganizationId;
                entity.ParentId = model.ParentId;
                Context.Add(entity);
            }
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.Description = model.Description;

            Context.SaveChanges();
            return new(model);
        }

        [HttpDelete]
        public ResultMsg Delete(Guid id)
        {
            if (Context.Organizations.Any(x => x.ParentId == id))
            {
                return new ResultMsg(false, "存在子节点，请先删除子节点");
            }

            Context.Organizations.Remove(Context.Organizations.Find(id));
            Context.SaveChanges();
            return new(true);
        }

        #endregion

        #region 指标绑定


        [HttpPost]
        public ResultData<IndexLibraryDto> LinkOrg(OrgIndexLinkDto linkDto)
        {
            if (Context.OrganizationIndices.Any(x => x.OrganizationId == linkDto.OrganizationId && x.IndexLibraryId == linkDto.IndexLibraryId))
            {
                return new ResultData<IndexLibraryDto>(false, "已添加相同指标，不能重复添加！");
            }

            var entity = new OrganizationIndex()
            {
                OrganizationIndexId = Guid.NewGuid(),
                OrganizationId = linkDto.OrganizationId,
                IndexLibraryId = linkDto.IndexLibraryId,
            };
            Context.Add(entity);
            Context.SaveChanges();

            var index = Context.IndexLibraries.FirstOrDefault(x => x.IndexLibraryId == linkDto.IndexLibraryId);
            return new(Mapper.Map<IndexLibraryDto>(index));
        }

        [HttpDelete]
        public ResultMsg DelLinkOrg(Guid orgId, Guid indexId)
        {
            Context.OrganizationIndices.Remove(Context.OrganizationIndices.First(x => x.OrganizationId == orgId && x.IndexLibraryId == indexId));
            Context.SaveChanges();
            return new(true);
        }

        [HttpGet]
        public ResultDataSet<IndexLibraryDto> GetIndexs(Guid orgId)
        {
            var result = Context.OrganizationIndices.Where(x => x.OrganizationId == orgId)
                .Select(x => Mapper.Map<IndexLibraryDto>(x.IndexLibrary)).ToList();
            return new(result, result.Count);
        }



        #endregion

        #region 

        [HttpGet]
        public ResultDataSet<SelectOptionCore> GetOrgOptions()
        {
            var result = Context.Organizations.Select(x => new SelectOptionCore(x.OrganizationId.ToString(), x.Name)).ToList();
            return new(result, result.Count);
        }

        #endregion
    }

}
