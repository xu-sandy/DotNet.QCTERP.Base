using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qct.Persistance.Data;
using System.Linq;
using System.Collections.Generic;
using Qct.Objects.Entities;
using Qct.Repository;

namespace QCT.Objects.Test
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestJoinIn()
        {
            ProductRepository PR = new ProductRepository();
            PR.Get(100);

            using (DataContext dc = new DataContext())
            {
               //var product = dc.ProductRecords.FirstOrDefault(o=>o.ProductCode== "000016");
               //var sp= dc.ReadOnly<ProductGroupSplit>().FirstOrDefault();
                //Console.WriteLine(product);
            }
            //new SysMenusRepository().DeleteWithSaveChanges(new object[] { 3,4,5,6});
            //new SysMenusRepository().DeleteWithSaveChanges(1);
            //var ls= new NoticeRepository().GetEntities().ToList();
            var ls = new SysDictionaryRepository().GetEntities().ToList();
        }
    }
}
