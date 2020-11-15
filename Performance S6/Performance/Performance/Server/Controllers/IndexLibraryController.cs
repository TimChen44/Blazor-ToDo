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
    public class IndexLibraryController : ControllerBase
    {
        PerformanceContext Context;
        IMapper Mapper;
        public IndexLibraryController(PerformanceContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        #region 维护指标库

        [HttpPost]
        public ResultDataSet<IndexLibraryDto> Search(IndexLibraryQueryDto req)
        {
            var query = (IQueryable<IndexLibrary>)Context.IndexLibraries;

            query = query.AddWhere(req.Name, x => x.Name.Contains(req.Name))//指标名称
                        .AddWhere(req.Scope, x => x.Scope == req.Scope)//指标范围
                        .AddWhere(req.Type, x => x.Type.Contains(req.Type))//指标类别
                        .AddWhere(req.Definition, x => x.Definition.Contains(req.Definition));//指标定义

            foreach (var sort in req.Sorts)
            {
                if (sort.SortOrder == "descend")
                    query = query.OrderBy(sort.SortField + " DESC");
                else
                    query = query.OrderBy(sort.SortField);

            }

            var result = query.PageForAnt(req).Select(x => Mapper.Map<IndexLibraryDto>(x)).ToList();

            return new(result, query.Count());
        }

        [HttpGet]
        public ResultData<IndexLibraryDto> New()
        {
            return new() { Data = new() { IndexLibraryId = GuidGenerator.NewGuid(), IsEnabled = true } };
        }

        [HttpGet]
        public ResultData<IndexLibraryDto> Get(Guid id)
        {
            var entity = Context.IndexLibraries.FirstOrDefault(x => x.IndexLibraryId == id);
            return new ResultData<IndexLibraryDto>(Mapper.Map<IndexLibraryDto>(entity));
        }

        [HttpPut]
        public ResultData<IndexLibraryDto> Save(IndexLibraryDto model)
        {
            var entity = Context.IndexLibraries.FirstOrDefault(x => x.IndexLibraryId == model.IndexLibraryId);
            if (entity == null)
            {
                entity = new IndexLibrary();
                entity.IndexLibraryId = model.IndexLibraryId;
                Context.Add(entity);
            }
            entity.Name = model.Name;
            entity.Scope = model.Scope;
            entity.Type = model.Type;
            entity.Unit = model.Unit;
            entity.Definition = model.Definition;
            entity.Remark = model.Remark;
            entity.IsEnabled = model.IsEnabled;

            Context.SaveChanges();
            return new(model);
        }


        [HttpDelete]
        public ResultMsg Delete(Guid id)
        {
            if (Context.OrganizationIndices.Any(x => x.IndexLibraryId == id))
            {
                return new ResultMsg(false, "指标已被使用，不能删除，建议使用禁用属性");
            }

            Context.IndexLibraries.Remove(Context.IndexLibraries.Find(id));
            Context.SaveChanges();
            return new(true);
        }

        #endregion

        [HttpGet]
        public ResultDataSet<SelectOptionCore> GetIndexOptions()
        {
            var result = Context.IndexLibraries.Where(x => x.IsEnabled == true).Select(x => new SelectOptionCore(x.IndexLibraryId.ToString(), x.Name)).ToList();
            return new(result);
        }
    }

}
