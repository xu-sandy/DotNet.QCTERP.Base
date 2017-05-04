using System.Collections.Generic;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Data;

namespace Qct.IRepository
{
    public interface ISysDepartmentRepository: IEFRepository<SysDepartments>
    {
        List<SysDepartmentsModel> GetExtList();
        List<SysDepartmentsModel> GetExtList(int id);
        SysDepartments GetListByDepId(int depId);
        List<SysDepartments> GetListByPDepId(int pDepId);
        List<SysDepartments> GetListByType(int type);
        SysDepartmentsModel GetModel(int id, int pdepid);
        OperateResult SaveDep(SysDepartments model);
        void ChangeStatus(int id);
    }
}