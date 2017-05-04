using Qct.Infrastructure.MessageClient.Implementations;
using System;
using System.IO;

namespace Qct.Infrastructure.MessageClient
{
    public class PublisherFactory
    {
        private const string PUBLISHERID = "PublisherID.Config";

        public static IPublisher Create()
        {
            return new EventPublisher(InitPublisherId());
        }
        /// <summary>
        /// 初始化事件发布器Id,如果文件存在则重新加载历史Id,否则新建并保存Id
        /// </summary>
        private static Guid InitPublisherId()
        {
            var assemblyFile = typeof(EventPublisher).Assembly.CodeBase;
            var assemblyFileLike = new Uri(assemblyFile);
            var assemblyDirectoryName = Path.GetDirectoryName(assemblyFileLike.AbsolutePath);
            var fileFullName = Path.Combine(assemblyDirectoryName, PUBLISHERID);
            lock (PUBLISHERID)
            {
                if (File.Exists(fileFullName))
                {
                    var publisherIdBytes = File.ReadAllBytes(fileFullName);
                    return new Guid(publisherIdBytes);
                }
                else
                {
                    var publisherId = Guid.NewGuid();
                    File.WriteAllBytes(fileFullName, publisherId.ToByteArray());
                    return publisherId;
                }
            }
        }
    }
}
