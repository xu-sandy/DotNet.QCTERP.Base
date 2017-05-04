namespace Qct.Domain.CommonObject
{
    /// <summary>
    /// 产品分类信息
    /// </summary>
    public class CategoryNode
    {
        /// <summary>
        /// 产品分类节点
        /// </summary>
        /// <param name="nodeId">节点ID</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodePath">节点路径</param>
        /// <param name="nodePathName">节点路径名称</param>
        public CategoryNode(int nodeId, string nodeName, string nodePath, string nodePathName)
        {
            Id = nodeId;
            Title = nodeName;
            CategoryPath = nodePath;
            CategoryPathTitle = nodePathName;
        }
        /// <summary>
        /// 分类id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 分类路径
        /// </summary>
        public string CategoryPath { get; set; }
        /// <summary>
        /// 分类路径名称
        /// </summary>
        public string CategoryPathTitle { get; set; }
        /// <summary>
        /// 判断分类是否为父节点
        /// </summary>
        /// <param name="category">分类</param>
        /// <returns></returns>
        public bool IsParentNode(CategoryNode category)
        {
            return CategoryPath.StartsWith(category.CategoryPath) && Id != category.Id;
        }
        /// <summary>
        /// 判断节点是否为父节点或者与相比节点为同一节点
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool IsParentOrSelfNode(CategoryNode category)
        {
            return CategoryPath.StartsWith(category.CategoryPath);
        }
        /// <summary>
        /// 判断节点是否为子节点
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool IsChildNode(CategoryNode category)
        {
            return category.CategoryPath.StartsWith(CategoryPath) && Id != category.Id;
        }
        /// <summary>
        /// 判断是否为子节点或者与相比节点为同一节点
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool IsChildOrSelfNode(CategoryNode category)
        {
            return category.CategoryPath.StartsWith(CategoryPath);
        }
        /// <summary>
        /// 重载比较方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is string)
            {
                return CategoryPath.Equals(obj);
            }
            if (obj is CategoryNode)
            {
                var category = obj as CategoryNode;
                return CategoryPath.Equals(category.CategoryPath);
            }
            return false;
        }
        /// <summary>
        /// 重载HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return CategoryPath.GetHashCode();
        }
    }
}
