using Qct.ElectronicBalance;
using System;
using System.Collections.Generic;

namespace Qct.Settings
{
    /// <summary>
    /// 电子称功能设置
    /// </summary>
    public class ElectronicBalanceSetting
    {
        /// <summary>
        /// 是否启用电子称功能
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// 电子称工作模式
        /// </summary>
        public ElectronicBalanceWorkPattern WorkPattern { get; set; }
        /// <summary>
        /// 指定模式下的设置
        /// </summary>
        public Dictionary<PatternSettingItem, string> PatternSettings { get; set; }
        /// <summary>
        /// 获取默认配置
        /// </summary>
        public static ElectronicBalanceSetting Default
        {
            get
            { 
                var settings = new Dictionary<PatternSettingItem, string>();
                settings.Add(PatternSettingItem.Length, "13");
                settings.Add(PatternSettingItem.FlagCode, "27");
                settings.Add(PatternSettingItem.ProductCodeLength, "5");
                settings.Add(PatternSettingItem.WeightLength, "5");
                settings.Add(PatternSettingItem.WeightPrecision, "3");
                settings.Add(PatternSettingItem.CheckCode, "1");
                return new ElectronicBalanceSetting()
                {
                    Enable = false,
                    WorkPattern = ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight_CheckCode,
                    PatternSettings = settings
                };
            }
        }
        /// <summary>
        /// 是否可能为称重商品
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public bool MaybeWeightProduct(string barcode)
        {
            int len = 0;
            if (
                int.TryParse(PatternSettings[PatternSettingItem.Length], out len) && len == barcode.Length//处理长度
                && barcode.StartsWith(PatternSettings[PatternSettingItem.FlagCode])//处理标识
                )
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断条码是否由货号构成，是则返回货号
        /// </summary>
        /// <param name="weightBarcode"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        public bool IsProductCode(string weightBarcode, out string productCode)
        {
            productCode = string.Empty;
            switch (WorkPattern)
            {
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount_CheckCode:

                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Amount:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Amount_CheckCode:
                    {
                        var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                        var productCodeLength = int.Parse(PatternSettings[PatternSettingItem.ProductCodeLength]);
                        productCode = weightBarcode.Substring(flagCodeLength, productCodeLength);
                    }
                    break;
            }
            return string.IsNullOrWhiteSpace(productCode);
        }
        /// <summary>
        /// 判断条码是否为商品条码构成，是则返回商品条码
        /// </summary>
        /// <param name="weightBarcode"></param>
        /// <param name="productBarcode"></param>
        /// <returns></returns>
        public bool IsProductBarcode(string weightBarcode, out string productBarcode)
        {
            productBarcode = string.Empty;
            switch (WorkPattern)
            {
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount:
                case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount_CheckCode:

                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight_CheckCode:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Amount:
                case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Amount_CheckCode:
                    {
                        var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                        var productBarcodeLength = int.Parse(PatternSettings[PatternSettingItem.BarcodeLength]);
                        productBarcode = weightBarcode.Substring(flagCodeLength, productBarcodeLength);
                    }
                    break;
            }
            return string.IsNullOrWhiteSpace(productBarcode);
        }

        /// <summary>
        /// 获取称重条码 金额或者重量
        /// </summary>
        /// <param name="weightBarcode"></param>
        /// <param name="amount"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public bool ProductWeight(string weightBarcode, out decimal? amount, out decimal? weight)
        {
            amount = null;
            weight = null;
            try
            {
                switch (WorkPattern)
                {
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight:
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Amount_Weight_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var barcodeLength = int.Parse(PatternSettings[PatternSettingItem.BarcodeLength]);

                            var amountLength = int.Parse(PatternSettings[PatternSettingItem.AmountLength]);
                            var amountPrecision = int.Parse(PatternSettings[PatternSettingItem.AmountPrecision]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var amountText = weightBarcode.Substring(flagCodeLength + barcodeLength, amountLength);
                            var weightText = weightBarcode.Substring(flagCodeLength + barcodeLength + amountLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);
                            var amountPrecisionF = Math.Pow(0.1d, amountPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            amount = int.Parse(amountText) * (decimal)amountPrecisionF;
                            return true;
                        }
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount:
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_Barcode_Weight_Amount_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var barcodeLength = int.Parse(PatternSettings[PatternSettingItem.BarcodeLength]);

                            var amountLength = int.Parse(PatternSettings[PatternSettingItem.AmountLength]);
                            var amountPrecision = int.Parse(PatternSettings[PatternSettingItem.AmountPrecision]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var amountText = weightBarcode.Substring(flagCodeLength + barcodeLength + weightLength, amountLength);
                            var weightText = weightBarcode.Substring(flagCodeLength + barcodeLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);
                            var amountPrecisionF = Math.Pow(0.1d, amountPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            amount = int.Parse(amountText) * (decimal)amountPrecisionF;
                            return true;
                        }


                    case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight:
                    case ElectronicBalanceWorkPattern.Length13_FlagCode_Barcode_Weight_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var barcodeLength = int.Parse(PatternSettings[PatternSettingItem.BarcodeLength]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var weightText = weightBarcode.Substring(flagCodeLength + barcodeLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            return true;
                        }
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight:
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Amount_Weight_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var productCodeLength = int.Parse(PatternSettings[PatternSettingItem.ProductCodeLength]);

                            var amountLength = int.Parse(PatternSettings[PatternSettingItem.AmountLength]);
                            var amountPrecision = int.Parse(PatternSettings[PatternSettingItem.AmountPrecision]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var amountText = weightBarcode.Substring(flagCodeLength + productCodeLength, amountLength);
                            var weightText = weightBarcode.Substring(flagCodeLength + productCodeLength + amountLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);
                            var amountPrecisionF = Math.Pow(0.1d, amountPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            amount = int.Parse(amountText) * (decimal)amountPrecisionF;
                            return true;
                        }
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount:
                    case ElectronicBalanceWorkPattern.Length18_FlagCode_ProductCode_Weight_Amount_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var productCodeLength = int.Parse(PatternSettings[PatternSettingItem.ProductCodeLength]);

                            var amountLength = int.Parse(PatternSettings[PatternSettingItem.AmountLength]);
                            var amountPrecision = int.Parse(PatternSettings[PatternSettingItem.AmountPrecision]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var amountText = weightBarcode.Substring(flagCodeLength + productCodeLength + weightLength, amountLength);
                            var weightText = weightBarcode.Substring(flagCodeLength + productCodeLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);
                            var amountPrecisionF = Math.Pow(0.1d, amountPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            amount = int.Parse(amountText) * (decimal)amountPrecisionF;
                            return true;
                        }
                    case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight:
                    case ElectronicBalanceWorkPattern.Length13_FlagCode_ProductCode_Weight_CheckCode:
                        {
                            var flagCodeLength = PatternSettings[PatternSettingItem.FlagCode].Length;
                            var productCodeLength = int.Parse(PatternSettings[PatternSettingItem.ProductCodeLength]);

                            var weightLength = int.Parse(PatternSettings[PatternSettingItem.WeightLength]);
                            var weightPrecision = double.Parse(PatternSettings[PatternSettingItem.WeightPrecision]);

                            var weightText = weightBarcode.Substring(flagCodeLength + productCodeLength, weightLength);

                            var weightPrecisionF = Math.Pow(0.1d, weightPrecision);

                            weight = int.Parse(weightText) * (decimal)weightPrecisionF;
                            return true;
                        }
                }
            }
            catch { }
            return false;
        }
    }
}
