模块设计开发者：余雄文
开发时间：2017-02-15
模块功能：EF封装

一、Repository封装说明及规范化
1）、Repository的主要职责是CRUD,所以通常情况下，他们的接口变化不大，所以提供默认的Repository的泛型化实现，减少部分重复繁琐的Repository定义
2）、Repository的泛型化实现并不能满足所有需求，在泛型接口无法满足需求时，应重新定义Repository的接口与实现，新接口可以继承IEFRepository<T>接口或者完成重新定义。
3）、DomainService在实现时不应该越权，实现数据库的CRUD操作，但是可以依赖与Repository提供完整的服务。
4）、Repository根据业务需求情况，提供基于DbContext【当业务无需做任何事务控制或者业务部分修改不需要事务控制时】或者IUnitOfWork【当业务需要事务控制时】的依赖注入
5）、操作Repository有条件的情况下，尽量使用IOC框架来减少代码耦合度。在没有条件的情况下，使用Repository接口调用（如： IRepository= new Repository();IRepository.Get(Id)）
二、基于指定实体接口规范定义的实体的好处
1）、能轻松实现Repository的泛型化实现，
2）、可以轻松实现内容管理，如数据同步
3）、规范不会污染实体
三、提供IUnitOfWork的必要性
1、可以提供单位事务控制
2、使用IUnitOfWork可以更好的对事务进行分步骤执行，容易扩展。