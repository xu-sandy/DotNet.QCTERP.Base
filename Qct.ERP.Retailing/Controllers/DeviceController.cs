using System.Linq;
using System.Web.Mvc;
using Qct.Infrastructure.Web.Extensions;
using Qct.IRepository;
using System;
using Qct.ISevices.Systems;
using Qct.Objects.ValueObjects;
using Qct.Objects.ValueObjects.Systems;

namespace Qct.ERP.Retailing.Controllers
{
    public class DeviceController : Controller
    {
        IDeviceRepository deviceRepository = null;
        IWarehouseRepository warehouseRespository = null;
        IDeviceService deviceService = null;
        ISysWebSettingRepository webSettingRepository = null;
        IWarehouseRepository warehouseRepository = null;

        public DeviceController(IDeviceRepository _deviceRepository, IWarehouseRepository _warehouseRespository, IDeviceService _deviceService, ISysWebSettingRepository _webSettingRepository, IWarehouseRepository _warehouseRepository)
        {
            deviceRepository = _deviceRepository;
            warehouseRespository = _warehouseRespository;
            deviceService = _deviceService;
            webSettingRepository = _webSettingRepository;
            warehouseRepository = _warehouseRepository;
        }
        //GET: Device
        public ActionResult Index()
        {
            ViewBag.stores = this.ToSelectTitle(warehouseRespository.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }), emptyTitle: "全部");
            return View();
        }
        [HttpPost]
        public ActionResult FindPageList()
        {
            int count;
            var list = deviceRepository.GetList(out count);
            return this.ToDataGrid(list, count);
        }

        public ActionResult FindPageByWhere(DeviceType machineType, string store, DeviceState status, string keyword)
        {
            int count;
            var list = deviceRepository.GetListByWhere(machineType, store, status, keyword, out count);
            return this.ToDataGrid(list, count);
        }


        [HttpPost]
        public ActionResult SetDevState(string ids, DeviceState state, string uid)
        {
            var result = deviceRepository.ChangeStatus(ids, state, uid);
            return this.ToJsonResult(result);
        }

        [HttpPost]
        public ActionResult SaveMemo(int id, string memo)
        {
            var result = deviceRepository.SetMemo(id, memo);
            return this.ToJsonResult(result);
        }
        [HttpGet]
        public ActionResult CreateSecurityCode()
        {
            ViewBag.stores = this.ToSelectTitle(warehouseRespository.GetList().Select(o => new SelectListItem() { Value = o.StoreId, Text = o.Title }));
            return View();
        }
        [HttpPost]
        public ActionResult CreateSecurityCode(string storeId)
        {
            var webSetting = webSettingRepository.GetReadOnlyEntities().FirstOrDefault();
            var store = warehouseRepository.GetReadOnlyEntities().FirstOrDefault(o => o.StoreId == storeId);
            var code = deviceService.GetSecurityCode(new StoreInformation() { CompanyId = 104, CompanyName = webSetting.CompanyFullTitle, CompanyShorterName = webSetting.CompanyTitle, StoreId = storeId, StoreName = store.Title, Timestamp = DateTime.Now });
            return Content(code);
        }
    }
}