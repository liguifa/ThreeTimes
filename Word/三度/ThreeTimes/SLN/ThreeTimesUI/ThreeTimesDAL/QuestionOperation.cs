using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThreeTimesCommon.Models;

namespace ThreeTimesDAL
{
    /// <summary>
    ///  数据库访问类UserOperation 对象表Question_T
    /// </summary>
    class QuestionOperation
    {
        ThreeTimesData db = new ThreeTimesData();

        #region 数据库查询+public List<Question_T> questionSelect(Expression<Func<Question_T, bool>> SelectWhere)
        /// <summary>
        /// 数据库查询
        /// </summary>
        /// <param name="SelectWhere"></param>
        /// <returns></returns>
        public List<Question_T> questionSelect(Expression<Func<Question_T, bool>> SelectWhere)
        {
            return db.Question_T.Where(SelectWhere).ToList();
        } 
        #endregion

        #region 数据库查询(排序)+public List<Question_T> questionSelect<TKey>(Expression<Func<Question_T, bool>> SelectWhere, Expression<Func<Question_T, TKey>> OrderLambda)
        /// <summary>
        /// 数据库查询(排序)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <returns></returns>
        public List<Question_T> questionSelect<TKey>(Expression<Func<Question_T, bool>> SelectWhere, Expression<Func<Question_T, TKey>> OrderLambda)
        {
            return db.Question_T.Where(SelectWhere).OrderBy(OrderLambda).ToList();
        } 
        #endregion

        #region 数据库查询(分页)+public List<Question_T> questionSelect<TKey>(Expression<Func<Question_T, bool>> SelectWhere, Expression<Func<Question_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        /// <summary>
        /// 数据库查询(分页)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="SelectWhere"></param>
        /// <param name="OrderLambda"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Question_T> questionSelect<TKey>(Expression<Func<Question_T, bool>> SelectWhere, Expression<Func<Question_T, TKey>> OrderLambda, int pageIndex, int pageSize)
        {
            return db.Question_T.Where(SelectWhere).OrderBy(OrderLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        } 
        #endregion

        #region 数据库增加+public int questionAdd(Question_T model)
        /// <summary>
        /// 数据库增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int questionAdd(Question_T model)
        {
            db.Question_T.Add(model);
            return db.SaveChanges();
        } 
        #endregion

        #region 数据库删除+public int questionDel(Expression<Func<Question_T, bool>> DelWhere)
        /// <summary>
        /// 数据库删除
        /// </summary>
        /// <param name="DelWhere"></param>
        /// <returns></returns>
        public int questionDel(Expression<Func<Question_T, bool>> DelWhere)
        {
            List<Question_T> question = db.Question_T.Where(DelWhere).ToList();
            question.ForEach(d => db.Question_T.Remove(d));
            return db.SaveChanges();
        }

        #endregion

        #region 数据库修改+public int questionModify(Question_T model, params string[] proNames)
        /// <summary>
        /// 数据库修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int questionModify(Question_T model, params string[] proNames)
        {
            DbEntityEntry entry = db.Entry<Question_T>(model);
            entry.State = EntityState.Unchanged;
            foreach (string proName in proNames)
            {
                entry.Property(proName).IsModified = true;
            }
            return db.SaveChanges();
        } 
        #endregion
    } 
}
