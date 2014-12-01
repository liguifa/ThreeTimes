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
    
    class KeyOperation
    {
        ThreeTimesData db = new ThreeTimesData();
        #region 1.0 新增实体 +int keyAdd(Key_T modle)
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public int keyAdd(Key_T modle)
        {
            db.Key_T.Add(modle);
            return db.SaveChanges();
        } 
        #endregion
        #region 2.0 根据条件删除+ int keyDel(Expression<Func<Key_T, bool>> whereDel)
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="whereDel"></param>
        /// <returns></returns>
        public int keyDel(Expression<Func<Key_T, bool>> whereDel)
        {
            List<Key_T> listDeleting = db.Key_T.Where(whereDel).ToList();
            listDeleting.ForEach(u => db.Key_T.Remove(u));
            return db.SaveChanges();
        } 
        #endregion
        #region 3.0 根据属性修改属性值 +int keyModify(Key_T modle, params string[] proNames)
        /// <summary>
        /// 根据属性修改属性值
        /// </summary>
        /// <param name="modle"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int keyModify(Key_T modle, params string[] proNames)
        {
            DbEntityEntry entity = db.Entry<Key_T>(modle);
            entity.State = System.Data.EntityState.Unchanged;
            foreach (string proName in proNames)
            {
                entity.Property(proName).IsModified = true;
            }
            return db.SaveChanges();
        } 
        #endregion
        #region 4.0 查询 返回列表 + List<Key_T> keySelect(Expression<Func<Key_T, bool>> whereLambda)
        /// <summary>
        /// 查询 返回列表
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<Key_T> keySelect(Expression<Func<Key_T, bool>> whereLambda)
        {

            return db.Key_T.Where(whereLambda).ToList();
        } 
        #endregion
        #region 4.1 重载查询+1 可排序 +List<Key_T> keySelect<TKey>(Expression<Func<Key_T, bool>> whereLambda, Expression<Func<Key_T, bool>> orderByLambda)
        /// <summary>
        ///重载查询+1
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderByLambda"></param>
        /// <returns></returns>
        public List<Key_T> keySelect<TKey>(Expression<Func<Key_T, bool>> whereLambda, Expression<Func<Key_T, bool>> orderByLambda)
        {

            return db.Key_T.Where(whereLambda).OrderBy(orderByLambda).ToList();
        }  
        #endregion
        #region 4.2 重载+2 分页查询+List<Key_T> keySelect<TKey>
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件Lambda</param>
        /// <param name="orderLambda">排序lambda</param>
        /// <returns></returns>
        public List<Key_T> keySelect<TKey>(int pageIndex, int pageSize, Expression<Func<Key_T, bool>> whereLambda, Expression<Func<Key_T, TKey>> orderLambda)
        {

            return db.Key_T.Where(whereLambda).OrderBy(orderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        } 
        #endregion
    }
}
