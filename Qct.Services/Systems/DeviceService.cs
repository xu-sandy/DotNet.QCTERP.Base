using Qct.ISevices.Systems;
using System;
using System.Linq;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Security;
using Qct.Infrastructure.Json;
using System.Collections.Generic;
using Qct.ISevices.Exceptions;

namespace Qct.Services.Systems
{
    public class DeviceService : IDeviceService
    {
        public string CreateMachineSn(IEnumerable<string> machineSns, int companyId, string storeId)
        {
            if (machineSns == null)
                throw new MachineSNOverflowException("注册设备失败,历史设备编号无法确认！");
            for (int i = 1; i < 100; i++)
            {
                var currentMachineSn = i.ToString("00");
                if (!machineSns.Contains(currentMachineSn))
                {
                    return currentMachineSn;
                }
            }
            throw new MachineSNOverflowException("注册设备失败，同门店下最多能注册99台设备！");
        }

        public string GetSecurityCode(StoreInformation storeInfo)
        {
            if (storeInfo == null)
                throw new Exception();
            var storeInfoText = JsonHelper.ToJson(storeInfo);

            var certificateContent = DES.DESEncryptBase64WithKeyIVToMd5Base64(storeInfoText, ConstValues.DESKEY, ConstValues.DESKEY);
            return certificateContent;
        }

        public StoreInformation DecryptSecurityCode(string certificateContent)
        {
            var text = DES.DESDecryptBase64WithKeyIVToMd5Base64(certificateContent, ConstValues.DESKEY, ConstValues.DESKEY);
            return JsonHelper.ToObject<StoreInformation>(text);
        }
    }
}
