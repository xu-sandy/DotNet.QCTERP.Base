using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Infrastructure.Data.Extensions;
using System.Collections.Specialized;
using Qct.Infrastructure.Extensions;

namespace Qct.Repository.Systems
{
    public class ApiLibraryRepository : BaseEFRepository<ApiLibrary>, IApiLibraryRepository
    {
        readonly ISysDictionaryRepository _sysDictionaryRepository = null;
        public ApiLibraryRepository(ISysDictionaryRepository sysDictionaryRepository)
        {
            _sysDictionaryRepository = sysDictionaryRepository;
        }
        public PageInformaction FindPageList(NameValueCollection nvl)
        {
            var queryDict= _sysDictionaryRepository.GetReadOnlyEntities();

            var query = from x in GetReadOnlyEntities()
                        select new
                        {
                            ApiTypeTitle = (from y in queryDict
                                            where y.DicPSN == 10 && y.DicSN == x.ApiType && y.CompanyId == x.CompanyId
                                            select y.Title).FirstOrDefault(),
                            x.Id,
                            x.ApiType,
                            x.ApiCode,
                            x.Title,
                            x.ApiUrl,
                            x.State,
                            x.ApiOrder
                        };
            var apiType = nvl["apiType"].ToType<short?>();
            var state = nvl["state"].ToType<short?>();
            var keyword = nvl["keyword"].ToTrim();
            if (apiType.HasValue)
                query = query.Where(o => o.ApiType == apiType.Value);
            if (state.HasValue)
                query = query.Where(o => o.State == state.Value);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                int code = 0;
                int.TryParse(keyword, out code);
                query = query.Where(o => o.Title.Contains(keyword) || o.ApiCode == code);
            }
            return query.OrderBy(o => o.ApiOrder).GetPageWithInformaction();
        }

        public List<ApiLibrary> GetList(bool all = false)
        {
            var query = GetReadOnlyEntities();
            if (!all) query = query.Where(o => o.State == 1);
            return query.ToList();
        }

        public void MoveItem(int mode, int id)
        {
            var obj = Get(id);
            switch (mode)
            {
                case 2://下移
                    var next = GetEntities().Where(o => o.ApiOrder > obj.ApiOrder).OrderBy(o => o.ApiOrder).FirstOrDefault();
                    if (next != null)
                    {
                        var sort = obj.ApiOrder;
                        obj.ApiOrder = next.ApiOrder;
                        next.ApiOrder = sort;
                        SaveChanges();
                    }
                    break;
                default:
                    var prev = GetEntities().Where(o => o.ApiOrder < obj.ApiOrder).OrderByDescending(o => o.ApiOrder).FirstOrDefault();
                    if (prev != null)
                    {
                        var sort = obj.ApiOrder;
                        obj.ApiOrder = prev.ApiOrder;
                        prev.ApiOrder = sort;
                        SaveChanges();
                    }
                    break;
            }
        }
        public override dynamic AddOrUpdate(params ApiLibrary[] objs)
        {
            foreach (var obj in objs)
            {
                if (obj.Id == 0)
                {
                    obj.ApiCode = GetMaxValInt(o => o.ApiCode);
                    obj.ApiOrder = GetMaxValInt(o => o.ApiOrder);
                    Create(obj);
                }
                else
                {
                    var source = Get(obj.Id);
                    obj.ToCopyProperty(source, true, "ApiCode", "CompanyId", "ApiPwd");
                    if (!obj.ApiPwd.IsNullOrEmpty()) source.ApiPwd = obj.ApiPwd;
                }
            }
            SaveChanges();
            //todo:clearcache
            //var stores = string.Join(",", WarehouseService.GetList().Select(o => o.StoreId));
            //Pharos.Infrastructure.Data.Redis.RedisManager.Publish("SyncDatabase", new Pharos.ObjectModels.DTOs.DatabaseChanged() { CompanyId = obj.CompanyId, StoreId = //stores, Target = "ApiLibrary" });
            return Qct.Objects.ValueObjects.OperateResult.Success();
        }

        public void SetState(string ids, short state)
        {
            var idlist = ids.Split(',').Select(o => int.Parse(o)).ToList();
            var list = GetEntities().Where(o => idlist.Contains(o.Id)).ToList();
            var titles = list.Select(o => o.Title).ToList();
            var resours = state == 1 ? GetEntities().Where(o => titles.Contains(o.Title) && !idlist.Contains(o.Id) && o.State == 1).ToList() : new List<ApiLibrary>();
            list.Each(o =>
            {
                o.State = state;
                if (state == 1)
                {
                    var res = resours.FirstOrDefault(i => i.Title == o.Title);
                    if (res != null) res.State = 0;//存在同名则设为禁用
                }
            });
            SaveChanges();
        }
    }
}
