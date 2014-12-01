using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTimesCommon.Models;
using System.Linq.Expressions;
namespace ThreeTimesDAL
{   
    /// <summary>
    /// 数据库访问类：AdminOperation 对象表Admin_T
    /// </summary>
    class AdminOperation
    {
        ThreeTimesData db = new ThreeTimesData();
        #region 1.0 增加 +adminAdd(Admin_T model)
        /// <summary>
        /// 将实体通过EF传入数据库，返回受影响行数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int adminAdd(Admin_T model)
        {

            db.Aamin_T.Add(model);

            return db.SaveChanges();//保存成功后会将新增Id设置给model 并返回受影响行数
        } 
        #endregion
        #region 2.0 根据条件删除 +int adminDel(System.Linq.Expressions.Expression<Func<Admin_T, bool>> delWhere)
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="delWhere"></param>
        /// <returns></returns>
        public int adminDel(Expression<Func<Admin_T, bool>> delWhere)
        {

            List<Admin_T> listDeleting = db.Aamin_T.Where(delWhere).ToList();
            listDeleting.ForEach(u => db.Aamin_T.Remove(u));
            
            return db.SaveChanges();
        } 
        #endregion
        #region 3.0 根据属性修改实体 + int adminModify(Admin_T model, params string[] proNames)
        /// <summary>
        /// 根据属性修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int adminModify(Admin_T model, params string[] proNames)
        {

            DbEntityEntry entry = db.Entry<Admin_T>(model);
            entry.State = System.Data.EntityState.Unchanged;
            foreach (string proName in proNames)
            {
                entry.Property(proName).IsModified = true;
            }
            return db.SaveChanges();

        } 
        #endregion
        #region 4.0 查询+List<Admin_T> adminSelect(Expression<Func<Admin_T, bool>> whereLambada)
        /// <summary>
        /// 查询 返回列表
        /// </summary>
        /// <param name="whereLambada"></param>
        /// <returns></returns>
        public List<Admin_T> adminSelect(Expression<Func<Admin_T, bool>> whereLambda)
        {

            return db.Aamin_T.Where(whereLambda).ToList();
        } 
        #endregion
        #region 4.1 查询、可排序adminSelct<Tkey>(Expression<Func<Admin_T, bool>> whereLambda, Expression<Func<Admin_T, Tkey>> orderLambda)
        /// <summary>
        /// 查询、可排序
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public List<Admin_T> adminSelct<Tkey>(Expression<Func<Admin_T, bool>> whereLambda, Expression<Func<Admin_T, Tkey>> orderLambda)
        {
            return db.Aamin_T.Where(whereLambda).OrderBy(orderLambda).ToList();
        } 
        #endregion
        #region 4.2查询、返回分页数据 keySelect<TKey>(int pageIndex, int pageSize, Expression<Func<Admin_T, bool>> whereLambda, Expression<Func<Admin_T, TKey>> orderLambda)
        /// <summary>
        /// 查询、返回分页数据
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public List<Admin_T> keySelect<TKey>(int pageIndex, int pageSize, Expression<Func<Admin_T, bool>> whereLambda, Expression<Func<Admin_T, TKey>> orderLambda)
        {

            return db.Aamin_T.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    } 
        #endregion
}
