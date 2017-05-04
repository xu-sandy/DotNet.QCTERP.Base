namespace Qct.Domain.CommonObject
{
    /// <summary>
    /// 品牌信息
    /// </summary>
    public class Brand
    {
        /// <summary>
        /// 品牌构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        public Brand(int id, string title)
        {
            Title = title;
            Id = id;
        }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 重载对象比较，支持int与Brand比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is int)
            {
                return Id.Equals(obj);
            }
            else if (obj is Brand)
            {
                var brand = obj as Brand;
                return Id.Equals(brand.Id);
            }
            return false;
        }
        /// <summary>
        /// 获取此实例的哈希代码
        /// </summary>
        /// <returns>返回此实例的哈希代码</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
